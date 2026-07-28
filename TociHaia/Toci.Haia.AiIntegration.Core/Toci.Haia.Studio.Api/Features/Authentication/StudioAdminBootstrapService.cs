using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Toci.Haia.Database.Persistence.Context;
using Toci.Haia.Database.Persistence.Entities;
using Toci.Haia.Studio.Api.Infrastructure;

namespace Toci.Haia.Studio.Api.Features.Authentication;

public sealed class StudioAdminBootstrapService(
    HaiaDbContext db,
    TimeProvider timeProvider,
    IOptions<StudioPasswordPolicyOptions> passwordPolicyOptions,
    IStudioBootstrapConsole bootstrapConsole,
    ILogger<StudioAdminBootstrapService> logger) : IStudioAdminBootstrapService
{
    private const string ActiveAccountStatus = "active";
    private const string VerifiedEmailStatus = "verified";
    private const string StudioAdminRoleKey = "StudioAdmin";
    private const string StudioRoleScope = StudioScopeMapping.StudioRoleScope;
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        bootstrapConsole.Write("Podaj email administratora Studio: ");
        var emailInput = bootstrapConsole.ReadLine();
        if (string.IsNullOrWhiteSpace(emailInput))
        {
            throw new InvalidOperationException("Email nie może być pusty.");
        }

        var normalizedEmail = StudioAuthEmailNormalization.NormalizeEmail(emailInput);

        bootstrapConsole.Write("Podaj hasło administratora Studio: ");
        var password = bootstrapConsole.ReadSecret();
        bootstrapConsole.WriteLine(string.Empty);

        ValidatePassword(password, passwordPolicyOptions.Value);

        var now = timeProvider.GetUtcNow().UtcDateTime;

        var identity = await db.AccountEmails
            .Include(item => item.Account)
                .ThenInclude(account => account.PasswordCredential)
            .Include(item => item.Account)
                .ThenInclude(account => account.AccountRoleAssignmentAccounts)
                    .ThenInclude(assignment => assignment.AccountRole)
            .FirstOrDefaultAsync(item => item.EmailNormalized == normalizedEmail && item.ReleasedAt == null, cancellationToken);

        var account = identity?.Account;
        if (account is null)
        {
            account = new Account
            {
                AccountStatus = ActiveAccountStatus,
                CreatedAt = now,
                StatusChangedAt = now,
                ActivatedAt = now,
                RowVersion = 1,
            };
            db.Accounts.Add(account);
            await db.SaveChangesAsync(cancellationToken);

            db.AccountEmails.Add(new AccountEmail
            {
                AccountId = account.AccountId,
                EmailOriginal = emailInput.Trim(),
                EmailNormalized = normalizedEmail,
                IsPrimary = true,
                VerificationStatus = VerifiedEmailStatus,
                VerifiedAt = now,
                CreatedAt = now,
                UpdatedAt = now,
            });

            logger.LogInformation("Studio bootstrap: utworzono konto {AccountId}", account.AccountId);
        }
        else
        {
            account.AccountStatus = ActiveAccountStatus;
            account.ActivatedAt ??= now;
            account.StatusChangedAt = now;
        }

        var primaryEmail = await db.AccountEmails
            .FirstOrDefaultAsync(item => item.AccountId == account.AccountId && item.IsPrimary && item.ReleasedAt == null, cancellationToken);
        if (primaryEmail is not null)
        {
            primaryEmail.VerificationStatus = VerifiedEmailStatus;
            primaryEmail.VerifiedAt ??= now;
            primaryEmail.UpdatedAt = now;
        }

        var credential = await db.PasswordCredentials
            .FirstOrDefaultAsync(item => item.AccountId == account.AccountId && item.IsActive, cancellationToken);

        var accountForHasher = new Account { AccountId = account.AccountId, AccountStatus = ActiveAccountStatus };
        var passwordHash = _passwordHasher.HashPassword(accountForHasher, password);

        if (credential is null)
        {
            db.PasswordCredentials.Add(new PasswordCredential
            {
                AccountId = account.AccountId,
                PasswordHash = passwordHash,
                HashAlgorithm = "aspnetcore_passwordhasher_v3",
                HashFormatVersion = "v3",
                CreatedAt = now,
                ChangedAt = now,
                ForceChangeRequired = false,
                IsActive = true,
            });
        }
        else
        {
            credential.PasswordHash = passwordHash;
            credential.ChangedAt = now;
            credential.IsActive = true;
            credential.ForceChangeRequired = false;
        }

        var role = await db.AccountRoles
            .FirstOrDefaultAsync(item => item.RoleKey == StudioAdminRoleKey && item.RoleScope == StudioRoleScope, cancellationToken);

        if (role is null)
        {
            role = new AccountRole
            {
                RoleKey = StudioAdminRoleKey,
                RoleName = "Studio Administrator",
                RoleScope = StudioRoleScope,
                IsActive = true,
            };
            db.AccountRoles.Add(role);
            await db.SaveChangesAsync(cancellationToken);
        }
        else
        {
            role.IsActive = true;
        }

        var assignment = await db.AccountRoleAssignments
            .FirstOrDefaultAsync(item => item.AccountId == account.AccountId && item.AccountRoleId == role.AccountRoleId && item.IsActive, cancellationToken);

        if (assignment is null)
        {
            db.AccountRoleAssignments.Add(new AccountRoleAssignment
            {
                AccountId = account.AccountId,
                AccountRoleId = role.AccountRoleId,
                AssignedAt = now,
                IsActive = true,
            });
        }

        await db.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Studio bootstrap zakończony dla konta {AccountId}.", account.AccountId);
        bootstrapConsole.WriteLine("Bootstrap administratora Studio zakończony.");
    }

    private static void ValidatePassword(string password, StudioPasswordPolicyOptions options)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException("Hasło nie może być puste.");
        }

        if (password.Length < options.MinLength)
        {
            throw new InvalidOperationException($"Hasło musi mieć co najmniej {options.MinLength} znaków.");
        }

        if (options.RequireUppercase && !password.Any(char.IsUpper))
        {
            throw new InvalidOperationException("Hasło musi zawierać wielką literę.");
        }

        if (options.RequireLowercase && !password.Any(char.IsLower))
        {
            throw new InvalidOperationException("Hasło musi zawierać małą literę.");
        }

        if (options.RequireDigit && !password.Any(char.IsDigit))
        {
            throw new InvalidOperationException("Hasło musi zawierać cyfrę.");
        }

        if (options.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
        {
            throw new InvalidOperationException("Hasło musi zawierać znak specjalny.");
        }
    }
}
