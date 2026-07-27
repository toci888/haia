namespace Toci.Haia.Api.Features.LegalDocuments.Contracts;

public sealed record CurrentLegalDocumentsResponse(IReadOnlyList<CurrentLegalDocumentItem> Documents);

public sealed record CurrentLegalDocumentItem(
    Guid DocumentVersionId,
    string DocumentKey,
    string Version,
    string RequiredAction,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo,
    string Language);
