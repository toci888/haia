namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

using System.Text.Json;
using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Models;
using Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;
using Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;
using Toci.Haia.Studio.Api.Common.Errors;

public sealed class MemeEvaluationService(
    IAiOperation<EvaluateStudioMemeImageRequest, EvaluateStudioMemeImageResponse> operation,
    EvaluateStudioMemeImageRequestValidator requestValidator,
    EvaluateStudioMemeImageResponseValidator responseValidator) : IMemeEvaluationService
{
    public async Task<PersistedMemeEvaluation> EvaluateAsync(
        MemeIntakeAggregate intake,
        byte[] imageBytes,
        ClassificationTaxonomySnapshot taxonomy,
        string correlationId,
        CancellationToken cancellationToken)
    {
        var taxonomyInput = new MemeEvaluationTaxonomy
        {
            CategoricalAxisValues = taxonomy.AxisByKey
                .ToDictionary(
                    kv => kv.Key,
                    kv => (IReadOnlyCollection<string>)taxonomy.ValueByAxisAndKey.Keys
                        .Where(v => v.AxisKey == kv.Key)
                        .Select(v => v.ValueKey)
                        .Distinct(StringComparer.Ordinal)
                        .ToArray(),
                    StringComparer.Ordinal),
            NumericAxisKeys = taxonomy.AxisByKey
                .Where(x => string.Equals(x.Value.ClassificationAxis.AxisType, "scalar", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Key)
                .ToArray(),
            SensitivityCategoryKeys = taxonomy.SensitivityByKey.Keys.ToArray(),
            ReactionMechanismKeys = taxonomy.ReactionMechanismsByKey.Keys.ToArray(),
            DrynessLevels = taxonomy.DrynessLevelsByNo.Keys.Order().ToArray(),
        };

        var request = new EvaluateStudioMemeImageRequest
        {
            CorrelationId = correlationId,
            Image = new ImageInput(imageBytes, intake.ContentType),
            OutputLanguage = "pl-PL",
            Taxonomy = taxonomyInput,
            ReactionCount = 12,
            InitiallyVisibleReactionCount = 6,
        };

        var requestIssues = requestValidator.Validate(request, cancellationToken);
        if (requestIssues.Count > 0)
        {
            throw Validation(requestIssues);
        }

        var result = await operation.ExecuteAsync(request, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
        {
            var message = result.Error?.Message ?? "AI evaluation failed.";
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadGateway,
                ErrorCodes.AiEvaluationFailed,
                "AI evaluation failed",
                message);
        }

        var responseIssues = responseValidator.Validate(request, result.Value, cancellationToken);
        if (responseIssues.Count > 0)
        {
            throw Validation(responseIssues);
        }

        return new PersistedMemeEvaluation(
            OverallConfidence: result.Value.OverallConfidence,
            SuggestedTitle: result.Value.SuggestedTitle,
            VisualDescription: result.Value.VisualDescription,
            DetectedText: result.Value.DetectedText,
            LanguageCode: result.Value.LanguageCode,
            ContentFormat: result.Value.ContentFormat,
            SuggestedRoleInFlow: result.Value.SuggestedRoleInFlow,
            SuggestedRoleConfidence: result.Value.SuggestedRoleConfidence,
            EditorialSummary: result.Value.EditorialSummary,
            PredictedDrynessLevel: result.Value.PredictedDrynessLevel,
            PredictedDrynessConfidence: result.Value.PredictedDrynessConfidence,
            ModerationRecommendation: result.Value.ModerationRecommendation,
            ModerationSummary: result.Value.ModerationSummary,
            Classifications: result.Value.HumorClassifications.Select(x =>
                new PersistedMemeClassificationValue(x.AxisKey, x.ValueKey, x.RelevanceScore, x.Confidence, x.IsPrimary, x.RankNo)).ToArray(),
            Measures: result.Value.NumericMeasures.Select(x =>
                new PersistedMemeMeasureValue(x.AxisKey, x.NormalizedValue, x.Confidence)).ToArray(),
            Safety: result.Value.Safety.Select(x =>
                new PersistedMemeSafetyValue(x.CategoryKey, x.SeverityLevel, x.Confidence, x.ModerationRelevance)).ToArray(),
            Reactions: result.Value.Reactions.Select(x =>
                new PersistedMemeReactionValue(x.Text, x.DisplayOrder, x.InitiallyVisible, x.RelevanceScore, x.Confidence, x.SecondPunchline, x.MechanismKeys)).ToArray());
    }

    public static string ToStructuredOutputJson(PersistedMemeEvaluation evaluation)
        => JsonSerializer.Serialize(evaluation);

    private static StudioProblemDetailsException Validation(IReadOnlyList<ValidationIssue> issues)
    {
        var issueText = string.Join("; ", issues.Select(x => $"{x.Path}: {x.Message}"));
        return new StudioProblemDetailsException(
            System.Net.HttpStatusCode.BadRequest,
            ErrorCodes.ValidationFailed,
            "Validation failed",
            issueText);
    }
}
