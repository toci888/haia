namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

using Toci.Haia.Database.Persistence.Entities;

public interface IMemeIntakeStore
{
    Task<MemeIntakeDraft> CreateDraftAsync(
        Guid accountId,
        string fileName,
        string contentType,
        long sizeBytes,
        string? workingTitle,
        CancellationToken cancellationToken);

    Task<MemeIntakeAggregate?> GetByIntakeIdAsync(Guid intakeId, CancellationToken cancellationToken);

    Task<bool> IsDuplicateUploadAsync(Guid intakeId, string sha256Hash, CancellationToken cancellationToken);

    Task MarkUploadedAsync(Guid intakeId, string sha256Hash, int? widthPx, int? heightPx, CancellationToken cancellationToken);

    Task SaveEvaluationAsync(
        Guid intakeId,
        Guid aiOperationExecutionId,
        string structuredOutputJson,
        string mappedOutputJson,
        CancellationToken cancellationToken);

    Task SaveFailedEvaluationAsync(
        Guid intakeId,
        Guid aiOperationExecutionId,
        string errorCode,
        string structuredOutputJson,
        CancellationToken cancellationToken);

    Task<ClassificationTaxonomySnapshot> GetActiveTaxonomyAsync(CancellationToken cancellationToken);

    Task<OnboardingCandidateVersion?> GetCandidateVersionAsync(Guid intakeId, CancellationToken cancellationToken);

    Task<OnboardingCandidateVersion?> GetCandidateVersionByIdAsync(Guid candidateVersionId, CancellationToken cancellationToken);

    Task SaveClassificationAsync(Guid intakeId, Guid aiOperationExecutionId, PersistedMemeEvaluation evaluation, CancellationToken cancellationToken);

    Task<EditorialReviewSnapshot> StartEditorialReviewAsync(Guid intakeId, Guid reviewerAccountId, CancellationToken cancellationToken);

    Task<EditorialReviewSnapshot> ApproveAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        string? summary,
        string? editorialNote,
        CancellationToken cancellationToken);

    Task<EditorialReviewSnapshot> RejectAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        string? summary,
        string? editorialNote,
        CancellationToken cancellationToken);
}

public sealed record MemeIntakeDraft(
    Guid IntakeId,
    Guid CandidateId,
    Guid CandidateVersionId,
    Guid AssetId,
    string ObjectKey,
    string FileName,
    string ContentType,
    long SizeBytes,
    string? WorkingTitle,
    string Status);

public sealed record MemeIntakeAggregate(
    Guid IntakeId,
    string Status,
    string FileName,
    string ContentType,
    long SizeBytes,
    string ObjectKey,
    string? WorkingTitle,
    CandidateClassification? CandidateClassification,
    string? LatestMappedOutputJson,
    string? LatestStructuredOutputJson);

public sealed record EditorialReviewSnapshot(
    Guid IntakeId,
    Guid CandidateClassificationId,
    string CandidateStatus,
    string EditorialStatus,
    string ClassificationStatus,
    string ReviewStatus,
    Guid? ReviewedByAccountId,
    DateTimeOffset? ReviewedAtUtc);

public sealed record ClassificationTaxonomySnapshot(
    ClassificationModelVersion ActiveModel,
    IReadOnlyDictionary<string, ClassificationModelAxis> AxisByKey,
    IReadOnlyDictionary<(string AxisKey, string ValueKey), ClassificationModelValue> ValueByAxisAndKey,
    IReadOnlyDictionary<string, SensitivityCategory> SensitivityByKey,
    IReadOnlyDictionary<int, DrynessScaleLevel> DrynessLevelsByNo,
    IReadOnlyDictionary<string, ReactionMechanism> ReactionMechanismsByKey);

public sealed record PersistedMemeEvaluation(
    decimal OverallConfidence,
    string SuggestedTitle,
    string VisualDescription,
    string? DetectedText,
    string LanguageCode,
    string ContentFormat,
    string SuggestedRoleInFlow,
    decimal SuggestedRoleConfidence,
    string? EditorialSummary,
    int PredictedDrynessLevel,
    decimal PredictedDrynessConfidence,
    string ModerationRecommendation,
    string? ModerationSummary,
    IReadOnlyList<PersistedMemeClassificationValue> Classifications,
    IReadOnlyList<PersistedMemeMeasureValue> Measures,
    IReadOnlyList<PersistedMemeSafetyValue> Safety,
    IReadOnlyList<PersistedMemeReactionValue> Reactions);

public sealed record PersistedMemeClassificationValue(
    string AxisKey,
    string ValueKey,
    decimal RelevanceScore,
    decimal Confidence,
    bool IsPrimary,
    int? RankNo);

public sealed record PersistedMemeMeasureValue(
    string AxisKey,
    decimal NormalizedValue,
    decimal Confidence);

public sealed record PersistedMemeSafetyValue(
    string CategoryKey,
    short SeverityLevel,
    decimal Confidence,
    bool ModerationRelevance);

public sealed record PersistedMemeReactionValue(
    string Text,
    int DisplayOrder,
    bool InitiallyVisible,
    decimal RelevanceScore,
    decimal Confidence,
    string SecondPunchline,
    IReadOnlyList<string> MechanismKeys);
