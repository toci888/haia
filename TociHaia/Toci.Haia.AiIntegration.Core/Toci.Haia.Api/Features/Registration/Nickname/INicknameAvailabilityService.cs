using Toci.Haia.Api.Features.Registration.Nickname.Contracts;

namespace Toci.Haia.Api.Features.Registration.Nickname;

public interface INicknameAvailabilityService
{
    Task<NicknameAvailabilityResponse> CheckAsync(string nickname, CancellationToken cancellationToken);
}
