namespace Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

/// <summary>
/// Bezpieczna reprezentacja obrazu wejściowego do operacji multimodalnej.
/// </summary>
public sealed record ImageInput(
    byte[] Content,
    string MimeType,
    string? FileName = null);