namespace Toci.Haia.Api.Common.Errors;

public static class ErrorCodes
{
    public const string ValidationFailed = "validation_failed";
    public const string NicknameUnavailable = "nickname_unavailable";
    public const string LegalDocumentsChanged = "legal_documents_changed";
    public const string LegalDecisionsIncomplete = "legal_decisions_incomplete";
    public const string InvalidOrExpiredVerificationToken = "invalid_or_expired_verification_token";
    public const string VerificationDeliveryUnavailable = "verification_delivery_unavailable";
    public const string DatabaseUnavailable = "database_unavailable";
    public const string UnexpectedError = "unexpected_error";
}
