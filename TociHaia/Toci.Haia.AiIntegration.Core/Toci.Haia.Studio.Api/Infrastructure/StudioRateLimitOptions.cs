namespace Toci.Haia.Studio.Api.Infrastructure;

public sealed class StudioRateLimitOptions
{
    public const string SectionName = "RateLimiting";
    public int StudioDefaultPermitLimit { get; init; } = 60;
    public int WindowSeconds { get; init; } = 60;
}
