namespace Toci.Haia.AiIntegration.OpenAI.Configuration;

/// <summary>
/// Konfiguracja integracji HAIA z OpenAI.
/// </summary>
public sealed record OpenAiIntegrationOptions
{
    public const string SectionName = "TociHaia:AiIntegration:OpenAI";

    public string? ApiKey { get; init; }

    public string? Model { get; init; }

    public Uri? Endpoint { get; init; }

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(45);

    public int MaxRetryAttempts { get; init; } = 2;

    public int MaxImageBytes { get; init; } = 5 * 1024 * 1024;

    public int DefaultReactionCount { get; init; } = 8;

    public int MaxReactionCount { get; init; } = 12;

    public int MaxRepairAttempts { get; init; } = 1;

    public bool EnableMetadataLogging { get; init; } = true;

    public string? ImageDetailLevel { get; init; }

    public bool EnableSemanticRepair { get; init; } = true;
}