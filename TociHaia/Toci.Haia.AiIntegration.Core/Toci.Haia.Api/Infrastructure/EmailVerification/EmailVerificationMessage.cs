namespace Toci.Haia.Api.Infrastructure.EmailVerification;

public sealed record EmailVerificationMessage(
    string RecipientEmail,
    string Token,
    DateTimeOffset ExpiresAt,
    string Language,
    string CorrelationId);
