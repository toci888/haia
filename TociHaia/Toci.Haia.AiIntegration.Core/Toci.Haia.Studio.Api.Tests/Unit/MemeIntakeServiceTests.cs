using Toci.Haia.Database.Persistence.Entities;
using Toci.Haia.Studio.Api.Common.Errors;
using Toci.Haia.Studio.Api.Features.MemeIntakes;

namespace Toci.Haia.Studio.Api.Tests.Unit;

public sealed class MemeIntakeServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldRejectUnsupportedContentType()
    {
        var store = new FakeStore();
        var storage = new FakeStorage();
        var evaluation = new FakeEvaluationService();
        var service = CreateService(store, storage, evaluation);

        var ex = await Assert.ThrowsAsync<StudioProblemDetailsException>(() =>
            service.CreateAsync(Guid.NewGuid(), new CreateMemeIntakeRequest("meme.gif", "image/gif", 100, null), CancellationToken.None));

        Assert.Equal(ErrorCodes.UnsupportedMediaType, ex.Code);
    }

    [Fact]
    public async Task CreateAsync_ShouldRejectTooLargeFile()
    {
        var store = new FakeStore();
        var storage = new FakeStorage();
        var evaluation = new FakeEvaluationService();
        var service = CreateService(store, storage, evaluation);

        var ex = await Assert.ThrowsAsync<StudioProblemDetailsException>(() =>
            service.CreateAsync(Guid.NewGuid(), new CreateMemeIntakeRequest("meme.png", "image/png", 20 * 1024 * 1024, null), CancellationToken.None));

        Assert.Equal(ErrorCodes.FileTooLarge, ex.Code);
    }

    [Fact]
    public async Task FinalizeAsync_ShouldRejectMissingObject()
    {
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(Guid.NewGuid(), "creating_draft", "meme.png", "image/png", 1024, "key", null, null, null, null),
        };

        var storage = new FakeStorage
        {
            Metadata = new MediaObjectMetadata(false, null, 0, null),
        };

        var service = CreateService(store, storage, new FakeEvaluationService());

        var ex = await Assert.ThrowsAsync<StudioProblemDetailsException>(() => service.FinalizeAsync(store.Intake.IntakeId, CancellationToken.None));
        Assert.Equal(ErrorCodes.UploadObjectMissing, ex.Code);
    }

    [Fact]
    public async Task FinalizeAsync_ShouldRejectDuplicateUpload()
    {
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(Guid.NewGuid(), "creating_draft", "meme.png", "image/png", 1024, "key", null, null, null, null),
            IsDuplicateUpload = true,
        };

        var service = CreateService(store, new FakeStorage(), new FakeEvaluationService());

        var response = await service.FinalizeAsync(store.Intake.IntakeId, CancellationToken.None);

        Assert.Equal("duplicate_upload", response.Status);
        Assert.True(response.IsDuplicate);
        Assert.Equal("Uploaded file is a duplicate of an existing media asset.", response.Message);
        Assert.False(store.MarkUploadedCalled);
    }

    [Fact]
    public async Task FinalizeAsync_ShouldBeIdempotentForUploadedStatus()
    {
        var intakeId = Guid.NewGuid();
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(intakeId, "uploaded", "meme.png", "image/png", 1024, "key", null, null, null, null),
        };

        var service = CreateService(store, new FakeStorage(), new FakeEvaluationService());

        var result = await service.FinalizeAsync(intakeId, CancellationToken.None);

        Assert.Equal("uploaded", result.Status);
        Assert.False(store.MarkUploadedCalled);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldRejectWhenNotUploaded()
    {
        var intakeId = Guid.NewGuid();
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(intakeId, "creating_draft", "meme.png", "image/png", 1024, "key", null, null, null, null),
        };

        var service = CreateService(store, new FakeStorage(), new FakeEvaluationService());

        var ex = await Assert.ThrowsAsync<StudioProblemDetailsException>(() => service.EvaluateAsync(intakeId, "corr-1", CancellationToken.None));

        Assert.Equal(ErrorCodes.IntakeNotUploaded, ex.Code);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldPersistDraftClassification()
    {
        var intakeId = Guid.NewGuid();
        var candidateVersionId = Guid.NewGuid();
        var taxonomy = BuildTaxonomy();

        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(intakeId, "uploaded", "meme.png", "image/png", 1024, "key", "Draft", null, null, null),
            Taxonomy = taxonomy,
            CandidateVersion = new OnboardingCandidateVersion
            {
                OnboardingCandidateVersionId = candidateVersionId,
                OnboardingCandidateId = intakeId,
                VersionNo = 1,
                LanguageCode = "pl-PL",
                RoleInFlow = "exploration",
                ModerationStatus = "pending",
                Title = "Draft",
            },
        };

        var evaluationResult = new PersistedMemeEvaluation(
            OverallConfidence: 0.83m,
            SuggestedTitle: "Tytuł",
            VisualDescription: "Opis",
            DetectedText: "Tekst",
            LanguageCode: "pl-PL",
            ContentFormat: "image",
            SuggestedRoleInFlow: "exploration",
            SuggestedRoleConfidence: 0.7m,
            EditorialSummary: "Summary",
            PredictedDrynessLevel: 3,
            PredictedDrynessConfidence: 0.8m,
            ModerationRecommendation: "review",
            ModerationSummary: "summary",
            Classifications:
            [
                new PersistedMemeClassificationValue("mechanism", "incongruity", 0.9m, 0.8m, true, 1),
            ],
            Measures:
            [
                new PersistedMemeMeasureValue("absurdity", 0.4m, 0.6m),
            ],
            Safety:
            [
                new PersistedMemeSafetyValue("general", 1, 0.8m, true),
            ],
            Reactions:
            [
                new PersistedMemeReactionValue("R1", 1, true, 0.9m, 0.8m, "SP1", ["incongruity"]),
                new PersistedMemeReactionValue("R2", 2, true, 0.9m, 0.8m, "SP2", ["incongruity"]),
                new PersistedMemeReactionValue("R3", 3, true, 0.9m, 0.8m, "SP3", ["incongruity"]),
                new PersistedMemeReactionValue("R4", 4, true, 0.9m, 0.8m, "SP4", ["incongruity"]),
                new PersistedMemeReactionValue("R5", 5, true, 0.9m, 0.8m, "SP5", ["incongruity"]),
                new PersistedMemeReactionValue("R6", 6, true, 0.9m, 0.8m, "SP6", ["incongruity"]),
                new PersistedMemeReactionValue("R7", 7, false, 0.9m, 0.8m, "SP7", ["incongruity"]),
                new PersistedMemeReactionValue("R8", 8, false, 0.9m, 0.8m, "SP8", ["incongruity"]),
                new PersistedMemeReactionValue("R9", 9, false, 0.9m, 0.8m, "SP9", ["incongruity"]),
                new PersistedMemeReactionValue("R10", 10, false, 0.9m, 0.8m, "SP10", ["incongruity"]),
                new PersistedMemeReactionValue("R11", 11, false, 0.9m, 0.8m, "SP11", ["incongruity"]),
                new PersistedMemeReactionValue("R12", 12, false, 0.9m, 0.8m, "SP12", ["incongruity"]),
            ]);

        var service = CreateService(store, new FakeStorage { ObjectBytes = [0x89, 0x50, 0x4E, 0x47] }, new FakeEvaluationService { Result = evaluationResult });

        var result = await service.EvaluateAsync(intakeId, "corr-1", CancellationToken.None);

        Assert.Equal("ready_for_review", result.Status);
        Assert.True(store.SaveEvaluationCalled);
        Assert.True(store.SaveClassificationCalled);
        Assert.NotNull(store.Intake.CandidateClassification);
        Assert.Equal("ai_analysis_ready", store.Intake.Status);
    }

    [Fact]
    public async Task StartEditorialReviewAsync_ShouldReturnReviewSnapshot()
    {
        var intakeId = Guid.NewGuid();
        var reviewerId = Guid.NewGuid();
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(intakeId, "ai_analysis_ready", "meme.png", "image/png", 1024, "key", "Draft", null, null, null),
            StartSnapshot = new EditorialReviewSnapshot(
                IntakeId: intakeId,
                CandidateClassificationId: Guid.NewGuid(),
                CandidateStatus: "editorial_review",
                EditorialStatus: "editorial_review",
                ClassificationStatus: "awaiting_review",
                ReviewStatus: "in_review",
                ReviewedByAccountId: null,
                ReviewedAtUtc: null),
        };

        var service = CreateService(store, new FakeStorage(), new FakeEvaluationService());

        var result = await service.StartEditorialReviewAsync(intakeId, reviewerId, CancellationToken.None);

        Assert.Equal(intakeId, result.IntakeId);
        Assert.Equal("ready_for_review", result.Status);
        Assert.Equal("editorial_review", result.EditorialStatus);
        Assert.Equal("awaiting_review", result.ClassificationStatus);
        Assert.NotNull(result.CandidateClassificationId);
    }

    [Fact]
    public async Task ApproveAsync_ShouldReturnReviewerMetadata()
    {
        var intakeId = Guid.NewGuid();
        var reviewerId = Guid.NewGuid();
        var reviewedAt = DateTimeOffset.UtcNow;
        var classificationId = Guid.NewGuid();
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(intakeId, "editorial_review", "meme.png", "image/png", 1024, "key", "Draft", null, null, null),
            ApproveSnapshot = new EditorialReviewSnapshot(
                IntakeId: intakeId,
                CandidateClassificationId: classificationId,
                CandidateStatus: "approved",
                EditorialStatus: "approved",
                ClassificationStatus: "approved",
                ReviewStatus: "approved",
                ReviewedByAccountId: reviewerId,
                ReviewedAtUtc: reviewedAt),
        };

        var service = CreateService(store, new FakeStorage(), new FakeEvaluationService());

        var result = await service.ApproveAsync(intakeId, reviewerId, new MemeEditorialDecisionRequest("ok", "note"), CancellationToken.None);

        Assert.Equal("approved", result.Status);
        Assert.Equal("approved", result.ReviewStatus);
        Assert.Equal(reviewerId, result.ReviewedByAccountId);
        Assert.Equal(classificationId, result.CandidateClassificationId);
    }

    [Fact]
    public async Task RejectAsync_ShouldReturnReviewerMetadata()
    {
        var intakeId = Guid.NewGuid();
        var reviewerId = Guid.NewGuid();
        var reviewedAt = DateTimeOffset.UtcNow;
        var store = new FakeStore
        {
            Intake = new MemeIntakeAggregate(intakeId, "editorial_review", "meme.png", "image/png", 1024, "key", "Draft", null, null, null),
            RejectSnapshot = new EditorialReviewSnapshot(
                IntakeId: intakeId,
                CandidateClassificationId: Guid.NewGuid(),
                CandidateStatus: "rejected",
                EditorialStatus: "rejected",
                ClassificationStatus: "rejected",
                ReviewStatus: "rejected",
                ReviewedByAccountId: reviewerId,
                ReviewedAtUtc: reviewedAt),
        };

        var service = CreateService(store, new FakeStorage(), new FakeEvaluationService());

        var result = await service.RejectAsync(intakeId, reviewerId, new MemeEditorialDecisionRequest("fix", "note"), CancellationToken.None);

        Assert.Equal("rejected", result.Status);
        Assert.Equal("rejected", result.ReviewStatus);
        Assert.Equal(reviewerId, result.ReviewedByAccountId);
    }

    private static MemeIntakeService CreateService(FakeStore store, FakeStorage storage, FakeEvaluationService evaluation)
    {
        return new MemeIntakeService(
            store,
            storage,
            evaluation,
            new MemeIntakeOptions
            {
                MaxFileSizeBytes = 10 * 1024 * 1024,
                UploadUrlTtlMinutes = 10,
                AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"],
            });
    }

    private static ClassificationTaxonomySnapshot BuildTaxonomy()
    {
        var axis = new ClassificationAxis
        {
            ClassificationAxisId = Guid.NewGuid(),
            AxisKey = "mechanism",
            DisplayName = "Mechanizm",
            AxisType = "categorical",
            AxisRole = "affinity",
            Cardinality = "multi",
            IsActive = true,
        };

        var model = new ClassificationModelVersion
        {
            ClassificationModelVersionId = Guid.NewGuid(),
            ModelKey = "haia_humor_classification",
            VersionNo = 1,
            VersionLabel = "v1",
            Status = "active",
            Config = "{}",
        };

        var modelAxis = new ClassificationModelAxis
        {
            ClassificationModelAxisId = Guid.NewGuid(),
            ClassificationModelVersionId = model.ClassificationModelVersionId,
            ClassificationAxisId = axis.ClassificationAxisId,
            ClassificationAxis = axis,
            ClassificationModelVersion = model,
            AxisWeight = 1,
            IsRequired = true,
            MinimumAssignments = 1,
            NormalizationMethod = "mean",
            MemberStatus = "active",
        };

        var value = new ClassificationValue
        {
            ClassificationValueId = Guid.NewGuid(),
            ClassificationAxisId = axis.ClassificationAxisId,
            ClassificationAxis = axis,
            ValueKey = "incongruity",
            DisplayName = "Incongruity",
            IsActive = true,
        };

        var modelValue = new ClassificationModelValue
        {
            ClassificationModelValueId = Guid.NewGuid(),
            ClassificationModelVersionId = model.ClassificationModelVersionId,
            ClassificationModelAxisId = modelAxis.ClassificationModelAxisId,
            ClassificationAxisId = axis.ClassificationAxisId,
            ClassificationValueId = value.ClassificationValueId,
            ClassificationModelAxis = modelAxis,
            ClassificationValue = value,
            IsEnabled = true,
        };

        modelAxis.ClassificationModelValues.Add(modelValue);
        model.ClassificationModelAxes.Add(modelAxis);

        return new ClassificationTaxonomySnapshot(
            model,
            new Dictionary<string, ClassificationModelAxis>(StringComparer.Ordinal)
            {
                ["mechanism"] = modelAxis,
                ["absurdity"] = new ClassificationModelAxis
                {
                    ClassificationModelAxisId = Guid.NewGuid(),
                    ClassificationModelVersionId = model.ClassificationModelVersionId,
                    ClassificationAxis = new ClassificationAxis
                    {
                        ClassificationAxisId = Guid.NewGuid(),
                        AxisKey = "absurdity",
                        DisplayName = "Absurdity",
                        AxisType = "scalar",
                        AxisRole = "affinity",
                        Cardinality = "single",
                        IsActive = true,
                    },
                    ClassificationAxisId = Guid.NewGuid(),
                    AxisWeight = 1,
                    IsRequired = true,
                    MinimumAssignments = 1,
                    NormalizationMethod = "mean",
                    MemberStatus = "active",
                },
            },
            new Dictionary<(string AxisKey, string ValueKey), ClassificationModelValue>
            {
                [("mechanism", "incongruity")] = modelValue,
            },
            new Dictionary<string, SensitivityCategory>(StringComparer.Ordinal)
            {
                ["general"] = new SensitivityCategory
                {
                    SensitivityCategoryId = Guid.NewGuid(),
                    CategoryKey = "general",
                    DisplayName = "General",
                    IsActive = true,
                },
            },
            new Dictionary<int, DrynessScaleLevel>
            {
                [3] = new DrynessScaleLevel
                {
                    DrynessScaleLevelId = Guid.NewGuid(),
                    LevelNo = 3,
                    Label = "Średni",
                },
            },
            new Dictionary<string, ReactionMechanism>(StringComparer.Ordinal)
            {
                ["incongruity"] = new ReactionMechanism
                {
                    ReactionMechanismId = Guid.NewGuid(),
                    MechanismKey = "incongruity",
                    DisplayName = "Incongruity",
                    IsActive = true,
                },
            });
    }

    private sealed class FakeStore : IMemeIntakeStore
    {
        public MemeIntakeAggregate Intake { get; set; } =
            new(Guid.NewGuid(), "creating_draft", "meme.png", "image/png", 100, "key", null, null, null, null);

        public ClassificationTaxonomySnapshot Taxonomy { get; set; } = BuildTaxonomy();

        public OnboardingCandidateVersion CandidateVersion { get; set; } = new()
        {
            OnboardingCandidateVersionId = Guid.NewGuid(),
            OnboardingCandidateId = Guid.NewGuid(),
            VersionNo = 1,
            LanguageCode = "pl-PL",
            RoleInFlow = "exploration",
            ModerationStatus = "pending",
        };

        public bool MarkUploadedCalled { get; private set; }

        public bool IsDuplicateUpload { get; set; }

        public bool SaveEvaluationCalled { get; private set; }

        public bool SaveClassificationCalled { get; private set; }

        public EditorialReviewSnapshot? StartSnapshot { get; set; }

        public EditorialReviewSnapshot? ApproveSnapshot { get; set; }

        public EditorialReviewSnapshot? RejectSnapshot { get; set; }

        public Task<MemeIntakeDraft> CreateDraftAsync(Guid accountId, string fileName, string contentType, long sizeBytes, string? workingTitle, CancellationToken cancellationToken)
        {
            var intakeId = Guid.NewGuid();
            Intake = new MemeIntakeAggregate(intakeId, "creating_draft", fileName, contentType, sizeBytes, "key", workingTitle, null, null, null);
            CandidateVersion = new OnboardingCandidateVersion
            {
                OnboardingCandidateVersionId = Guid.NewGuid(),
                OnboardingCandidateId = intakeId,
                VersionNo = 1,
                LanguageCode = "pl-PL",
                RoleInFlow = "exploration",
                ModerationStatus = "pending",
            };

            return Task.FromResult(new MemeIntakeDraft(intakeId, intakeId, CandidateVersion.OnboardingCandidateVersionId, Guid.NewGuid(), "key", fileName, contentType, sizeBytes, workingTitle, "creating_draft"));
        }

        public Task<MemeIntakeAggregate?> GetByIntakeIdAsync(Guid intakeId, CancellationToken cancellationToken)
        {
            return Task.FromResult<MemeIntakeAggregate?>(Intake.IntakeId == intakeId ? Intake : null);
        }

        public Task<bool> IsDuplicateUploadAsync(Guid intakeId, string sha256Hash, CancellationToken cancellationToken)
        {
            return Task.FromResult(IsDuplicateUpload);
        }

        public Task MarkUploadedAsync(Guid intakeId, string sha256Hash, int? widthPx, int? heightPx, CancellationToken cancellationToken)
        {
            MarkUploadedCalled = true;
            Intake = Intake with { Status = "uploaded" };
            return Task.CompletedTask;
        }

        public Task SaveEvaluationAsync(Guid intakeId, Guid aiOperationExecutionId, string structuredOutputJson, string mappedOutputJson, CancellationToken cancellationToken)
        {
            SaveEvaluationCalled = true;
            return Task.CompletedTask;
        }

        public Task SaveFailedEvaluationAsync(Guid intakeId, Guid aiOperationExecutionId, string errorCode, string structuredOutputJson, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<ClassificationTaxonomySnapshot> GetActiveTaxonomyAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Taxonomy);
        }

        public Task<OnboardingCandidateVersion?> GetCandidateVersionAsync(Guid intakeId, CancellationToken cancellationToken)
        {
            return Task.FromResult<OnboardingCandidateVersion?>(CandidateVersion.OnboardingCandidateId == intakeId ? CandidateVersion : null);
        }

        public Task<OnboardingCandidateVersion?> GetCandidateVersionByIdAsync(Guid candidateVersionId, CancellationToken cancellationToken)
        {
            return Task.FromResult<OnboardingCandidateVersion?>(CandidateVersion.OnboardingCandidateVersionId == candidateVersionId ? CandidateVersion : null);
        }

        public Task SaveClassificationAsync(Guid intakeId, Guid aiOperationExecutionId, PersistedMemeEvaluation evaluation, CancellationToken cancellationToken)
        {
            SaveClassificationCalled = true;

            var classification = new CandidateClassification
            {
                CandidateClassificationId = Guid.NewGuid(),
                OnboardingCandidateVersionId = CandidateVersion.OnboardingCandidateVersionId,
                ClassificationModelVersionId = Taxonomy.ActiveModel.ClassificationModelVersionId,
                SourceType = "ai",
                ClassificationStatus = "awaiting_review",
                RevisionNo = 1,
                OverallConfidence = evaluation.OverallConfidence,
                PredictedDrynessScaleLevel = Taxonomy.DrynessLevelsByNo[evaluation.PredictedDrynessLevel],
                PredictedDrynessConfidence = evaluation.PredictedDrynessConfidence,
                CandidateClassificationValues = [],
                CandidateClassificationMeasures = [],
                CandidateClassificationSensitivities = [],
            };

            Intake = Intake with
            {
                Status = "ai_analysis_ready",
                CandidateClassification = classification,
            };

            return Task.CompletedTask;
        }

        public Task<EditorialReviewSnapshot> StartEditorialReviewAsync(Guid intakeId, Guid reviewerAccountId, CancellationToken cancellationToken)
        {
            if (StartSnapshot is null)
            {
                throw new InvalidOperationException("Start snapshot was not configured.");
            }

            return Task.FromResult(StartSnapshot);
        }

        public Task<EditorialReviewSnapshot> ApproveAsync(Guid intakeId, Guid reviewerAccountId, string? summary, string? editorialNote, CancellationToken cancellationToken)
        {
            if (ApproveSnapshot is null)
            {
                throw new InvalidOperationException("Approve snapshot was not configured.");
            }

            return Task.FromResult(ApproveSnapshot);
        }

        public Task<EditorialReviewSnapshot> RejectAsync(Guid intakeId, Guid reviewerAccountId, string? summary, string? editorialNote, CancellationToken cancellationToken)
        {
            if (RejectSnapshot is null)
            {
                throw new InvalidOperationException("Reject snapshot was not configured.");
            }

            return Task.FromResult(RejectSnapshot);
        }
    }

    private sealed class FakeStorage : IMediaObjectStorage
    {
        public MediaObjectMetadata Metadata { get; set; } = new(true, "image/png", 1024, "etag");

        public byte[] Prefix { get; set; } = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];

        public byte[] ObjectBytes { get; set; } = [0x89, 0x50, 0x4E, 0x47];

        public Task<MediaUploadIntent> CreateUploadIntentAsync(string objectKey, string contentType, DateTimeOffset expiresAtUtc, CancellationToken cancellationToken)
            => Task.FromResult(new MediaUploadIntent("https://private-r2/upload", new Dictionary<string, string> { ["Content-Type"] = contentType }, expiresAtUtc));

        public Task<MediaObjectMetadata> GetObjectMetadataAsync(string objectKey, CancellationToken cancellationToken)
            => Task.FromResult(Metadata);

        public Task<byte[]> ReadObjectPrefixAsync(string objectKey, int maxBytes, CancellationToken cancellationToken)
            => Task.FromResult(Prefix);

        public Task<byte[]> ReadObjectBytesAsync(string objectKey, long maxBytes, CancellationToken cancellationToken)
            => Task.FromResult(ObjectBytes);
    }

    private sealed class FakeEvaluationService : IMemeEvaluationService
    {
        public PersistedMemeEvaluation? Result { get; set; }

        public Task<PersistedMemeEvaluation> EvaluateAsync(MemeIntakeAggregate intake, byte[] imageBytes, ClassificationTaxonomySnapshot taxonomy, string correlationId, CancellationToken cancellationToken)
        {
            if (Result is null)
            {
                throw new InvalidOperationException("Result was not configured.");
            }

            return Task.FromResult(Result);
        }
    }
}
