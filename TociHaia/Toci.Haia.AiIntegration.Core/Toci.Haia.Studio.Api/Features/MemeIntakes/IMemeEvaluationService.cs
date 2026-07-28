namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

public interface IMemeEvaluationService
{
    Task<PersistedMemeEvaluation> EvaluateAsync(
        MemeIntakeAggregate intake,
        byte[] imageBytes,
        ClassificationTaxonomySnapshot taxonomy,
        string correlationId,
        CancellationToken cancellationToken);
}
