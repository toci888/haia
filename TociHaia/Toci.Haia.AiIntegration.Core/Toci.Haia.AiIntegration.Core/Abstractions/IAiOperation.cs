namespace Toci.Haia.AiIntegration.Core.Abstractions;

using Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Reprezentuje typowaną operację AI wykonywaną asynchronicznie.
/// </summary>
public interface IAiOperation<in TRequest, TResponse>
{
    Task<AiOperationResult<TResponse>> ExecuteAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}