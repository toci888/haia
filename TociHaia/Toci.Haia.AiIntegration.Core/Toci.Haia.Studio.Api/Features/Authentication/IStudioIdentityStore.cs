namespace Toci.Haia.Studio.Api.Features.Authentication;

public interface IStudioIdentityStore
{
    Task<StudioIdentityRecord?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);

    Task<bool> UpdatePasswordHashAsync(Guid passwordCredentialId, string passwordHash, DateTime changedAtUtc, CancellationToken cancellationToken);

    Task<StudioSessionSnapshot?> GetSessionSnapshotAsync(Guid accountId, CancellationToken cancellationToken);
}
