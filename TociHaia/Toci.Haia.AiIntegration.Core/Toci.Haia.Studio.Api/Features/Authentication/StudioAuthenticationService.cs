using Microsoft.AspNetCore.Identity;
using Toci.Haia.Database.Persistence.Entities;

namespace Toci.Haia.Studio.Api.Features.Authentication;

public sealed class StudioAuthenticationService(
    IStudioIdentityStore identityStore,
    TimeProvider timeProvider,
    ILogger<StudioAuthenticationService> logger) : IStudioAuthenticationService
{
    private const string ActiveAccountStatus = "active";
    private const string RequiredRole = "StudioAdmin";
    private const string RequiredScope = StudioScopeMapping.StudioApiScope;
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public async Task<StudioLoginResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            return StudioLoginResult.Failed(StudioLoginFailureReason.InvalidInput);
        }

        var normalizedEmail = StudioAuthEmailNormalization.NormalizeEmail(email);
        var identity = await identityStore.FindByNormalizedEmailAsync(normalizedEmail, cancellationToken);
        if (identity is null)
        {
            logger.LogInformation("Studio login failed: account not found for normalized email");
            return StudioLoginResult.Failed(StudioLoginFailureReason.AccountNotFound);
        }

        if (!string.Equals(identity.AccountStatus, ActiveAccountStatus, StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("Studio login rejected for account {AccountId} with status {AccountStatus}", identity.AccountId, identity.AccountStatus);
            return StudioLoginResult.Failed(StudioLoginFailureReason.InactiveAccount);
        }

        if (!identity.PasswordCredentialIsActive)
        {
            logger.LogWarning("Studio login rejected for account {AccountId}: inactive password credential", identity.AccountId);
            return StudioLoginResult.Failed(StudioLoginFailureReason.MissingCredential);
        }

        var accountForHasher = new Account { AccountId = identity.AccountId, AccountStatus = identity.AccountStatus };
        var verification = _passwordHasher.VerifyHashedPassword(accountForHasher, identity.PasswordHash, password);
        if (verification is PasswordVerificationResult.Failed)
        {
            logger.LogInformation("Studio login failed for account {AccountId}: invalid password", identity.AccountId);
            return StudioLoginResult.Failed(StudioLoginFailureReason.InvalidPassword);
        }

        var hasRequiredRole = identity.Roles.Any(role => string.Equals(role, RequiredRole, StringComparison.Ordinal));
        var hasRequiredScope = identity.Scopes.Any(scope => string.Equals(scope, RequiredScope, StringComparison.Ordinal));
        if (!hasRequiredRole || !hasRequiredScope)
        {
            logger.LogInformation("Studio login denied for account {AccountId}: missing role/scope", identity.AccountId);
            return StudioLoginResult.Failed(StudioLoginFailureReason.MissingStudioAccess);
        }

        if (verification is PasswordVerificationResult.SuccessRehashNeeded)
        {
            var newHash = _passwordHasher.HashPassword(accountForHasher, password);
            await identityStore.UpdatePasswordHashAsync(identity.PasswordCredentialId, newHash, timeProvider.GetUtcNow().UtcDateTime, cancellationToken);
        }

        var session = new StudioAuthenticatedSession(
            AccountId: identity.AccountId,
            DisplayName: identity.DisplayName,
            Email: identity.EmailOriginal,
            Roles: identity.Roles,
            Scopes: identity.Scopes,
            AccountStatus: identity.AccountStatus);

        logger.LogInformation("Studio login succeeded for account {AccountId}", identity.AccountId);
        return StudioLoginResult.Success(session);
    }

    public async Task<bool> ValidateSessionAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var snapshot = await identityStore.GetSessionSnapshotAsync(accountId, cancellationToken);
        if (snapshot is null)
        {
            return false;
        }

        if (!string.Equals(snapshot.AccountStatus, ActiveAccountStatus, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return snapshot.HasRequiredRole && snapshot.HasRequiredScope;
    }
}
