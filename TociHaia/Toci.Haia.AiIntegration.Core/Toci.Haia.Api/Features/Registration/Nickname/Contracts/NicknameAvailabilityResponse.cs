namespace Toci.Haia.Api.Features.Registration.Nickname.Contracts;

public sealed record NicknameAvailabilityResponse(bool Available, string NormalizedNickname);
