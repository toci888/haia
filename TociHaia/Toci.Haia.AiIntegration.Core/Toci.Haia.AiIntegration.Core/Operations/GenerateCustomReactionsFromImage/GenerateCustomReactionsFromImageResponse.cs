namespace Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

using Toci.Haia.AiIntegration.Core.Operations;

/// <summary>
/// Typowana odpowiedź operacji generowania reakcji z obrazu.
/// </summary>
public sealed record GenerateCustomReactionsFromImageResponse
{
    public required string MaterialSummary { get; init; }

    public IReadOnlyList<string> DetectedHumorMechanisms { get; init; } = [];

    public IReadOnlyList<string> RequiredContexts { get; init; } = [];

    public bool RescuePotentialDetected { get; init; }

    public IReadOnlyList<string> EditorialWarnings { get; init; } = [];

    public IReadOnlyList<string> SafetyFlags { get; init; } = [];

    public required IReadOnlyList<GeneratedReaction> Reactions { get; init; }

    public EditorialStatus EditorialStatus { get; init; } = EditorialStatus.GeneratedAwaitingEditorialReview;

    public string? MaterialHashSha256 { get; init; }
}

/// <summary>
/// Pojedyncza reakcja wygenerowana przez AI.
/// </summary>
public sealed record GeneratedReaction
{
    public int Order { get; init; }

    public required string Label { get; init; }

    public string? Emoji { get; init; }

    public string? SecondPunchline { get; init; }

    public required string Style { get; init; }

    public IReadOnlyList<string> HumorMechanisms { get; init; } = [];

    public int Intensity { get; init; }

    public bool IsRescueReaction { get; init; }

    public string? EditorialNote { get; init; }

    public IReadOnlyList<string> SafetyTags { get; init; } = [];

    public bool IsDefaultVisible { get; init; }
}