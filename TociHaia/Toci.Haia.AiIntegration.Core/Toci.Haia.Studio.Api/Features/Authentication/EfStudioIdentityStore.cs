using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence.Context;

namespace Toci.Haia.Studio.Api.Features.Authentication;

public sealed class EfStudioIdentityStore(HaiaDbContext db) : IStudioIdentityStore
{
    public async Task<StudioIdentityRecord?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        var account = await db.AccountEmails
            .Where(email => email.EmailNormalized == normalizedEmail && email.ReleasedAt == null)
            .Select(email => new
            {
                email.EmailOriginal,
                email.EmailNormalized,
                email.Account,
                ProfileNickname = email.Account.PublicProfile != null ? email.Account.PublicProfile.Nickname : null,
                Password = email.Account.PasswordCredential,
                Roles = email.Account.AccountRoleAssignmentAccounts
                    .Where(assignment => assignment.IsActive && assignment.RevokedAt == null && assignment.AccountRole.IsActive)
                    .Select(assignment => new { assignment.AccountRole.RoleKey, assignment.AccountRole.RoleScope })
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null || account.Password is null)
        {
            return null;
        }

        var displayName = string.IsNullOrWhiteSpace(account.ProfileNickname)
            ? account.EmailOriginal
            : account.ProfileNickname;

        var roles = account.Roles
            .Select(role => role.RoleKey)
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var scopes = account.Roles
            .Select(role => role.RoleScope)
            .Where(scope => !string.IsNullOrWhiteSpace(scope))
            .Select(StudioScopeMapping.ToClaimScope)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return new StudioIdentityRecord(
            AccountId: account.Account.AccountId,
            AccountStatus: account.Account.AccountStatus,
            EmailOriginal: account.EmailOriginal,
            EmailNormalized: account.EmailNormalized,
            DisplayName: displayName,
            PasswordCredentialId: account.Password.PasswordCredentialId,
            PasswordHash: account.Password.PasswordHash,
            PasswordCredentialIsActive: account.Password.IsActive,
            Roles: roles,
            Scopes: scopes);
    }

    public async Task<bool> UpdatePasswordHashAsync(Guid passwordCredentialId, string passwordHash, DateTime changedAtUtc, CancellationToken cancellationToken)
    {
        var credential = await db.PasswordCredentials
            .FirstOrDefaultAsync(item => item.PasswordCredentialId == passwordCredentialId, cancellationToken);

        if (credential is null)
        {
            return false;
        }

        credential.PasswordHash = passwordHash;
        credential.ChangedAt = changedAtUtc;

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<StudioSessionSnapshot?> GetSessionSnapshotAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await db.Accounts
            .Where(item => item.AccountId == accountId)
            .Select(item => new
            {
                item.AccountId,
                item.AccountStatus,
                ActiveRoles = item.AccountRoleAssignmentAccounts
                    .Where(assignment => assignment.IsActive && assignment.RevokedAt == null && assignment.AccountRole.IsActive)
                    .Select(assignment => new
                    {
                        assignment.AccountRole.RoleKey,
                        assignment.AccountRole.RoleScope,
                    })
                    .ToList(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return null;
        }

        var hasRequiredRole = account.ActiveRoles.Any(role => string.Equals(role.RoleKey, "StudioAdmin", StringComparison.Ordinal));
        var hasRequiredScope = account.ActiveRoles
            .Select(role => StudioScopeMapping.ToClaimScope(role.RoleScope))
            .Any(scope => string.Equals(scope, StudioScopeMapping.StudioApiScope, StringComparison.Ordinal));

        return new StudioSessionSnapshot(
            AccountId: account.AccountId,
            AccountStatus: account.AccountStatus,
            HasRequiredRole: hasRequiredRole,
            HasRequiredScope: hasRequiredScope);
    }
}
