namespace Toci.Haia.Api.Features.Registration.Email;

public sealed class EmailRegistrationRateLimitOptions
{
    public const string SectionName = "RateLimiting";

    public int RegistrationPermitLimit { get; init; } = 10;
    public int VerificationPermitLimit { get; init; } = 20;
    public int ResendPermitLimit { get; init; } = 10;
    public int NicknamePermitLimit { get; init; } = 30;
    public int WindowSeconds { get; init; } = 60;
}
