namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

public interface IMemeIntakeService
{
    Task<CreateMemeIntakeResponse> CreateAsync(Guid accountId, CreateMemeIntakeRequest request, CancellationToken cancellationToken);

    Task<FinalizeMemeIntakeResponse> FinalizeAsync(Guid intakeId, CancellationToken cancellationToken);

    Task<EvaluateMemeIntakeResponse> EvaluateAsync(Guid intakeId, string correlationId, CancellationToken cancellationToken);

    Task<StartEditorialReviewResponse> StartEditorialReviewAsync(Guid intakeId, Guid reviewerAccountId, CancellationToken cancellationToken);

    Task<MemeEditorialDecisionResponse> ApproveAsync(Guid intakeId, Guid reviewerAccountId, MemeEditorialDecisionRequest request, CancellationToken cancellationToken);

    Task<MemeEditorialDecisionResponse> RejectAsync(Guid intakeId, Guid reviewerAccountId, MemeEditorialDecisionRequest request, CancellationToken cancellationToken);

    Task<GetMemeIntakeResponse> GetAsync(Guid intakeId, CancellationToken cancellationToken);
}
