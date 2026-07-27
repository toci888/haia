namespace Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

/// <summary>
/// Żądanie wygenerowania pakietu customowych reakcji z obrazu.
/// </summary>
public sealed record GenerateCustomReactionsFromImageRequest
{
    public required ImageInput Image { get; init; }

    public string? Caption { get; init; }

    public string? SituationDescription { get; init; }

    public string? EditorialHint { get; init; }

    public string OutputLanguage { get; init; } = "pl-PL";

    public string? AudienceProfessionContext { get; init; }

    public string? AudienceAgeRange { get; init; }

    public IReadOnlyList<string> HumorContextTags { get; init; } = [];

    public int PreferredIntensity { get; init; } = 3;

    public bool AllowMildProfanity { get; init; }

    public IReadOnlyList<string> ExcludedAreas { get; init; } = [];

    public int RequestedReactionCount { get; init; } = 8;

    public int DefaultVisibleReactionCount { get; init; } = 4;

    public bool IncludeSecondPunchlines { get; init; } = true;

    public string? EditorialDirection { get; init; }

    public string CorrelationId { get; init; } = Guid.NewGuid().ToString("N");

    public string? CandidateId { get; init; }
}