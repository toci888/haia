namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Status wykonania operacji AI.
/// </summary>
public enum AiExecutionStatus
{
    Succeeded,
    Failed,
    Cancelled,
    Refused,
}