using Toci.Haia.Database.Persistence.Entities;

namespace Toci.Haia.Api.Features.Registration.Email;

public interface IEmailRegistrationStore
{
    Task<bool> ExistsActiveEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<bool> ExistsActiveNicknameAsync(string normalizedNickname, CancellationToken cancellationToken);
    Task<IReadOnlyList<LegalDocumentVersion>> GetCurrentRequiredLegalDocumentsAsync(string language, DateTimeOffset now, CancellationToken cancellationToken);
    Task<Account?> FindAccountByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<AccountEmail?> FindPrimaryActiveEmailAsync(Guid accountId, CancellationToken cancellationToken);
    Task<SecurityToken?> FindTokenByHashAsync(string tokenHash, string tokenPurpose, CancellationToken cancellationToken);
    Task<SecurityToken?> FindLatestActiveTokenAsync(Guid accountId, string tokenPurpose, CancellationToken cancellationToken);
    Task<RegistrationInsertResult> InsertRegistrationAsync(RegistrationInsertData data, CancellationToken cancellationToken);
    Task MarkTokenUsedAndActivateAsync(SecurityToken token, CancellationToken cancellationToken);
    Task InvalidateAndCreateResendTokenAsync(SecurityToken? activeToken, Account account, AccountEmail accountEmail, SecurityToken newToken, CancellationToken cancellationToken);
}

public sealed record RegistrationInsertData(
    string EmailOriginal,
    string EmailNormalized,
    string NicknameOriginal,
    string NicknameNormalized,
    string PasswordHash,
    DateTimeOffset Now,
    string CorrelationId,
    string AccountStatus,
    string EmailVerificationStatus,
    string ProfileVisibility,
    IReadOnlyList<LegalDecisionPersisted> LegalDecisions,
    SecurityToken VerificationToken);

public sealed record LegalDecisionPersisted(Guid DocumentVersionId, string Action, DateTimeOffset AcceptedAt);

public sealed record RegistrationInsertResult(bool Success, bool NicknameConflict);
