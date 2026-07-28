namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

public sealed record CreateMemeIntakeRequest(
    string FileName,
    string ContentType,
    long SizeBytes,
    string? WorkingTitle);

public sealed record CreateMemeIntakeResponse(
    Guid IntakeId,
    Guid CandidateId,
    Guid CandidateVersionId,
    Guid AssetId,
    string ObjectKey,
    string UploadUrl,
    IReadOnlyDictionary<string, string> UploadHeaders,
    DateTimeOffset UploadUrlExpiresAtUtc,
    string Status);

public sealed record FinalizeMemeIntakeResponse(
    Guid IntakeId,
    string Status,
    string ContentType,
    long SizeBytes,
    string Sha256Hash);

public sealed record EvaluateMemeIntakeResponse(
    Guid IntakeId,
    string Status,
    MemeEvaluationResultDto Evaluation);

public sealed record StartEditorialReviewResponse(
    Guid IntakeId,
    string Status,
    string EditorialStatus,
    string ClassificationStatus,
    Guid? CandidateClassificationId);

public sealed record MemeEditorialDecisionRequest(
    string? Summary,
    string? EditorialNote);

public sealed record MemeEditorialDecisionResponse(
    Guid IntakeId,
    string Status,
    string EditorialStatus,
    string ClassificationStatus,
    string ReviewStatus,
    Guid CandidateClassificationId,
    DateTimeOffset ReviewedAtUtc,
    Guid ReviewedByAccountId);

public sealed record GetMemeIntakeResponse(
    Guid IntakeId,
    string Status,
    MemeAssetDto Asset,
    MemeEvaluationResultDto? Evaluation,
    string? WorkingTitle);

public sealed record MemeAssetDto(
    string FileName,
    string ContentType,
    long SizeBytes,
    string ObjectKey);

public sealed record MemeEvaluationResultDto
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

    public required IReadOnlyList<MemeClassificationDto> Classifications { get; init; }

    public required IReadOnlyList<MemeMeasureDto> Measures { get; init; }

    public required IReadOnlyList<MemeSafetyDto> Safety { get; init; }

    public required string ModerationRecommendation { get; init; }

    public string? ModerationSummary { get; init; }

    public int PredictedDrynessLevel { get; init; }

    public required string PredictedDrynessLabel { get; init; }

    public decimal PredictedDrynessConfidence { get; init; }

    public required IReadOnlyList<MemeReactionDto> Reactions { get; init; }
}

public sealed record MemeClassificationDto(
    string AxisKey,
    string AxisDisplayName,
    string ValueKey,
    string ValueDisplayName,
    decimal RelevanceScore,
    decimal Confidence,
    bool IsPrimary,
    int? RankNo);

public sealed record MemeMeasureDto(
    string AxisKey,
    string AxisDisplayName,
    decimal NormalizedValue,
    decimal Confidence);

public sealed record MemeSafetyDto(
    string CategoryKey,
    string CategoryDisplayName,
    short SeverityLevel,
    decimal Confidence,
    bool ModerationRelevance);

public sealed record MemeReactionDto(
    string Text,
    int DisplayOrder,
    bool InitiallyVisible,
    decimal RelevanceScore,
    decimal Confidence,
    string SecondPunchline,
    IReadOnlyList<string> MechanismKeys);
