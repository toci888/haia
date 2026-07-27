namespace Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Models;

/// <summary>
/// Semantyczna walidacja odpowiedzi wygenerowanej przez AI.
/// </summary>
public sealed class GenerateCustomReactionsFromImageResponseValidator(GenerateCustomReactionsValidationSettings settings)
    : IAiResponseValidator<GenerateCustomReactionsFromImageRequest, GenerateCustomReactionsFromImageResponse>
{
    private readonly GenerateCustomReactionsValidationSettings _settings = settings;

    public IReadOnlyList<ValidationIssue> Validate(
        GenerateCustomReactionsFromImageRequest request,
        GenerateCustomReactionsFromImageResponse response,
        CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return [new ValidationIssue("$", "Operation was cancelled.")];
        }

        var issues = new List<ValidationIssue>();

        if (response.Reactions is null || response.Reactions.Count == 0)
        {
            issues.Add(new ValidationIssue("reactions", "Response must contain reactions."));
            return issues;
        }

        if (response.Reactions.Count != request.RequestedReactionCount)
        {
            issues.Add(new ValidationIssue("reactions", "Reaction count does not match request."));
        }

        var labels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var defaultVisibleCount = 0;

        foreach (var reaction in response.Reactions)
        {
            if (string.IsNullOrWhiteSpace(reaction.Label))
            {
                issues.Add(new ValidationIssue("reactions[].label", "Reaction label cannot be empty."));
            }
            else
            {
                if (reaction.Label.Length > _settings.MaxLabelLength)
                {
                    issues.Add(new ValidationIssue("reactions[].label", "Reaction label is too long."));
                }

                if (!labels.Add(reaction.Label.Trim()))
                {
                    issues.Add(new ValidationIssue("reactions[].label", "Reaction labels must be unique."));
                }
            }

            if (reaction.Intensity is < 1 or > 5)
            {
                issues.Add(new ValidationIssue("reactions[].intensity", "Reaction intensity must be in range 1..5."));
            }

            if (!request.IncludeSecondPunchlines && !string.IsNullOrWhiteSpace(reaction.SecondPunchline))
            {
                issues.Add(new ValidationIssue("reactions[].secondPunchline", "Second punchline is not allowed for this request."));
            }

            if (request.IncludeSecondPunchlines && string.IsNullOrWhiteSpace(reaction.SecondPunchline))
            {
                issues.Add(new ValidationIssue("reactions[].secondPunchline", "Second punchline is required by request."));
            }

            if (reaction.SecondPunchline?.Length > _settings.MaxSecondPunchlineLength)
            {
                issues.Add(new ValidationIssue("reactions[].secondPunchline", "Second punchline is too long."));
            }

            if (reaction.EditorialNote?.Length > _settings.MaxEditorialNoteLength)
            {
                issues.Add(new ValidationIssue("reactions[].editorialNote", "Editorial note is too long."));
            }

            if (reaction.IsDefaultVisible)
            {
                defaultVisibleCount++;
            }
        }

        if (defaultVisibleCount > request.DefaultVisibleReactionCount)
        {
            issues.Add(new ValidationIssue("reactions", "Default visible reactions exceed configured limit."));
        }

        return issues;
    }
}