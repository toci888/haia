namespace Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Kody błędów operacji AI używane w kontrakcie Core.
/// </summary>
public enum AiErrorCode
{
    ValidationFailed,
    ConfigurationMissing,
    AuthenticationFailed,
    RateLimited,
    ProviderUnavailable,
    Timeout,
    Cancelled,
    ContentRefused,
    InvalidStructuredOutput,
    SemanticValidationFailed,
    UnsupportedImageType,
    ImageTooLarge,
    UnknownProviderError,
}