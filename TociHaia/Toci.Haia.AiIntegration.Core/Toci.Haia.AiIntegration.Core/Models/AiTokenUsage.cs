namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Zużycie tokenów raportowane przez providera AI.
/// </summary>
public sealed record AiTokenUsage(
    int? InputTokens,
    int? OutputTokens,
    int? TotalTokens);