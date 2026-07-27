namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Opisuje pojedynczy problem walidacji requestu lub odpowiedzi.
/// </summary>
public sealed record ValidationIssue(string Path, string Message);