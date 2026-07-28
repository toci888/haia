namespace Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;

public sealed record EvaluateStudioMemeImageValidationSettings
{
    public int MaxImageBytes { get; init; } = 10 * 1024 * 1024;

    public int MaxTitleLength { get; init; } = 180;

    public int MaxDescriptionLength { get; init; } = 3000;

    public int MaxDetectedTextLength { get; init; } = 3000;

    public int MaxEditorialSummaryLength { get; init; } = 2000;

    public int MaxReactionTextLength { get; init; } = 240;

    public int MaxSecondPunchlineLength { get; init; } = 240;
}
