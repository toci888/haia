namespace Toci.Haia.Studio.Api.Features.Authentication;

public sealed record StudioIdentityRecord(
    Guid AccountId,
    string AccountStatus,
    string EmailOriginal,
    string EmailNormalized,
    string DisplayName,
    Guid PasswordCredentialId,
    string PasswordHash,
    bool PasswordCredentialIsActive,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Scopes);

public sealed record StudioAuthenticatedSession(
    Guid AccountId,
    string DisplayName,
    string Email,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Scopes,
    string AccountStatus);

public enum StudioLoginFailureReason
{
    InvalidInput,
    AccountNotFound,
    InactiveAccount,
    MissingCredential,
    InvalidPassword,
    MissingStudioAccess,
}

public sealed record StudioLoginResult(
    bool Succeeded,
    StudioAuthenticatedSession? Session,
    StudioLoginFailureReason? FailureReason)
{
    public static StudioLoginResult Failed(StudioLoginFailureReason reason) => new(false, null, reason);

    public static StudioLoginResult Success(StudioAuthenticatedSession session) => new(true, session, null);
}

public sealed record StudioSessionSnapshot(
    Guid AccountId,
    string AccountStatus,
    bool HasRequiredRole,
    bool HasRequiredScope);
