using Toci.Haia.Api.Features.Registration.Email;
using Toci.Haia.Api.Features.Registration.Nickname.Contracts;

namespace Toci.Haia.Api.Features.Registration.Nickname;

public sealed class EfNicknameAvailabilityService(IEmailRegistrationStore store) : INicknameAvailabilityService
{
    public async Task<NicknameAvailabilityResponse> CheckAsync(string nickname, CancellationToken cancellationToken)
    {
        var normalized = EmailRegistrationNormalization.NormalizeNickname(nickname);
        var taken = await store.ExistsActiveNicknameAsync(normalized, cancellationToken);
        return new NicknameAvailabilityResponse(!taken, normalized);
    }
}
