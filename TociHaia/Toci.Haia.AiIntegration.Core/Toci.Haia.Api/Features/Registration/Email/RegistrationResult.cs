namespace Toci.Haia.Api.Features.Registration.Email;

public enum RegistrationResult
{
    Accepted,
    NicknameUnavailable,
    LegalDocumentsChanged,
    LegalDecisionsIncomplete,
    VerificationDeliveryUnavailable
}
