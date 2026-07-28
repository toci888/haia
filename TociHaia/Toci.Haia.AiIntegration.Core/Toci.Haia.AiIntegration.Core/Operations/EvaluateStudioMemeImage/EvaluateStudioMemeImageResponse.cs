namespace Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;

public sealed record EvaluateStudioMemeImageResponse
{
    public required string SuggestedTitle { get; init; }

    public required string VisualDescription { get; init; }

    public string? DetectedText { get; init; }

    public required string LanguageCode { get; init; }

    public required string ContentFormat { get; init; }

    public required string SuggestedRoleInFlow { get; init; }

    public decimal SuggestedRoleConfidence { get; init; }

    public string? EditorialSummary { get; init; }

    public decimal OverallConfidence { get; init; }

    public required IReadOnlyList<HumorClassificationValue> HumorClassifications { get; init; }

    public required IReadOnlyList<HumorMeasureValue> NumericMeasures { get; init; }

    public required IReadOnlyList<SafetyClassificationValue> Safety { get; init; }

    public required string ModerationRecommendation { get; init; }

    public string? ModerationSummary { get; init; }

    public int PredictedDrynessLevel { get; init; }

    public decimal PredictedDrynessConfidence { get; init; }

    public required IReadOnlyList<MemeReactionProposal> Reactions { get; init; }
}

public sealed record HumorClassificationValue
{
    public required string AxisKey { get; init; }

    public required string ValueKey { get; init; }

    public decimal RelevanceScore { get; init; }

    public decimal Confidence { get; init; }

    public bool IsPrimary { get; init; }

    public int? RankNo { get; init; }
}

public sealed record HumorMeasureValue
{
    public required string AxisKey { get; init; }

    public decimal NormalizedValue { get; init; }

    public decimal Confidence { get; init; }
}

public sealed record SafetyClassificationValue
{
    public required string CategoryKey { get; init; }

    public short SeverityLevel { get; init; }

    public decimal Confidence { get; init; }

    public bool ModerationRelevance { get; init; }
}

public sealed record MemeReactionProposal
{
    public required string Text { get; init; }

    public int DisplayOrder { get; init; }

    public bool InitiallyVisible { get; init; }

    public decimal RelevanceScore { get; init; }

    public decimal Confidence { get; init; }

    public required string SecondPunchline { get; init; }

    public required IReadOnlyList<string> MechanismKeys { get; init; }
}
