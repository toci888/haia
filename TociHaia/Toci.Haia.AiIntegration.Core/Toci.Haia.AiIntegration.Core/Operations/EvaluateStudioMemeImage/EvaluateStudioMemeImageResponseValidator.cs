namespace Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;

using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Models;

public sealed class EvaluateStudioMemeImageResponseValidator(EvaluateStudioMemeImageValidationSettings settings)
    : IAiResponseValidator<EvaluateStudioMemeImageRequest, EvaluateStudioMemeImageResponse>
{
    public IReadOnlyList<ValidationIssue> Validate(
        EvaluateStudioMemeImageRequest request,
        EvaluateStudioMemeImageResponse response,
        CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return [new ValidationIssue("$", "Operation was cancelled.")];
        }

        var issues = new List<ValidationIssue>();
        var allowedAxisKeys = request.Taxonomy.CategoricalAxisValues.Keys.ToHashSet(StringComparer.Ordinal);
        var allowedNumericAxisKeys = request.Taxonomy.NumericAxisKeys.ToHashSet(StringComparer.Ordinal);
        var allowedSensitivity = request.Taxonomy.SensitivityCategoryKeys.ToHashSet(StringComparer.Ordinal);
        var allowedMechanisms = request.Taxonomy.ReactionMechanismKeys.ToHashSet(StringComparer.Ordinal);
        var allowedDryness = request.Taxonomy.DrynessLevels.ToHashSet();

        if (string.IsNullOrWhiteSpace(response.SuggestedTitle) || response.SuggestedTitle.Length > settings.MaxTitleLength)
        {
            issues.Add(new ValidationIssue("suggestedTitle", "Suggested title is missing or too long."));
        }

        if (string.IsNullOrWhiteSpace(response.VisualDescription) || response.VisualDescription.Length > settings.MaxDescriptionLength)
        {
            issues.Add(new ValidationIssue("visualDescription", "Visual description is missing or too long."));
        }

        if (response.DetectedText?.Length > settings.MaxDetectedTextLength)
        {
            issues.Add(new ValidationIssue("detectedText", "Detected text is too long."));
        }

        if (response.EditorialSummary?.Length > settings.MaxEditorialSummaryLength)
        {
            issues.Add(new ValidationIssue("editorialSummary", "Editorial summary is too long."));
        }

        ValidateRange(response.SuggestedRoleConfidence, "suggestedRoleConfidence", issues);
        ValidateRange(response.OverallConfidence, "overallConfidence", issues);
        ValidateRange(response.PredictedDrynessConfidence, "predictedDrynessConfidence", issues);

        if (!allowedDryness.Contains(response.PredictedDrynessLevel))
        {
            issues.Add(new ValidationIssue("predictedDrynessLevel", "Predicted dryness level is not present in taxonomy."));
        }

        foreach (var value in response.HumorClassifications)
        {
            if (!allowedAxisKeys.Contains(value.AxisKey))
            {
                issues.Add(new ValidationIssue("humorClassifications[].axisKey", $"Unknown axis key: {value.AxisKey}"));
                continue;
            }

            var axisValues = request.Taxonomy.CategoricalAxisValues[value.AxisKey];
            if (!axisValues.Contains(value.ValueKey, StringComparer.Ordinal))
            {
                issues.Add(new ValidationIssue("humorClassifications[].valueKey", $"Unknown value key for axis {value.AxisKey}: {value.ValueKey}"));
            }

            ValidateRange(value.RelevanceScore, "humorClassifications[].relevanceScore", issues);
            ValidateRange(value.Confidence, "humorClassifications[].confidence", issues);
        }

        foreach (var value in response.NumericMeasures)
        {
            if (!allowedNumericAxisKeys.Contains(value.AxisKey))
            {
                issues.Add(new ValidationIssue("numericMeasures[].axisKey", $"Unknown numeric axis key: {value.AxisKey}"));
            }

            ValidateRange(value.NormalizedValue, "numericMeasures[].normalizedValue", issues);
            ValidateRange(value.Confidence, "numericMeasures[].confidence", issues);
        }

        foreach (var value in response.Safety)
        {
            if (!allowedSensitivity.Contains(value.CategoryKey))
            {
                issues.Add(new ValidationIssue("safety[].categoryKey", $"Unknown safety category: {value.CategoryKey}"));
            }

            if (value.SeverityLevel is < 0 or > 3)
            {
                issues.Add(new ValidationIssue("safety[].severityLevel", "Severity level must be in range 0..3."));
            }

            ValidateRange(value.Confidence, "safety[].confidence", issues);
        }

        if (response.Reactions.Count != request.ReactionCount)
        {
            issues.Add(new ValidationIssue("reactions", "Response must contain exactly 12 reactions."));
            return issues;
        }

        var visibleCount = response.Reactions.Count(r => r.InitiallyVisible);
        if (visibleCount != request.InitiallyVisibleReactionCount)
        {
            issues.Add(new ValidationIssue("reactions", "Exactly 6 reactions must be initially visible."));
        }

        foreach (var reaction in response.Reactions)
        {
            if (string.IsNullOrWhiteSpace(reaction.Text) || reaction.Text.Length > settings.MaxReactionTextLength)
            {
                issues.Add(new ValidationIssue("reactions[].text", "Reaction text is missing or too long."));
            }

            if (string.IsNullOrWhiteSpace(reaction.SecondPunchline) || reaction.SecondPunchline.Length > settings.MaxSecondPunchlineLength)
            {
                issues.Add(new ValidationIssue("reactions[].secondPunchline", "Second punchline is required and must fit length limits."));
            }

            if (reaction.DisplayOrder is < 1 or > 12)
            {
                issues.Add(new ValidationIssue("reactions[].displayOrder", "Display order must be in range 1..12."));
            }

            ValidateRange(reaction.RelevanceScore, "reactions[].relevanceScore", issues);
            ValidateRange(reaction.Confidence, "reactions[].confidence", issues);

            if (reaction.MechanismKeys.Count == 0)
            {
                issues.Add(new ValidationIssue("reactions[].mechanismKeys", "At least one mechanism key is required."));
            }

            foreach (var mechanismKey in reaction.MechanismKeys)
            {
                if (!allowedMechanisms.Contains(mechanismKey))
                {
                    issues.Add(new ValidationIssue("reactions[].mechanismKeys", $"Unknown mechanism key: {mechanismKey}"));
                }
            }
        }

        return issues;
    }

    private static void ValidateRange(decimal value, string path, ICollection<ValidationIssue> issues)
    {
        if (value is < 0 or > 1)
        {
            issues.Add(new ValidationIssue(path, "Value must be in range 0..1."));
        }
    }
}
