namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Toci.Haia.Database.Persistence.Context;
using Toci.Haia.Database.Persistence.Entities;
using Toci.Haia.Studio.Api.Common.Errors;

public sealed class EfMemeIntakeStore(HaiaDbContext dbContext) : IMemeIntakeStore
{
    public async Task<MemeIntakeDraft> CreateDraftAsync(
        Guid accountId,
        string fileName,
        string contentType,
        long sizeBytes,
        string? workingTitle,
        CancellationToken cancellationToken)
    {
        var intakeId = Guid.NewGuid();
        var candidateId = intakeId;
        var candidateVersionId = Guid.NewGuid();
        var assetId = Guid.NewGuid();
        var candidateKey = $"studio-meme-intake-{intakeId:N}";
        var objectKey = $"studio/onboarding-candidates/{candidateId:D}/{assetId:D}/original";

        await using var tx = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var candidate = new OnboardingCandidate
        {
            OnboardingCandidateId = candidateId,
            CandidateKey = candidateKey,
            ContentFormat = "image",
            CandidateStatus = "draft",
            CreatedByAccountId = accountId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var candidateVersion = new OnboardingCandidateVersion
        {
            OnboardingCandidateVersionId = candidateVersionId,
            OnboardingCandidateId = candidateId,
            VersionNo = 1,
            LanguageCode = "pl-PL",
            Title = workingTitle,
            RoleInFlow = "exploration",
            EditorialStatus = "draft",
            ModerationStatus = "pending",
            SafetyClassification = "unknown",
            RightsStatus = "pending_review",
            IsSignificantChange = true,
            CreatedByAccountId = accountId,
            CreatedAt = DateTime.UtcNow,
        };

        var mediaAsset = new MediaAsset
        {
            MediaAssetId = assetId,
            StorageKey = objectKey,
            MimeType = contentType,
            ByteSize = sizeBytes,
            Sha256Hash = CreatePlaceholderHash(),
            ProcessingStatus = "ready",
            RightsStatus = "unknown",
            ModerationStatus = "pending",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var candidateVersionMedia = new CandidateVersionMedium
        {
            CandidateVersionMediaId = Guid.NewGuid(),
            OnboardingCandidateVersionId = candidateVersionId,
            MediaAssetId = assetId,
            MediaRole = "original",
            DisplayOrder = 1,
            CaptionOverride = fileName,
            CreatedAt = DateTime.UtcNow,
        };

        dbContext.OnboardingCandidates.Add(candidate);
        dbContext.OnboardingCandidateVersions.Add(candidateVersion);
        dbContext.MediaAssets.Add(mediaAsset);
        dbContext.CandidateVersionMedia.Add(candidateVersionMedia);

        await dbContext.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return new MemeIntakeDraft(
            IntakeId: intakeId,
            CandidateId: candidateId,
            CandidateVersionId: candidateVersionId,
            AssetId: assetId,
            ObjectKey: objectKey,
            FileName: fileName,
            ContentType: contentType,
            SizeBytes: sizeBytes,
            WorkingTitle: workingTitle,
            Status: "creating_draft");
    }

    public async Task<MemeIntakeAggregate?> GetByIntakeIdAsync(Guid intakeId, CancellationToken cancellationToken)
    {
        var candidate = await dbContext.OnboardingCandidates
            .AsNoTracking()
            .Include(c => c.OnboardingCandidateVersions)
                .ThenInclude(v => v.CandidateVersionMedia)
                    .ThenInclude(vm => vm.MediaAsset)
            .Include(c => c.OnboardingCandidateVersions)
                .ThenInclude(v => v.CandidateClassification)
                    .ThenInclude(cc => cc.CandidateClassificationValues)
            .Include(c => c.OnboardingCandidateVersions)
                .ThenInclude(v => v.CandidateClassification)
                    .ThenInclude(cc => cc.CandidateClassificationMeasures)
            .Include(c => c.OnboardingCandidateVersions)
                .ThenInclude(v => v.CandidateClassification)
                    .ThenInclude(cc => cc.CandidateClassificationSensitivities)
            .SingleOrDefaultAsync(c => c.OnboardingCandidateId == intakeId, cancellationToken);

        if (candidate is null)
        {
            return null;
        }

        var version = candidate.OnboardingCandidateVersions.OrderByDescending(v => v.VersionNo).First();
        var media = version.CandidateVersionMedia.OrderBy(m => m.DisplayOrder).First();

        var latestOutput = await dbContext.AiGeneratedOutputs
            .AsNoTracking()
            .Where(x => x.AiOperationExecution.AiGenerationTarget!.OnboardingCandidateVersionId == version.OnboardingCandidateVersionId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new { x.MappedOutput, x.StructuredOutput })
            .FirstOrDefaultAsync(cancellationToken);

        return new MemeIntakeAggregate(
            IntakeId: intakeId,
            Status: candidate.CandidateStatus,
            FileName: media.CaptionOverride ?? "uploaded-image",
            ContentType: media.MediaAsset.MimeType,
            SizeBytes: media.MediaAsset.ByteSize,
            ObjectKey: media.MediaAsset.StorageKey,
            WorkingTitle: version.Title,
            CandidateClassification: version.CandidateClassification,
            LatestMappedOutputJson: latestOutput?.MappedOutput,
            LatestStructuredOutputJson: latestOutput?.StructuredOutput);
    }

    public async Task MarkUploadedAsync(Guid intakeId, string sha256Hash, int? widthPx, int? heightPx, CancellationToken cancellationToken)
    {
        var candidate = await dbContext.OnboardingCandidates
            .Include(c => c.OnboardingCandidateVersions)
                .ThenInclude(v => v.CandidateVersionMedia)
                    .ThenInclude(vm => vm.MediaAsset)
            .SingleOrDefaultAsync(c => c.OnboardingCandidateId == intakeId, cancellationToken);

        if (candidate is null)
        {
            throw NotFound();
        }

        var version = candidate.OnboardingCandidateVersions.OrderByDescending(v => v.VersionNo).First();
        var media = version.CandidateVersionMedia.OrderBy(m => m.DisplayOrder).First();

        media.MediaAsset.Sha256Hash = sha256Hash;
        media.MediaAsset.WidthPx = widthPx;
        media.MediaAsset.HeightPx = heightPx;
        media.MediaAsset.UpdatedAt = DateTime.UtcNow;

        candidate.CandidateStatus = "uploaded";
        candidate.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveEvaluationAsync(
        Guid intakeId,
        Guid aiOperationExecutionId,
        string structuredOutputJson,
        string mappedOutputJson,
        CancellationToken cancellationToken)
    {
        var version = await GetCandidateVersionAsync(intakeId, cancellationToken)
            ?? throw NotFound();

        var target = new AiGenerationTarget
        {
            AiGenerationTargetId = Guid.NewGuid(),
            TargetId = version.OnboardingCandidateVersionId,
            TargetType = "onboarding_candidate_version",
            OnboardingCandidateVersionId = version.OnboardingCandidateVersionId,
            CreatedAt = DateTime.UtcNow,
        };

        var execution = new AiOperationExecution
        {
            AiOperationExecutionId = aiOperationExecutionId,
            AiGenerationTargetId = target.AiGenerationTargetId,
            OperationName = "EvaluateStudioMemeImage",
            Provider = "openai",
            Model = "unknown",
            StartedAt = DateTime.UtcNow,
            FinishedAt = DateTime.UtcNow,
            DurationMs = 1,
            ExecutionStatus = "succeeded",
            RetryCount = 0,
            RepairCount = 0,
            SourceMaterialRef = intakeId.ToString("D"),
            CreatedAt = DateTime.UtcNow,
        };

        var output = new AiGeneratedOutput
        {
            AiGeneratedOutputId = Guid.NewGuid(),
            AiOperationExecutionId = aiOperationExecutionId,
            OutputType = "studio_meme_evaluation_v0_1",
            StructuredOutput = structuredOutputJson,
            MappedOutput = mappedOutputJson,
            CreatedAt = DateTime.UtcNow,
        };

        dbContext.AiGenerationTargets.Add(target);
        dbContext.AiOperationExecutions.Add(execution);
        dbContext.AiGeneratedOutputs.Add(output);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveFailedEvaluationAsync(
        Guid intakeId,
        Guid aiOperationExecutionId,
        string errorCode,
        string structuredOutputJson,
        CancellationToken cancellationToken)
    {
        var version = await GetCandidateVersionAsync(intakeId, cancellationToken)
            ?? throw NotFound();

        var target = new AiGenerationTarget
        {
            AiGenerationTargetId = Guid.NewGuid(),
            TargetId = version.OnboardingCandidateVersionId,
            TargetType = "onboarding_candidate_version",
            OnboardingCandidateVersionId = version.OnboardingCandidateVersionId,
            CreatedAt = DateTime.UtcNow,
        };

        dbContext.AiGenerationTargets.Add(target);

        dbContext.AiOperationExecutions.Add(new AiOperationExecution
        {
            AiOperationExecutionId = aiOperationExecutionId,
            AiGenerationTargetId = target.AiGenerationTargetId,
            OperationName = "EvaluateStudioMemeImage",
            Provider = "openai",
            Model = "unknown",
            StartedAt = DateTime.UtcNow,
            FinishedAt = DateTime.UtcNow,
            DurationMs = 1,
            ExecutionStatus = "failed",
            RetryCount = 0,
            RepairCount = 0,
            ErrorCode = errorCode,
            SourceMaterialRef = intakeId.ToString("D"),
            CreatedAt = DateTime.UtcNow,
        });

        dbContext.AiGeneratedOutputs.Add(new AiGeneratedOutput
        {
            AiGeneratedOutputId = Guid.NewGuid(),
            AiOperationExecutionId = aiOperationExecutionId,
            OutputType = "studio_meme_evaluation_error_v0_1",
            StructuredOutput = structuredOutputJson,
            CreatedAt = DateTime.UtcNow,
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ClassificationTaxonomySnapshot> GetActiveTaxonomyAsync(CancellationToken cancellationToken)
    {
        var model = await dbContext.ClassificationModelVersions
            .Include(m => m.ClassificationModelAxes)
                .ThenInclude(a => a.ClassificationAxis)
            .Include(m => m.ClassificationModelAxes)
                .ThenInclude(a => a.ClassificationModelValues)
                    .ThenInclude(v => v.ClassificationValue)
            .SingleAsync(m => m.ModelKey == "haia_humor_classification" && m.VersionNo == 1 && m.Status == "active", cancellationToken);

        var sensitivity = await dbContext.SensitivityCategories.Where(s => s.IsActive)
            .ToDictionaryAsync(s => s.CategoryKey, StringComparer.Ordinal, cancellationToken);

        var dryness = await dbContext.DrynessScaleLevels
            .Include(x => x.DrynessScaleVersion)
                .ThenInclude(v => v.DrynessScale)
            .Where(x => x.DrynessScaleVersion.Status == "active")
            .ToDictionaryAsync(x => (int)x.LevelNo, cancellationToken);

        var mechanisms = await dbContext.ReactionMechanisms.Where(x => x.IsActive)
            .ToDictionaryAsync(x => x.MechanismKey, StringComparer.Ordinal, cancellationToken);

        var axisByKey = model.ClassificationModelAxes
            .Where(a => a.MemberStatus == "active")
            .ToDictionary(a => a.ClassificationAxis.AxisKey, StringComparer.Ordinal);

        var valueByAxisAndKey = model.ClassificationModelAxes
            .SelectMany(axis => axis.ClassificationModelValues.Select(value => new
            {
                axis.ClassificationAxis.AxisKey,
                value.ClassificationValue.ValueKey,
                Value = value,
            }))
            .ToDictionary(x => (x.AxisKey, x.ValueKey), x => x.Value);

        return new ClassificationTaxonomySnapshot(model, axisByKey, valueByAxisAndKey, sensitivity, dryness, mechanisms);
    }

    public async Task<OnboardingCandidateVersion?> GetCandidateVersionAsync(Guid intakeId, CancellationToken cancellationToken)
    {
        return await dbContext.OnboardingCandidateVersions
            .Include(v => v.OnboardingCandidate)
            .Include(v => v.CandidateVersionMedia)
                .ThenInclude(m => m.MediaAsset)
            .Include(v => v.ReactionPacks)
                .ThenInclude(p => p.ReactionPackVersion)
                    .ThenInclude(v => v.ReactionPackItems)
                        .ThenInclude(i => i.ReactionVersion)
                            .ThenInclude(rv => rv.ReactionMechanisms)
            .Where(v => v.OnboardingCandidateId == intakeId)
            .OrderByDescending(v => v.VersionNo)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OnboardingCandidateVersion?> GetCandidateVersionByIdAsync(Guid candidateVersionId, CancellationToken cancellationToken)
    {
        return await dbContext.OnboardingCandidateVersions
            .Include(v => v.OnboardingCandidate)
            .Include(v => v.ReactionPacks)
                .ThenInclude(p => p.ReactionPackVersion)
                    .ThenInclude(v => v.ReactionPackItems)
                        .ThenInclude(i => i.ReactionVersion)
                            .ThenInclude(rv => rv.ReactionMechanisms)
            .SingleOrDefaultAsync(v => v.OnboardingCandidateVersionId == candidateVersionId, cancellationToken);
    }

    public async Task SaveClassificationAsync(Guid intakeId, Guid aiOperationExecutionId, PersistedMemeEvaluation evaluation, CancellationToken cancellationToken)
    {
        var taxonomy = await GetActiveTaxonomyAsync(cancellationToken);
        var version = await GetCandidateVersionAsync(intakeId, cancellationToken)
            ?? throw NotFound();

        await using var tx = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var existing = await dbContext.CandidateClassifications
            .Include(x => x.CandidateClassificationValues)
            .Include(x => x.CandidateClassificationMeasures)
            .Include(x => x.CandidateClassificationSensitivities)
            .SingleOrDefaultAsync(x => x.OnboardingCandidateVersionId == version.OnboardingCandidateVersionId, cancellationToken);

        if (existing is null)
        {
            existing = new CandidateClassification
            {
                CandidateClassificationId = Guid.NewGuid(),
                OnboardingCandidateVersionId = version.OnboardingCandidateVersionId,
                ClassificationModelVersionId = taxonomy.ActiveModel.ClassificationModelVersionId,
                RevisionNo = 1,
                SourceType = "ai",
                ClassificationStatus = "awaiting_review",
                CreatedAt = DateTime.UtcNow,
            };

            dbContext.CandidateClassifications.Add(existing);
        }
        else
        {
            dbContext.CandidateClassificationValues.RemoveRange(existing.CandidateClassificationValues);
            dbContext.CandidateClassificationMeasures.RemoveRange(existing.CandidateClassificationMeasures);
            dbContext.CandidateClassificationSensitivities.RemoveRange(existing.CandidateClassificationSensitivities);
        }

        existing.FormatKey = evaluation.ContentFormat;
        existing.OverallConfidence = evaluation.OverallConfidence;
        existing.AiOperationExecutionId = aiOperationExecutionId;
        existing.EditorialNote = evaluation.EditorialSummary;
        existing.PredictedDrynessConfidence = evaluation.PredictedDrynessConfidence;
        existing.PredictedDrynessScaleLevelId = taxonomy.DrynessLevelsByNo[evaluation.PredictedDrynessLevel].DrynessScaleLevelId;
        existing.SafetyFlags = null;

        foreach (var item in evaluation.Classifications)
        {
            if (!taxonomy.ValueByAxisAndKey.TryGetValue((item.AxisKey, item.ValueKey), out var modelValue))
            {
                throw new StudioProblemDetailsException(
                    System.Net.HttpStatusCode.BadRequest,
                    ErrorCodes.TaxonomyMismatch,
                    "Taxonomy mismatch",
                    $"Unknown value key '{item.ValueKey}' for axis '{item.AxisKey}'.");
            }

            dbContext.CandidateClassificationValues.Add(new CandidateClassificationValue
            {
                CandidateClassificationValueId = Guid.NewGuid(),
                CandidateClassificationId = existing.CandidateClassificationId,
                ClassificationModelVersionId = taxonomy.ActiveModel.ClassificationModelVersionId,
                ClassificationModelAxisId = modelValue.ClassificationModelAxisId,
                ClassificationModelValueId = modelValue.ClassificationModelValueId,
                RelevanceScore = item.RelevanceScore,
                Confidence = item.Confidence,
                IsPrimary = item.IsPrimary,
                RankNo = item.RankNo,
                AssignmentSource = "ai",
                CreatedAt = DateTime.UtcNow,
            });
        }

        foreach (var item in evaluation.Measures)
        {
            if (!taxonomy.AxisByKey.TryGetValue(item.AxisKey, out var modelAxis))
            {
                throw new StudioProblemDetailsException(
                    System.Net.HttpStatusCode.BadRequest,
                    ErrorCodes.TaxonomyMismatch,
                    "Taxonomy mismatch",
                    $"Unknown measure axis '{item.AxisKey}'.");
            }

            dbContext.CandidateClassificationMeasures.Add(new CandidateClassificationMeasure
            {
                CandidateClassificationMeasureId = Guid.NewGuid(),
                CandidateClassificationId = existing.CandidateClassificationId,
                ClassificationModelVersionId = taxonomy.ActiveModel.ClassificationModelVersionId,
                ClassificationModelAxisId = modelAxis.ClassificationModelAxisId,
                NormalizedValue = item.NormalizedValue,
                Confidence = item.Confidence,
                MeasurementSource = "ai",
                CreatedAt = DateTime.UtcNow,
            });
        }

        foreach (var item in evaluation.Safety)
        {
            if (!taxonomy.SensitivityByKey.TryGetValue(item.CategoryKey, out var sensitivity))
            {
                throw new StudioProblemDetailsException(
                    System.Net.HttpStatusCode.BadRequest,
                    ErrorCodes.TaxonomyMismatch,
                    "Taxonomy mismatch",
                    $"Unknown safety category '{item.CategoryKey}'.");
            }

            dbContext.CandidateClassificationSensitivities.Add(new CandidateClassificationSensitivity
            {
                CandidateClassificationSensitivityId = Guid.NewGuid(),
                CandidateClassificationId = existing.CandidateClassificationId,
                SensitivityCategoryId = sensitivity.SensitivityCategoryId,
                SeverityLevel = item.SeverityLevel,
                Confidence = item.Confidence,
                ModerationRelevance = item.ModerationRelevance,
                AssignmentSource = "ai",
                CreatedAt = DateTime.UtcNow,
            });
        }

        await PersistReactionsAsync(version, evaluation, taxonomy, cancellationToken);

        version.EditorialStatus = "ai_analysis_ready";
        version.OnboardingCandidate.CandidateStatus = "ai_analysis_ready";
        version.OnboardingCandidate.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
    }

    public async Task<EditorialReviewSnapshot> StartEditorialReviewAsync(Guid intakeId, Guid reviewerAccountId, CancellationToken cancellationToken)
    {
        var version = await dbContext.OnboardingCandidateVersions
            .Include(v => v.OnboardingCandidate)
            .SingleOrDefaultAsync(v => v.OnboardingCandidateId == intakeId, cancellationToken)
            ?? throw NotFound();

        var classification = await dbContext.CandidateClassifications
            .SingleOrDefaultAsync(x => x.OnboardingCandidateVersionId == version.OnboardingCandidateVersionId, cancellationToken)
            ?? throw MissingClassification();

        var now = DateTime.UtcNow;

        if (version.OnboardingCandidate.CandidateStatus != "editorial_review")
        {
            version.OnboardingCandidate.CandidateStatus = "editorial_review";
            version.OnboardingCandidate.UpdatedAt = now;
        }

        if (version.EditorialStatus != "editorial_review")
        {
            version.EditorialStatus = "editorial_review";
        }

        var review = await dbContext.EditorialReviews
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(
                x => x.TargetType == "candidate_version" && x.TargetId == version.OnboardingCandidateVersionId,
                cancellationToken);

        if (review is null)
        {
            review = new EditorialReview
            {
                EditorialReviewId = Guid.NewGuid(),
                TargetType = "candidate_version",
                TargetId = version.OnboardingCandidateVersionId,
                ReviewStatus = "in_review",
                ReviewerAccountId = reviewerAccountId,
                Summary = classification.EditorialNote,
                CreatedAt = now,
                UpdatedAt = now,
            };

            dbContext.EditorialReviews.Add(review);
        }
        else
        {
            review.ReviewStatus = "in_review";
            review.ReviewerAccountId = reviewerAccountId;
            review.UpdatedAt = now;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new EditorialReviewSnapshot(
            IntakeId: intakeId,
            CandidateClassificationId: classification.CandidateClassificationId,
            CandidateStatus: version.OnboardingCandidate.CandidateStatus,
            EditorialStatus: version.EditorialStatus,
            ClassificationStatus: classification.ClassificationStatus,
            ReviewStatus: review.ReviewStatus,
            ReviewedByAccountId: classification.ReviewedByAccountId,
            ReviewedAtUtc: classification.ReviewedAt);
    }

    public Task<EditorialReviewSnapshot> ApproveAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        string? summary,
        string? editorialNote,
        CancellationToken cancellationToken)
        => DecideAsync(intakeId, reviewerAccountId, "approved", "approved", summary, editorialNote, cancellationToken);

    public Task<EditorialReviewSnapshot> RejectAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        string? summary,
        string? editorialNote,
        CancellationToken cancellationToken)
        => DecideAsync(intakeId, reviewerAccountId, "rejected", "rejected", summary, editorialNote, cancellationToken);

    private async Task<EditorialReviewSnapshot> DecideAsync(
        Guid intakeId,
        Guid reviewerAccountId,
        string reviewStatus,
        string classificationStatus,
        string? summary,
        string? editorialNote,
        CancellationToken cancellationToken)
    {
        var version = await dbContext.OnboardingCandidateVersions
            .Include(v => v.OnboardingCandidate)
            .SingleOrDefaultAsync(v => v.OnboardingCandidateId == intakeId, cancellationToken)
            ?? throw NotFound();

        var classification = await dbContext.CandidateClassifications
            .SingleOrDefaultAsync(x => x.OnboardingCandidateVersionId == version.OnboardingCandidateVersionId, cancellationToken)
            ?? throw MissingClassification();

        var now = DateTime.UtcNow;

        version.EditorialStatus = reviewStatus;
        version.OnboardingCandidate.CandidateStatus = reviewStatus;
        version.OnboardingCandidate.UpdatedAt = now;

        classification.ClassificationStatus = classificationStatus;
        classification.ReviewedByAccountId = reviewerAccountId;
        classification.ReviewedAt = now;
        classification.EditorialNote = string.IsNullOrWhiteSpace(editorialNote) ? classification.EditorialNote : editorialNote.Trim();

        var review = await dbContext.EditorialReviews
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(
                x => x.TargetType == "candidate_version" && x.TargetId == version.OnboardingCandidateVersionId,
                cancellationToken);

        if (review is null)
        {
            review = new EditorialReview
            {
                EditorialReviewId = Guid.NewGuid(),
                TargetType = "candidate_version",
                TargetId = version.OnboardingCandidateVersionId,
                ReviewStatus = reviewStatus,
                ReviewerAccountId = reviewerAccountId,
                Summary = summary,
                CreatedAt = now,
                UpdatedAt = now,
            };

            dbContext.EditorialReviews.Add(review);
        }
        else
        {
            review.ReviewStatus = reviewStatus;
            review.ReviewerAccountId = reviewerAccountId;
            review.Summary = string.IsNullOrWhiteSpace(summary) ? review.Summary : summary.Trim();
            review.UpdatedAt = now;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new EditorialReviewSnapshot(
            IntakeId: intakeId,
            CandidateClassificationId: classification.CandidateClassificationId,
            CandidateStatus: version.OnboardingCandidate.CandidateStatus,
            EditorialStatus: version.EditorialStatus,
            ClassificationStatus: classification.ClassificationStatus,
            ReviewStatus: review.ReviewStatus,
            ReviewedByAccountId: classification.ReviewedByAccountId,
            ReviewedAtUtc: classification.ReviewedAt);
    }

    private async Task PersistReactionsAsync(
        OnboardingCandidateVersion version,
        PersistedMemeEvaluation evaluation,
        ClassificationTaxonomySnapshot taxonomy,
        CancellationToken cancellationToken)
    {
        var existingPack = await dbContext.ReactionPacks
            .Include(p => p.ReactionPackVersion)
                .ThenInclude(v => v.ReactionPackItems)
            .SingleOrDefaultAsync(p => p.OnboardingCandidateVersionId == version.OnboardingCandidateVersionId, cancellationToken);

        ReactionPack pack;
        ReactionPackVersion packVersion;

        if (existingPack is null)
        {
            pack = new ReactionPack
            {
                ReactionPackId = Guid.NewGuid(),
                OnboardingCandidateVersionId = version.OnboardingCandidateVersionId,
                PackKey = $"studio-meme-{version.OnboardingCandidateVersionId:N}",
                LanguageCode = "pl-PL",
                SourceType = "ai",
                CreatedAt = DateTime.UtcNow,
            };

            packVersion = new ReactionPackVersion
            {
                ReactionPackVersionId = Guid.NewGuid(),
                ReactionPackId = pack.ReactionPackId,
                VersionNo = 1,
                Status = "generated",
                TargetPoolSize = 12,
                InitialVisibleCount = 6,
                MaxActiveSelections = 12,
                DrynessInteractionMode = "exclusive",
                EditorialStatus = "awaiting_editorial_review",
                ModerationStatus = "pending",
                CreatedAt = DateTime.UtcNow,
            };

            dbContext.ReactionPacks.Add(pack);
            dbContext.ReactionPackVersions.Add(packVersion);
        }
        else
        {
            pack = existingPack;
            packVersion = existingPack.ReactionPackVersion!;
            dbContext.ReactionPackItems.RemoveRange(packVersion.ReactionPackItems);
        }

        foreach (var reaction in evaluation.Reactions.OrderBy(x => x.DisplayOrder))
        {
            var entity = new Reaction
            {
                ReactionId = Guid.NewGuid(),
                ReactionKey = $"studio-meme-{version.OnboardingCandidateVersionId:N}-{reaction.DisplayOrder}",
                CreatedAt = DateTime.UtcNow,
            };

            var reactionVersion = new ReactionVersion
            {
                ReactionVersionId = Guid.NewGuid(),
                ReactionId = entity.ReactionId,
                VersionNo = 1,
                ReactionLabel = reaction.Text,
                StyleKey = "custom",
                Intensity = 3,
                IsReactionRescue = false,
                SecondPunchlineText = reaction.SecondPunchline,
                Status = "generated",
                CreatedAt = DateTime.UtcNow,
            };

            foreach (var mechanismKey in reaction.MechanismKeys)
            {
                if (!taxonomy.ReactionMechanismsByKey.TryGetValue(mechanismKey, out var mechanism))
                {
                    throw new StudioProblemDetailsException(
                        System.Net.HttpStatusCode.BadRequest,
                        ErrorCodes.TaxonomyMismatch,
                        "Taxonomy mismatch",
                        $"Unknown reaction mechanism '{mechanismKey}'.");
                }

                reactionVersion.ReactionMechanisms.Add(mechanism);
            }

            dbContext.Reactions.Add(entity);
            dbContext.ReactionVersions.Add(reactionVersion);

            dbContext.ReactionPackItems.Add(new ReactionPackItem
            {
                ReactionPackItemId = Guid.NewGuid(),
                ReactionPackVersionId = packVersion.ReactionPackVersionId,
                ReactionVersionId = reactionVersion.ReactionVersionId,
                PoolOrderNo = reaction.DisplayOrder,
                InitialSlotNo = reaction.InitiallyVisible ? reaction.DisplayOrder : null,
                ReplacementPriority = reaction.DisplayOrder,
                IsAvailableInMore = true,
                CreatedAt = DateTime.UtcNow,
            });
        }
    }

    private static string CreatePlaceholderHash()
    {
        Span<byte> bytes = stackalloc byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static StudioProblemDetailsException NotFound() =>
        new(
            System.Net.HttpStatusCode.NotFound,
            ErrorCodes.IntakeNotFound,
            "Meme intake not found",
            "Meme intake was not found.");

    private static StudioProblemDetailsException MissingClassification() =>
        new(
            System.Net.HttpStatusCode.BadRequest,
            ErrorCodes.IntakeNotReadyForReview,
            "Meme intake is not ready for editorial review",
            "Run AI evaluation before editorial decision.");
}
