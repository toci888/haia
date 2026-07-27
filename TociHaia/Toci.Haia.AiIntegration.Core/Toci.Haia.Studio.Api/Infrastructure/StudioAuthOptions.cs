namespace Toci.Haia.Studio.Api.Infrastructure;

public sealed class StudioAuthOptions
{
    public const string SectionName = "StudioAuth";

    public string Authority { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty;
    public string RequiredScope { get; init; } = "studio.api";
    public string RequiredRole { get; init; } = "StudioAdmin";
}
