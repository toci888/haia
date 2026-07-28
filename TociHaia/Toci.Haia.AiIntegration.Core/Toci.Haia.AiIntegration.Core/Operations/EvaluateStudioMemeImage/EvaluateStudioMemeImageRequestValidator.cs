namespace Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;

using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Models;

public sealed class EvaluateStudioMemeImageRequestValidator(EvaluateStudioMemeImageValidationSettings settings)
    : IAiRequestValidator<EvaluateStudioMemeImageRequest>
{
    private static readonly HashSet<string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
    };

    public IReadOnlyList<ValidationIssue> Validate(EvaluateStudioMemeImageRequest request, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return [new ValidationIssue("$", "Operation was cancelled.")];
        }

        var issues = new List<ValidationIssue>();

        if (request.Image.Content is null || request.Image.Content.Length == 0)
        {
            issues.Add(new ValidationIssue("image.content", "Image content cannot be empty."));
        }

        if (request.Image.Content is not null && request.Image.Content.Length > settings.MaxImageBytes)
        {
            issues.Add(new ValidationIssue("image.content", "Image exceeds configured maximum size."));
        }

        if (string.IsNullOrWhiteSpace(request.Image.MimeType) || !AllowedMimeTypes.Contains(request.Image.MimeType))
        {
            issues.Add(new ValidationIssue("image.mimeType", "Only image/jpeg, image/png and image/webp are supported."));
        }

        if (string.IsNullOrWhiteSpace(request.CorrelationId))
        {
            issues.Add(new ValidationIssue("correlationId", "CorrelationId is required."));
        }

        if (request.ReactionCount != 12)
        {
            issues.Add(new ValidationIssue("reactionCount", "ReactionCount must be exactly 12."));
        }

        if (request.InitiallyVisibleReactionCount != 6)
        {
            issues.Add(new ValidationIssue("initiallyVisibleReactionCount", "InitiallyVisibleReactionCount must be exactly 6."));
        }

        if (request.Taxonomy.CategoricalAxisValues.Count == 0)
        {
            issues.Add(new ValidationIssue("taxonomy.categoricalAxisValues", "At least one categorical axis is required."));
        }

        if (request.Taxonomy.NumericAxisKeys.Count == 0)
        {
            issues.Add(new ValidationIssue("taxonomy.numericAxisKeys", "Numeric axis keys are required."));
        }

        if (request.Taxonomy.SensitivityCategoryKeys.Count == 0)
        {
            issues.Add(new ValidationIssue("taxonomy.sensitivityCategoryKeys", "Safety categories are required."));
        }

        if (request.Taxonomy.ReactionMechanismKeys.Count == 0)
        {
            issues.Add(new ValidationIssue("taxonomy.reactionMechanismKeys", "Reaction mechanism keys are required."));
        }

        if (request.Taxonomy.DrynessLevels.Count == 0)
        {
            issues.Add(new ValidationIssue("taxonomy.drynessLevels", "Dryness levels are required."));
        }

        return issues;
    }
}
