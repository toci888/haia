namespace Toci.Haia.AiIntegration.Core.Abstractions;

using Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Waliduje odpowiedź operacji AI po stronie HAIA.
/// </summary>
public interface IAiResponseValidator<in TRequest, in TResponse>
{
    IReadOnlyList<ValidationIssue> Validate(TRequest request, TResponse response, CancellationToken cancellationToken = default);
}