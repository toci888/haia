namespace Toci.Haia.Studio.Api.Infrastructure;

public sealed class StudioCookieAuthOptions
{
    public const string SectionName = "StudioCookieAuth";
    public int SessionMinutes { get; init; } = 60;
    public string CookieName { get; init; } = "haia.studio.session";
}
