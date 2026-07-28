namespace Toci.Haia.Studio.Api.Infrastructure;

public sealed class StudioPasswordPolicyOptions
{
    public const string SectionName = "StudioPasswordPolicy";

    public int MinLength { get; init; } = 12;
    public bool RequireUppercase { get; init; } = true;
    public bool RequireLowercase { get; init; } = true;
    public bool RequireDigit { get; init; } = true;
    public bool RequireNonAlphanumeric { get; init; } = true;
}
