namespace Toci.Haia.Studio.Api.Common.Errors;

public static class ErrorCodes
{
    public const string DatabaseUnavailable = "database_unavailable";
    public const string InvalidCredentials = "invalid_credentials";
    public const string CsrfValidationFailed = "csrf_validation_failed";
    public const string UnexpectedError = "unexpected_error";
    public const string ValidationFailed = "validation_failed";
    public const string UnsupportedMediaType = "unsupported_media_type";
    public const string FileTooLarge = "file_too_large";
    public const string IntakeNotFound = "meme_intake_not_found";
    public const string UploadObjectMissing = "upload_object_missing";
    public const string UploadObjectInvalid = "upload_object_invalid";
    public const string UploadObjectDuplicate = "upload_object_duplicate";
    public const string IntakeNotUploaded = "meme_intake_not_uploaded";
    public const string EvaluationAlreadyRunning = "meme_evaluation_already_running";
    public const string EvaluationAlreadyCompleted = "meme_evaluation_already_completed";
    public const string EvaluationRetryNotAllowed = "meme_evaluation_retry_not_allowed";
    public const string IntakeNotReadyForReview = "meme_intake_not_ready_for_review";
    public const string AiEvaluationFailed = "ai_evaluation_failed";
    public const string TaxonomyMismatch = "taxonomy_mismatch";
    public const string StorageNotConfigured = "storage_not_configured";
}
