namespace Toci.Haia.AiIntegration.Core.Abstractions;

using Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Waliduje żądanie operacji AI po stronie HAIA.
/// </summary>
public interface IAiRequestValidator<in TRequest>
{
    IReadOnlyList<ValidationIssue> Validate(TRequest request, CancellationToken cancellationToken = default);
}