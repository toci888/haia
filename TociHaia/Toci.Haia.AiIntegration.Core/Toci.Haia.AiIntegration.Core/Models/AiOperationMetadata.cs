namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Metadane wykonania operacji AI na potrzeby audytu i diagnostyki.
/// </summary>
public sealed record AiOperationMetadata
{
    public required string OperationName { get; init; }

    public required string CorrelationId { get; init; }

    public required string Provider { get; init; }

    public required string Model { get; init; }

    public string? ProviderResponseId { get; init; }

    public required string PromptTemplateId { get; init; }

    public required string PromptTemplateVersion { get; init; }

    public required DateTimeOffset StartedAtUtc { get; init; }

    public required DateTimeOffset FinishedAtUtc { get; init; }

    public required TimeSpan Duration { get; init; }

    public AiTokenUsage? TokenUsage { get; init; }

    public int RetryCount { get; init; }

    public int RepairCount { get; init; }

    public AiExecutionStatus Status { get; init; }
}