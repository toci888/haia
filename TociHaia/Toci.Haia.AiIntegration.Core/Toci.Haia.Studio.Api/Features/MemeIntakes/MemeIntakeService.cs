namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

using System.Security.Cryptography;
using System.Text.Json;
using Toci.Haia.Studio.Api.Common.Errors;

public sealed class MemeIntakeService(
    IMemeIntakeStore store,
    IMediaObjectStorage storage,
    IMemeEvaluationService evaluationService,
    MemeIntakeOptions options) : IMemeIntakeService
{
    private readonly HashSet<string> _allowedContentTypes = options.AllowedContentTypes.ToHashSet(StringComparer.OrdinalIgnoreCase);

    public async Task<CreateMemeIntakeResponse> CreateAsync(Guid accountId, CreateMemeIntakeRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FileName))
        {
            throw Validation("FileName is required.");
        }

        if (!_allowedContentTypes.Contains(request.ContentType))
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.UnsupportedMediaType,
                "Unsupported media type",
                "Only image/jpeg, image/png and image/webp are supported.");
        }

        if (request.SizeBytes <= 0 || request.SizeBytes > options.MaxFileSizeBytes)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.FileTooLarge,
                "Invalid file size",
                "File size exceeds configured limit.");
        }

        var draft = await store.CreateDraftAsync(accountId, request.FileName, request.ContentType, request.SizeBytes, request.WorkingTitle, cancellationToken);
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(options.UploadUrlTtlMinutes);
        var intent = await storage.CreateUploadIntentAsync(draft.ObjectKey, draft.ContentType, expiresAt, cancellationToken);

        return new CreateMemeIntakeResponse(
            IntakeId: draft.IntakeId,
            CandidateId: draft.CandidateId,
            CandidateVersionId: draft.CandidateVersionId,
            AssetId: draft.AssetId,
            ObjectKey: draft.ObjectKey,
            UploadUrl: intent.UploadUrl,
            UploadHeaders: intent.UploadHeaders,
            UploadUrlExpiresAtUtc: intent.ExpiresAtUtc,
            Status: "creating_draft");
    }

    public async Task<FinalizeMemeIntakeResponse> FinalizeAsync(Guid intakeId, CancellationToken cancellationToken)
    {
        var intake = await store.GetByIntakeIdAsync(intakeId, cancellationToken)
            ?? throw NotFound();

        if (string.Equals(intake.Status, "uploaded", StringComparison.OrdinalIgnoreCase)
            || string.Equals(intake.Status, "awaiting_review", StringComparison.OrdinalIgnoreCase))
        {
            return new FinalizeMemeIntakeResponse(intakeId, intake.Status, intake.ContentType, intake.SizeBytes, string.Empty);
        }

        var metadata = await storage.GetObjectMetadataAsync(intake.ObjectKey, cancellationToken);
        if (!metadata.Exists)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.UploadObjectMissing,
                "Uploaded object missing",
                "Uploaded file is missing in private storage.");
        }

        if (metadata.SizeBytes <= 0 || metadata.SizeBytes > options.MaxFileSizeBytes)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.FileTooLarge,
                "Invalid file size",
                "Uploaded file exceeds configured limit.");
        }

        var prefix = await storage.ReadObjectPrefixAsync(intake.ObjectKey, 64, cancellationToken);
        var detectedType = DetectImageContentType(prefix);
        if (detectedType is null || !_allowedContentTypes.Contains(detectedType))
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.UploadObjectInvalid,
                "Invalid file",
                "Uploaded file is not a supported image.");
        }

        var imageBytes = await storage.ReadObjectBytesAsync(intake.ObjectKey, options.MaxFileSizeBytes, cancellationToken);
        var hash = ToSha256(imageBytes);
        var dimensions = ImageSniffer.TryReadDimensions(prefix, detectedType);

        await store.MarkUploadedAsync(intakeId, hash, dimensions.WidthPx, dimensions.HeightPx, cancellationToken);

        return new FinalizeMemeIntakeResponse(intakeId, "awaiting_review", detectedType, metadata.SizeBytes, hash);
    }

    public async Task<EvaluateMemeIntakeResponse> EvaluateAsync(Guid intakeId, string correlationId, CancellationToken cancellationToken)
    {
        var intake = await store.GetByIntakeIdAsync(intakeId, cancellationToken)
            ?? throw NotFound();

        if (!string.Equals(intake.Status, "uploaded", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(intake.Status, "awaiting_review", StringComparison.OrdinalIgnoreCase))
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.IntakeNotUploaded,
                "Upload not finalized",
                "Finalize upload before running AI evaluation.");
        }

        var taxonomy = await store.GetActiveTaxonomyAsync(cancellationToken);
        var imageBytes = await storage.ReadObjectBytesAsync(intake.ObjectKey, options.MaxFileSizeBytes, cancellationToken);
        var aiExecutionId = Guid.NewGuid();

        try
        {
            var evaluation = await evaluationService.EvaluateAsync(intake, imageBytes, taxonomy, correlationId, cancellationToken);
            var mappedJson = JsonSerializer.Serialize(evaluation);
            var structuredJson = MemeEvaluationService.ToStructuredOutputJson(evaluation);

            await store.SaveEvaluationAsync(intakeId, aiExecutionId, structuredJson, mappedJson, cancellationToken);
            await store.SaveClassificationAsync(intakeId, aiExecutionId, evaluation, cancellationToken);

            var refreshed = await GetAsync(intakeId, cancellationToken);
            return new EvaluateMemeIntakeResponse(intakeId, "ready_for_review", refreshed.Evaluation!);
        }
        catch (StudioProblemDetailsException ex)
        {
            await store.SaveFailedEvaluationAsync(intakeId, aiExecutionId, ex.Code, ex.Detail, cancellationToken);
            throw;
        }
    }

    public async Task<StartEditorialReviewResponse> StartEditorialReviewAsync(Guid intakeId, Guid reviewerAccountId, CancellationToken cancellationToken)
    {
        var snapshot = await store.StartEditorialReviewAsync(intakeId, reviewerAccountId, cancellationToken);
        return new StartEditorialReviewResponse(
            IntakeId: snapshot.IntakeId,
            Status: "ready_for_review",
            EditorialStatus: snapshot.EditorialStatus,
            ClassificationStatus: snapshot.ClassificationStatus,
            CandidateClassificationId: snapshot.CandidateClassificationId);
    }

    public async Task<MemeEditorialDecisionResponse> ApproveAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        MemeEditorialDecisionRequest request,
        CancellationToken cancellationToken)
    {
        var snapshot = await store.ApproveAsync(
            intakeId,
            reviewerAccountId,
            request.Summary,
            request.EditorialNote,
            cancellationToken);

        return ToDecisionResponse(snapshot);
    }

    public async Task<MemeEditorialDecisionResponse> RejectAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        MemeEditorialDecisionRequest request,
        CancellationToken cancellationToken)
    {
        var snapshot = await store.RejectAsync(
            intakeId,
            reviewerAccountId,
            request.Summary,
            request.EditorialNote,
            cancellationToken);

        return ToDecisionResponse(snapshot);
    }

    public async Task<GetMemeIntakeResponse> GetAsync(Guid intakeId, CancellationToken cancellationToken)
    {
        var intake = await store.GetByIntakeIdAsync(intakeId, cancellationToken)
            ?? throw NotFound();

        MemeEvaluationResultDto? evaluation = null;
        if (intake.CandidateClassification is not null)
        {
            evaluation = await BuildEvaluationResultAsync(intake.CandidateClassification, cancellationToken);
        }

        return new GetMemeIntakeResponse(
            IntakeId: intake.IntakeId,
            Status: intake.Status,
            Asset: new MemeAssetDto(intake.FileName, intake.ContentType, intake.SizeBytes, intake.ObjectKey),
            Evaluation: evaluation,
            WorkingTitle: intake.WorkingTitle);
    }

    private async Task<MemeEvaluationResultDto> BuildEvaluationResultAsync(
        Toci.Haia.Database.Persistence.Entities.CandidateClassification classification,
        CancellationToken cancellationToken)
    {
        var taxonomy = await store.GetActiveTaxonomyAsync(cancellationToken);

        var values = classification.CandidateClassificationValues
            .Select(item =>
            {
                var axis = taxonomy.AxisByKey.Values.Single(a => a.ClassificationModelAxisId == item.ClassificationModelAxisId);
                var modelValue = taxonomy.ValueByAxisAndKey.Values.Single(v => v.ClassificationModelValueId == item.ClassificationModelValueId);
                return new MemeClassificationDto(
                    axis.ClassificationAxis.AxisKey,
                    axis.ClassificationAxis.DisplayName,
                    modelValue.ClassificationValue.ValueKey,
                    modelValue.ClassificationValue.DisplayName,
                    item.RelevanceScore,
                    item.Confidence ?? 0,
                    item.IsPrimary,
                    item.RankNo);
            })
            .OrderBy(x => x.AxisKey)
            .ThenBy(x => x.RankNo ?? int.MaxValue)
            .ToArray();

        var measures = classification.CandidateClassificationMeasures
            .Select(item =>
            {
                var axis = taxonomy.AxisByKey.Values.Single(a => a.ClassificationModelAxisId == item.ClassificationModelAxisId);
                return new MemeMeasureDto(
                    axis.ClassificationAxis.AxisKey,
                    axis.ClassificationAxis.DisplayName,
                    item.NormalizedValue,
                    item.Confidence ?? 0);
            })
            .OrderBy(x => x.AxisKey)
            .ToArray();

        var safety = classification.CandidateClassificationSensitivities
            .Select(item =>
            {
                var category = taxonomy.SensitivityByKey.Values.Single(x => x.SensitivityCategoryId == item.SensitivityCategoryId);
                return new MemeSafetyDto(
                    category.CategoryKey,
                    category.DisplayName,
                    item.SeverityLevel,
                    item.Confidence ?? 0,
                    item.ModerationRelevance);
            })
            .OrderByDescending(x => x.SeverityLevel)
            .ThenBy(x => x.CategoryKey)
            .ToArray();

        var candidateVersion = await store.GetCandidateVersionByIdAsync(classification.OnboardingCandidateVersionId, cancellationToken);
        var reactions = candidateVersion?.ReactionPacks
            .SelectMany(x => x.ReactionPackVersion is null ? [] : x.ReactionPackVersion.ReactionPackItems)
            .OrderBy(x => x.PoolOrderNo)
            .Select(item => new MemeReactionDto(
                item.ReactionVersion.ReactionLabel,
                item.PoolOrderNo,
                item.InitialSlotNo.HasValue,
                0,
                0,
                item.ReactionVersion.SecondPunchlineText ?? string.Empty,
                item.ReactionVersion.ReactionMechanisms.Select(m => m.MechanismKey).ToArray()))
            .ToArray() ?? [];

        var drynessLevel = classification.PredictedDrynessScaleLevel?.LevelNo is short levelNo ? (int)levelNo : 1;
        var drynessLabel = classification.PredictedDrynessScaleLevel?.Label ?? "n/d";

        return new MemeEvaluationResultDto
        {
            SuggestedTitle = candidateVersion?.Title ?? string.Empty,
            VisualDescription = candidateVersion?.BodyText ?? string.Empty,
            DetectedText = candidateVersion?.CaptionText,
            LanguageCode = candidateVersion?.LanguageCode ?? "pl-PL",
            ContentFormat = classification.FormatKey ?? "image",
            SuggestedRoleInFlow = candidateVersion?.RoleInFlow ?? "exploration",
            SuggestedRoleConfidence = 0,
            EditorialSummary = classification.EditorialNote,
            OverallConfidence = classification.OverallConfidence ?? 0,
            Classifications = values,
            Measures = measures,
            Safety = safety,
            ModerationRecommendation = candidateVersion?.ModerationStatus ?? "pending",
            ModerationSummary = null,
            PredictedDrynessLevel = drynessLevel,
            PredictedDrynessLabel = drynessLabel,
            PredictedDrynessConfidence = classification.PredictedDrynessConfidence ?? 0,
            Reactions = reactions,
        };
    }

    private StudioProblemDetailsException NotFound() => new(
        System.Net.HttpStatusCode.NotFound,
        ErrorCodes.IntakeNotFound,
        "Meme intake not found",
        "Meme intake was not found.");

    private static StudioProblemDetailsException Validation(string message) => new(
        System.Net.HttpStatusCode.BadRequest,
        ErrorCodes.ValidationFailed,
        "Validation failed",
        message);

    private static string? DetectImageContentType(byte[] bytes)
    {
        if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
        {
            return "image/jpeg";
        }

        if (bytes.Length >= 8
            && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47
            && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A)
        {
            return "image/png";
        }

        if (bytes.Length >= 12
            && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46
            && bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
        {
            return "image/webp";
        }

        return null;
    }

    private static string ToSha256(byte[] bytes)
    {
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static MemeEditorialDecisionResponse ToDecisionResponse(EditorialReviewSnapshot snapshot)
    {
        if (!snapshot.ReviewedByAccountId.HasValue || !snapshot.ReviewedAtUtc.HasValue)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.InternalServerError,
                ErrorCodes.UnexpectedError,
                "Editorial decision not persisted",
                "Editorial decision missing reviewer metadata.");
        }

        return new MemeEditorialDecisionResponse(
            IntakeId: snapshot.IntakeId,
            Status: snapshot.CandidateStatus,
            EditorialStatus: snapshot.EditorialStatus,
            ClassificationStatus: snapshot.ClassificationStatus,
            ReviewStatus: snapshot.ReviewStatus,
            CandidateClassificationId: snapshot.CandidateClassificationId,
            ReviewedAtUtc: snapshot.ReviewedAtUtc.Value.ToUniversalTime(),
            ReviewedByAccountId: snapshot.ReviewedByAccountId.Value);
    }
}

internal static class ImageSniffer
{
    public static (int? WidthPx, int? HeightPx) TryReadDimensions(byte[] prefix, string mimeType)
    {
        if (mimeType == "image/png" && prefix.Length >= 24)
        {
            var width = ReadInt32BigEndian(prefix, 16);
            var height = ReadInt32BigEndian(prefix, 20);
            return (width, height);
        }

        return (null, null);
    }

    private static int ReadInt32BigEndian(byte[] bytes, int offset)
        => (bytes[offset] << 24) | (bytes[offset + 1] << 16) | (bytes[offset + 2] << 8) | bytes[offset + 3];
}
