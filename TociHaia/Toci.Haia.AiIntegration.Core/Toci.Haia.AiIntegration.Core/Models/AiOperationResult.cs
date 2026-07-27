namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Typowany rezultat operacji AI.
/// </summary>
public sealed record AiOperationResult<T>
{
    public required bool IsSuccess { get; init; }

    public T? Value { get; init; }

    public AiOperationError? Error { get; init; }

    public required AiOperationMetadata Metadata { get; init; }

    public static AiOperationResult<T> Success(T value, AiOperationMetadata metadata) =>
        new()
        {
            IsSuccess = true,
            Value = value,
            Metadata = metadata,
        };

    public static AiOperationResult<T> Failure(AiOperationError error, AiOperationMetadata metadata) =>
        new()
        {
            IsSuccess = false,
            Error = error,
            Metadata = metadata,
        };
}