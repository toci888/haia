using Toci.Haia.Studio.Api.Features.Authentication;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public sealed class StubStudioAuthenticationService : IStudioAuthenticationService
{
    public StudioLoginResult LoginResult { get; set; } = StudioLoginResult.Failed(StudioLoginFailureReason.AccountNotFound);

    public bool SessionIsValid { get; set; } = false;

    public Task<StudioLoginResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
        => Task.FromResult(LoginResult);

    public Task<bool> ValidateSessionAsync(Guid accountId, CancellationToken cancellationToken)
        => Task.FromResult(SessionIsValid);
}
