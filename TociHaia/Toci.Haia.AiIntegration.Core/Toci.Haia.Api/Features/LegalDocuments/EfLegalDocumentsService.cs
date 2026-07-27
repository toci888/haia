using Microsoft.Extensions.Options;
using Toci.Haia.Api.Features.LegalDocuments.Contracts;
using Toci.Haia.Api.Features.Registration.Email;

namespace Toci.Haia.Api.Features.LegalDocuments;

public sealed class EfLegalDocumentsService(
    IEmailRegistrationStore registrationStore,
    IOptions<EmailRegistrationOptions> options,
    TimeProvider timeProvider) : ILegalDocumentsService
{
    public async Task<CurrentLegalDocumentsResponse> GetCurrentAsync(string? language, CancellationToken cancellationToken)
    {
        var normalizedLanguage = EmailRegistrationNormalization.NormalizeLanguage(language, options.Value);
        var now = timeProvider.GetUtcNow();

        var docs = await registrationStore.GetCurrentRequiredLegalDocumentsAsync(normalizedLanguage, now, cancellationToken);
        var response = docs
            .Select(v => new CurrentLegalDocumentItem(
                v.LegalDocumentVersionId,
                v.LegalDocument.DocumentKey,
                v.VersionLabel,
                "accepted",
                v.ValidFrom,
                v.ValidTo,
                v.LanguageCode))
            .ToList();

        return new CurrentLegalDocumentsResponse(response);
    }

    public async Task<IReadOnlyList<CurrentLegalDocumentItem>> GetRequiredCurrentForRegistrationAsync(string language, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var docs = await registrationStore.GetCurrentRequiredLegalDocumentsAsync(language, now, cancellationToken);
        return docs
            .Select(v => new CurrentLegalDocumentItem(
                v.LegalDocumentVersionId,
                v.LegalDocument.DocumentKey,
                v.VersionLabel,
                "accepted",
                v.ValidFrom,
                v.ValidTo,
                v.LanguageCode))
            .ToList();
    }
}
