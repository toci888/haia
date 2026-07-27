using Toci.Haia.Api.Features.LegalDocuments.Contracts;

namespace Toci.Haia.Api.Features.LegalDocuments;

public interface ILegalDocumentsService
{
    Task<CurrentLegalDocumentsResponse> GetCurrentAsync(string? language, CancellationToken cancellationToken);
    Task<IReadOnlyList<CurrentLegalDocumentItem>> GetRequiredCurrentForRegistrationAsync(string language, CancellationToken cancellationToken);
}
