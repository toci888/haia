namespace Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Lokalna walidacja żądania operacji multimodalnej.
/// </summary>
public sealed class GenerateCustomReactionsFromImageRequestValidator(GenerateCustomReactionsValidationSettings settings)
    : IAiRequestValidator<GenerateCustomReactionsFromImageRequest>
{
    private static readonly HashSet<string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp",
    };

    private readonly GenerateCustomReactionsValidationSettings _settings = settings;

    public IReadOnlyList<ValidationIssue> Validate(GenerateCustomReactionsFromImageRequest request, CancellationToken cancellationToken = default)
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

        if (string.IsNullOrWhiteSpace(request.Image.MimeType) || !AllowedMimeTypes.Contains(request.Image.MimeType))
        {
            issues.Add(new ValidationIssue("image.mimeType", "Only image/jpeg, image/png and image/webp are supported."));
        }

        if (request.Image.Content is not null && request.Image.Content.Length > _settings.MaxImageBytes)
        {
            issues.Add(new ValidationIssue("image.content", "Image exceeds configured maximum size."));
        }

        if (string.IsNullOrWhiteSpace(request.OutputLanguage))
        {
            issues.Add(new ValidationIssue("outputLanguage", "Output language is required."));
        }

        if (request.RequestedReactionCount <= 0)
        {
            issues.Add(new ValidationIssue("requestedReactionCount", "Requested reaction count must be greater than zero."));
        }

        if (request.RequestedReactionCount > _settings.MaxReactionCount)
        {
            issues.Add(new ValidationIssue("requestedReactionCount", $"Requested reaction count must be <= {_settings.MaxReactionCount}."));
        }

        if (request.DefaultVisibleReactionCount <= 0)
        {
            issues.Add(new ValidationIssue("defaultVisibleReactionCount", "Default visible reaction count must be greater than zero."));
        }

        if (request.DefaultVisibleReactionCount > request.RequestedReactionCount)
        {
            issues.Add(new ValidationIssue("defaultVisibleReactionCount", "Default visible reaction count cannot exceed requested reaction count."));
        }

        if (request.PreferredIntensity is < 1 or > 5)
        {
            issues.Add(new ValidationIssue("preferredIntensity", "Preferred intensity must be in range 1..5."));
        }

        return issues;
    }
}