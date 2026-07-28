namespace Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;

using Toci.Haia.AiIntegration.Core.Operations.GenerateCustomReactionsFromImage;

public sealed record EvaluateStudioMemeImageRequest
{
    public required ImageInput Image { get; init; }

    public required string CorrelationId { get; init; }

    public string OutputLanguage { get; init; } = "pl-PL";

    public required MemeEvaluationTaxonomy Taxonomy { get; init; }

    public int ReactionCount { get; init; } = 12;

    public int InitiallyVisibleReactionCount { get; init; } = 6;
}

public sealed record MemeEvaluationTaxonomy
{
    public required IReadOnlyDictionary<string, IReadOnlyCollection<string>> CategoricalAxisValues { get; init; }

    public required IReadOnlyCollection<string> NumericAxisKeys { get; init; }

    public required IReadOnlyCollection<string> SensitivityCategoryKeys { get; init; }

    public required IReadOnlyCollection<string> ReactionMechanismKeys { get; init; }

    public required IReadOnlyCollection<int> DrynessLevels { get; init; }
}
