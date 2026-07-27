using System.ComponentModel.DataAnnotations;

namespace Toci.Haia.Api.Features.Registration.Email;

public sealed class EmailRegistrationOptions
{
    public const string SectionName = "EmailRegistration";

    [Range(1, 168)]
    public int TokenLifetimeHours { get; init; } = 24;

    [Range(1, 3600)]
    public int ResendCooldownSeconds { get; init; } = 60;

    [Range(8, 128)]
    public int MinimumPasswordLength { get; init; } = 12;

    [Range(12, 256)]
    public int MaximumPasswordLength { get; init; } = 128;

    [Required]
    public string DefaultLanguage { get; init; } = "pl-PL";
}
