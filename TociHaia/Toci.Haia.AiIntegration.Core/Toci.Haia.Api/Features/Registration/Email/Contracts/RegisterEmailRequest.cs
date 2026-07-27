namespace Toci.Haia.Api.Features.Registration.Email.Contracts;

public sealed record RegisterEmailRequest(
    string Email,
    string Password,
    string Nickname,
    string? Language,
    IReadOnlyList<LegalDecisionRequest>? LegalDecisions);

public sealed record LegalDecisionRequest(
    Guid DocumentVersionId,
    string Action);
