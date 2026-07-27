using Microsoft.EntityFrameworkCore;
using Npgsql;
using Toci.Haia.Database.Persistence.Context;
using Toci.Haia.Database.Persistence.Entities;

namespace Toci.Haia.Api.Features.Registration.Email;

public sealed class EfEmailRegistrationStore(HaiaDbContext db) : IEmailRegistrationStore
{
    public Task<bool> ExistsActiveEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        => db.AccountEmails.AnyAsync(x => x.EmailNormalized == normalizedEmail && x.ReleasedAt == null, cancellationToken);

    public Task<bool> ExistsActiveNicknameAsync(string normalizedNickname, CancellationToken cancellationToken)
        => db.PublicProfiles.AnyAsync(x => x.NicknameNormalized == normalizedNickname && x.ReleasedAt == null, cancellationToken);

    public async Task<IReadOnlyList<LegalDocumentVersion>> GetCurrentRequiredLegalDocumentsAsync(string language, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var nowUtc = now.UtcDateTime;
        return await db.LegalDocumentVersions
            .Include(v => v.LegalDocument)
            .Where(v => v.LanguageCode == language
                && v.Status == "active"
                && v.ValidFrom <= nowUtc
                && (v.ValidTo == null || v.ValidTo > nowUtc)
                && v.LegalDocument.IsActive
                && v.LegalDocument.IsRequired)
            .ToListAsync(cancellationToken);
    }

    public async Task<Account?> FindAccountByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await db.AccountEmails
            .Include(e => e.Account)
            .Where(e => e.EmailNormalized == normalizedEmail && e.ReleasedAt == null)
            .Select(e => e.Account)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<AccountEmail?> FindPrimaryActiveEmailAsync(Guid accountId, CancellationToken cancellationToken)
        => db.AccountEmails.FirstOrDefaultAsync(e => e.AccountId == accountId && e.IsPrimary && e.ReleasedAt == null, cancellationToken);

    public Task<SecurityToken?> FindTokenByHashAsync(string tokenHash, string tokenPurpose, CancellationToken cancellationToken)
        => db.SecurityTokens
            .Include(t => t.Account)
            .Include(t => t.AccountEmail)
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash && t.TokenPurpose == tokenPurpose, cancellationToken);

    public Task<SecurityToken?> FindLatestActiveTokenAsync(Guid accountId, string tokenPurpose, CancellationToken cancellationToken)
        => db.SecurityTokens
            .Where(t => t.AccountId == accountId
                && t.TokenPurpose == tokenPurpose
                && t.UsedAt == null
                && t.InvalidatedAt == null)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<RegistrationInsertResult> InsertRegistrationAsync(RegistrationInsertData data, CancellationToken cancellationToken)
    {
        await using var tx = await db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var account = new Account
            {
                AccountStatus = data.AccountStatus,
                CreatedAt = data.Now.UtcDateTime,
                StatusChangedAt = data.Now.UtcDateTime,
                RowVersion = 1
            };

            db.Accounts.Add(account);
            await db.SaveChangesAsync(cancellationToken);

            var email = new AccountEmail
            {
                AccountId = account.AccountId,
                EmailOriginal = data.EmailOriginal,
                EmailNormalized = data.EmailNormalized,
                IsPrimary = true,
                VerificationStatus = data.EmailVerificationStatus,
                CreatedAt = data.Now.UtcDateTime,
                UpdatedAt = data.Now.UtcDateTime
            };
            db.AccountEmails.Add(email);

            var password = new PasswordCredential
            {
                AccountId = account.AccountId,
                PasswordHash = data.PasswordHash,
                HashAlgorithm = "aspnetcore_passwordhasher_v3",
                HashFormatVersion = "v3",
                CreatedAt = data.Now.UtcDateTime,
                ChangedAt = data.Now.UtcDateTime,
                ForceChangeRequired = false,
                IsActive = true
            };
            db.PasswordCredentials.Add(password);

            var profile = new PublicProfile
            {
                AccountId = account.AccountId,
                Nickname = data.NicknameOriginal,
                NicknameNormalized = data.NicknameNormalized,
                ProfileVisibility = data.ProfileVisibility,
                CreatedAt = data.Now.UtcDateTime,
                UpdatedAt = data.Now.UtcDateTime
            };
            db.PublicProfiles.Add(profile);

            await db.SaveChangesAsync(cancellationToken);

            foreach (var decision in data.LegalDecisions)
            {
                db.AccountDocumentAcceptances.Add(new AccountDocumentAcceptance
                {
                    AccountId = account.AccountId,
                    LegalDocumentVersionId = decision.DocumentVersionId,
                    AcceptanceType = decision.Action,
                    AcceptedAt = decision.AcceptedAt.UtcDateTime,
                    AcceptedVia = "registration_form"
                });
            }

            var token = data.VerificationToken;
            token.AccountId = account.AccountId;
            token.AccountEmailId = email.AccountEmailId;
            db.SecurityTokens.Add(token);

            await db.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
            return new RegistrationInsertResult(true, false);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pg && pg.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            await tx.RollbackAsync(cancellationToken);
            if (pg.ConstraintName is "ux_public_profile_nickname_active")
            {
                return new RegistrationInsertResult(false, true);
            }

            return new RegistrationInsertResult(false, false);
        }
    }

    public async Task MarkTokenUsedAndActivateAsync(SecurityToken token, CancellationToken cancellationToken)
    {
        await using var tx = await db.Database.BeginTransactionAsync(cancellationToken);
        var now = DateTime.UtcNow;

        token.UsedAt ??= now;

        if (token.AccountEmail is not null)
        {
            token.AccountEmail.VerificationStatus = "verified";
            token.AccountEmail.VerifiedAt ??= now;
            token.AccountEmail.UpdatedAt = now;
        }

        token.Account.AccountStatus = "active";
        token.Account.ActivatedAt ??= now;
        token.Account.StatusChangedAt = now;

        await db.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
    }

    public async Task InvalidateAndCreateResendTokenAsync(SecurityToken? activeToken, Account account, AccountEmail accountEmail, SecurityToken newToken, CancellationToken cancellationToken)
    {
        await using var tx = await db.Database.BeginTransactionAsync(cancellationToken);
        if (activeToken is not null)
        {
            activeToken.InvalidatedAt = DateTime.UtcNow;
        }

        newToken.AccountId = account.AccountId;
        newToken.AccountEmailId = accountEmail.AccountEmailId;
        db.SecurityTokens.Add(newToken);

        await db.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
    }
}
