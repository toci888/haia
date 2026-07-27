namespace Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

/// <summary>
/// Ustawienia walidacji requestu i odpowiedzi dla operacji custom reactions.
/// </summary>
public sealed record GenerateCustomReactionsValidationSettings
{
    public int MaxImageBytes { get; init; } = 5 * 1024 * 1024;

    public int DefaultReactionCount { get; init; } = 8;

    public int MaxReactionCount { get; init; } = 12;

    public int MaxLabelLength { get; init; } = 64;

    public int MaxSecondPunchlineLength { get; init; } = 180;

    public int MaxEditorialNoteLength { get; init; } = 240;
}