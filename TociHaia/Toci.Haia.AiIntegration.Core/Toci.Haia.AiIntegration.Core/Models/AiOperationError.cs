namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Typowany błąd operacji AI zwracany do warstwy wywołującej.
/// </summary>
public sealed record AiOperationError(
    AiErrorCode Code,
    string Message,
    bool IsTransient = false,
    string? ProviderErrorCode = null,
    Exception? InnerException = null);