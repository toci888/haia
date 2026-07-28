using Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;
using Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

namespace Toci.Haia.AiIntegration.Tests;

public sealed class EvaluateStudioMemeImageValidatorsTests
{
    private static readonly EvaluateStudioMemeImageValidationSettings Settings = new();

    [Fact]
    public void RequestValidator_ShouldRejectInvalidImageAndCounts()
    {
        var validator = new EvaluateStudioMemeImageRequestValidator(Settings);
        var request = CreateValidRequest() with
        {
            Image = new ImageInput([], "image/gif"),
            ReactionCount = 10,
            InitiallyVisibleReactionCount = 3,
        };

        var issues = validator.Validate(request);

        Assert.NotEmpty(issues);
        Assert.Contains(issues, x => x.Path == "image.content");
        Assert.Contains(issues, x => x.Path == "image.mimeType");
        Assert.Contains(issues, x => x.Path == "reactionCount");
        Assert.Contains(issues, x => x.Path == "initiallyVisibleReactionCount");
    }

    [Fact]
    public void ResponseValidator_ShouldRejectUnknownValueKey()
    {
        var validator = new EvaluateStudioMemeImageResponseValidator(Settings);
        var request = CreateValidRequest();
        var response = CreateValidResponse() with
        {
            HumorClassifications =
            [
                new HumorClassificationValue
                {
                    AxisKey = "mechanism",
                    ValueKey = "unknown_key",
                    RelevanceScore = 0.8m,
                    Confidence = 0.8m,
                    IsPrimary = true,
                    RankNo = 1,
                },
            ],
        };

        var issues = validator.Validate(request, response);

        Assert.Contains(issues, x => x.Path == "humorClassifications[].valueKey");
    }

    [Fact]
    public void ResponseValidator_ShouldRejectInvalidReactionCount()
    {
        var validator = new EvaluateStudioMemeImageResponseValidator(Settings);
        var request = CreateValidRequest();
        var response = CreateValidResponse() with
        {
            Reactions = CreateValidReactions().Take(11).ToArray(),
        };

        var issues = validator.Validate(request, response);

        Assert.Contains(issues, x => x.Path == "reactions");
    }

    [Fact]
    public void ResponseValidator_ShouldRejectMissingSecondPunchline()
    {
        var validator = new EvaluateStudioMemeImageResponseValidator(Settings);
        var request = CreateValidRequest();
        var reactions = CreateValidReactions();
        reactions[0] = reactions[0] with { SecondPunchline = string.Empty };
        var response = CreateValidResponse() with { Reactions = reactions };

        var issues = validator.Validate(request, response);

        Assert.Contains(issues, x => x.Path == "reactions[].secondPunchline");
    }

    [Fact]
    public void ResponseValidator_ShouldRejectOutOfRangeScores()
    {
        var validator = new EvaluateStudioMemeImageResponseValidator(Settings);
        var request = CreateValidRequest();
        var response = CreateValidResponse() with
        {
            HumorClassifications =
            [
                new HumorClassificationValue
                {
                    AxisKey = "mechanism",
                    ValueKey = "incongruity",
                    RelevanceScore = 1.2m,
                    Confidence = -0.1m,
                    IsPrimary = true,
                    RankNo = 1,
                },
            ],
            NumericMeasures =
            [
                new HumorMeasureValue
                {
                    AxisKey = "absurdity",
                    NormalizedValue = 1.5m,
                    Confidence = 2m,
                },
            ],
        };

        var issues = validator.Validate(request, response);

        Assert.Contains(issues, x => x.Path == "humorClassifications[].relevanceScore");
        Assert.Contains(issues, x => x.Path == "humorClassifications[].confidence");
        Assert.Contains(issues, x => x.Path == "numericMeasures[].normalizedValue");
        Assert.Contains(issues, x => x.Path == "numericMeasures[].confidence");
    }

    private static EvaluateStudioMemeImageRequest CreateValidRequest()
    {
        return new EvaluateStudioMemeImageRequest
        {
            CorrelationId = "corr-1",
            Image = new ImageInput([0x89, 0x50, 0x4E, 0x47], "image/png"),
            ReactionCount = 12,
            InitiallyVisibleReactionCount = 6,
            Taxonomy = new MemeEvaluationTaxonomy
            {
                CategoricalAxisValues = new Dictionary<string, IReadOnlyCollection<string>>(StringComparer.Ordinal)
                {
                    ["mechanism"] = ["incongruity"],
                },
                NumericAxisKeys = ["absurdity", "intensity", "complexity", "universality", "rescue_potential"],
                SensitivityCategoryKeys = ["general"],
                ReactionMechanismKeys = ["incongruity"],
                DrynessLevels = [1, 2, 3, 4, 5, 6],
            },
        };
    }

    private static EvaluateStudioMemeImageResponse CreateValidResponse()
    {
        return new EvaluateStudioMemeImageResponse
        {
            SuggestedTitle = "Tytuł",
            VisualDescription = "Opis",
            DetectedText = "Tekst",
            LanguageCode = "pl-PL",
            ContentFormat = "image",
            SuggestedRoleInFlow = "exploration",
            SuggestedRoleConfidence = 0.8m,
            EditorialSummary = "Summary",
            OverallConfidence = 0.9m,
            HumorClassifications =
            [
                new HumorClassificationValue
                {
                    AxisKey = "mechanism",
                    ValueKey = "incongruity",
                    RelevanceScore = 0.8m,
                    Confidence = 0.9m,
                    IsPrimary = true,
                    RankNo = 1,
                },
            ],
            NumericMeasures =
            [
                new HumorMeasureValue
                {
                    AxisKey = "absurdity",
                    NormalizedValue = 0.5m,
                    Confidence = 0.7m,
                },
            ],
            Safety =
            [
                new SafetyClassificationValue
                {
                    CategoryKey = "general",
                    SeverityLevel = 1,
                    Confidence = 0.7m,
                    ModerationRelevance = true,
                },
            ],
            ModerationRecommendation = "review",
            ModerationSummary = "summary",
            PredictedDrynessLevel = 3,
            PredictedDrynessConfidence = 0.7m,
            Reactions = CreateValidReactions(),
        };
    }

    private static MemeReactionProposal[] CreateValidReactions()
    {
        var result = new MemeReactionProposal[12];
        for (var i = 0; i < 12; i++)
        {
            result[i] = new MemeReactionProposal
            {
                Text = $"Reaction {i + 1}",
                DisplayOrder = i + 1,
                InitiallyVisible = i < 6,
                RelevanceScore = 0.8m,
                Confidence = 0.8m,
                SecondPunchline = $"Second {i + 1}",
                MechanismKeys = ["incongruity"],
            };
        }

        return result;
    }
}
