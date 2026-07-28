namespace Toci.Haia.Studio.Api.Features.Authentication;

public interface IStudioAuthenticationService
{
    Task<StudioLoginResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);

    Task<bool> ValidateSessionAsync(Guid accountId, CancellationToken cancellationToken);
}
