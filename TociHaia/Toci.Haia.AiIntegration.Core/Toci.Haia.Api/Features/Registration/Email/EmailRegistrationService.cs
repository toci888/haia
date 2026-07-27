using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Toci.Haia.Api.Common.Correlation;
using Toci.Haia.Api.Common.Errors;
using Toci.Haia.Api.Common.Security;
using Toci.Haia.Api.Features.LegalDocuments;
using Toci.Haia.Api.Features.Registration.Email.Contracts;
using Toci.Haia.Api.Infrastructure.EmailVerification;
using Toci.Haia.Database.Persistence.Entities;

namespace Toci.Haia.Api.Features.Registration.Email;

public sealed class EmailRegistrationService(
    IEmailRegistrationStore store,
    ILegalDocumentsService legalDocumentsService,
    IEmailVerificationSender? sender,
    IOptions<EmailRegistrationOptions> options,
    TimeProvider timeProvider,
    ILogger<EmailRegistrationService> logger) : IEmailRegistrationService
{
    private const string PendingAccountStatus = "pending_email_verification";
    private const string ActiveAccountStatus = "active";
    private const string PendingEmailStatus = "pending";
    private const string VerifiedEmailStatus = "verified";
    private const string DraftPrivateVisibility = "draft_private";
    private const string EmailVerificationPurpose = "email_verification";
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public async Task<RegistrationResult> RegisterAsync(RegisterEmailRequest request, string correlationId, CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeAndValidateEmail(request.Email);
        ValidatePassword(request.Password);
        ValidateNickname(request.Nickname);

        var language = NormalizeAndValidateLanguage(request.Language);

        var existing = await store.ExistsActiveEmailAsync(normalizedEmail, cancellationToken);
        if (existing)
        {
            logger.LogInformation("Registration accepted with generic duplicate-email behavior for {Email}", EmailMasking.Mask(normalizedEmail));
            return RegistrationResult.Accepted;
        }

        var requiredDocs = await legalDocumentsService.GetRequiredCurrentForRegistrationAsync(language, cancellationToken);
        var legalDecisionsValidation = ValidateLegalDecisions(requiredDocs, request.LegalDecisions);
        if (legalDecisionsValidation == RegistrationResult.LegalDocumentsChanged || legalDecisionsValidation == RegistrationResult.LegalDecisionsIncomplete)
        {
            return legalDecisionsValidation;
        }

        if (await store.ExistsActiveNicknameAsync(EmailRegistrationNormalization.NormalizeNickname(request.Nickname), cancellationToken))
        {
            return RegistrationResult.NicknameUnavailable;
        }

        if (sender is null)
        {
            return RegistrationResult.VerificationDeliveryUnavailable;
        }

        var now = timeProvider.GetUtcNow();
        var token = VerificationTokenGenerator.Generate();
        var tokenHash = TokenHasher.ComputeSha256(token);
        var account = new Account();
        var passwordHash = _passwordHasher.HashPassword(account, request.Password);

        var insertResult = await store.InsertRegistrationAsync(
            new RegistrationInsertData(
                request.Email.Trim(),
                normalizedEmail,
                request.Nickname.Trim(),
                EmailRegistrationNormalization.NormalizeNickname(request.Nickname),
                passwordHash,
                now,
                correlationId,
                PendingAccountStatus,
                PendingEmailStatus,
                DraftPrivateVisibility,
                (request.LegalDecisions ?? [])
                    .Select(d => new LegalDecisionPersisted(d.DocumentVersionId, d.Action.Trim().ToLowerInvariant(), now))
                    .ToList(),
                new SecurityToken
                {
                    TokenPurpose = EmailVerificationPurpose,
                    TokenHash = tokenHash,
                    CreatedAt = now.UtcDateTime,
                    ExpiresAt = now.AddHours(options.Value.TokenLifetimeHours).UtcDateTime,
                    ResendSequence = 0,
                    RequestCorrelationId = correlationId
                }),
            cancellationToken);

        if (insertResult.NicknameConflict)
        {
            return RegistrationResult.NicknameUnavailable;
        }

        if (!insertResult.Success)
        {
            throw new DomainException(HttpStatusCode.Conflict, ErrorCodes.ValidationFailed, "Registration cannot be completed.");
        }

        await sender.SendAsync(
            new EmailVerificationMessage(
                request.Email.Trim(),
                token,
                now.AddHours(options.Value.TokenLifetimeHours),
                language,
                correlationId),
            cancellationToken);

        return RegistrationResult.Accepted;
    }

    public async Task VerifyAsync(VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            throw new DomainException(HttpStatusCode.BadRequest, ErrorCodes.InvalidOrExpiredVerificationToken, "Invalid verification token.");
        }

        var tokenHash = TokenHasher.ComputeSha256(request.Token.Trim());
        var token = await store.FindTokenByHashAsync(tokenHash, EmailVerificationPurpose, cancellationToken);
        if (token is null)
        {
            throw new DomainException(HttpStatusCode.BadRequest, ErrorCodes.InvalidOrExpiredVerificationToken, "Invalid verification token.");
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;
        if (token.InvalidatedAt is not null || token.ExpiresAt <= now)
        {
            throw new DomainException(HttpStatusCode.BadRequest, ErrorCodes.InvalidOrExpiredVerificationToken, "Invalid verification token.");
        }

        if (token.UsedAt is not null || token.Account.AccountStatus == ActiveAccountStatus)
        {
            if (token.Account.AccountStatus == ActiveAccountStatus)
            {
                return;
            }

            throw new DomainException(HttpStatusCode.BadRequest, ErrorCodes.InvalidOrExpiredVerificationToken, "Invalid verification token.");
        }

        if (token.AccountEmail is null)
        {
            throw new DomainException(HttpStatusCode.BadRequest, ErrorCodes.InvalidOrExpiredVerificationToken, "Invalid verification token.");
        }

        if (token.Account.AccountStatus != PendingAccountStatus)
        {
            throw new DomainException(HttpStatusCode.BadRequest, ErrorCodes.InvalidOrExpiredVerificationToken, "Invalid verification token.");
        }

        if (token.AccountEmail.VerificationStatus == VerifiedEmailStatus)
        {
            return;
        }

        await store.MarkTokenUsedAndActivateAsync(token, cancellationToken);
    }

    public async Task ResendAsync(ResendEmailRequest request, string correlationId, CancellationToken cancellationToken)
    {
        var normalizedEmail = NormalizeAndValidateEmail(request.Email);
        var account = await store.FindAccountByEmailAsync(normalizedEmail, cancellationToken);
        if (account is null || account.AccountStatus != PendingAccountStatus)
        {
            return;
        }

        if (sender is null)
        {
            return;
        }

        var accountEmail = await store.FindPrimaryActiveEmailAsync(account.AccountId, cancellationToken);
        if (accountEmail is null || accountEmail.VerificationStatus == VerifiedEmailStatus)
        {
            return;
        }

        var activeToken = await store.FindLatestActiveTokenAsync(account.AccountId, EmailVerificationPurpose, cancellationToken);
        var now = timeProvider.GetUtcNow();

        if (activeToken is not null)
        {
            var cooldown = TimeSpan.FromSeconds(options.Value.ResendCooldownSeconds);
            if (activeToken.CreatedAt.Add(cooldown) > now.UtcDateTime)
            {
                return;
            }
        }

        var plaintextToken = VerificationTokenGenerator.Generate();
        var newToken = new SecurityToken
        {
            TokenPurpose = EmailVerificationPurpose,
            TokenHash = TokenHasher.ComputeSha256(plaintextToken),
            CreatedAt = now.UtcDateTime,
            ExpiresAt = now.AddHours(options.Value.TokenLifetimeHours).UtcDateTime,
            ResendSequence = (activeToken?.ResendSequence ?? 0) + 1,
            RequestCorrelationId = correlationId
        };

        await store.InvalidateAndCreateResendTokenAsync(activeToken, account, accountEmail, newToken, cancellationToken);

        var language = NormalizeAndValidateLanguage(request.Language);
        await sender.SendAsync(
            new EmailVerificationMessage(request.Email.Trim(), plaintextToken, now.AddHours(options.Value.TokenLifetimeHours), language, correlationId),
            cancellationToken);
    }

    private string NormalizeAndValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Email is required.");
        }

        var normalized = EmailRegistrationNormalization.NormalizeEmail(email);
        if (!EmailRegistrationNormalization.IsValidEmail(normalized))
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Email format is invalid.");
        }

        return normalized;
    }

    private void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Password is required.");
        }

        if (password.Length < options.Value.MinimumPasswordLength || password.Length > options.Value.MaximumPasswordLength)
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Password length is invalid.");
        }
    }

    private void ValidateNickname(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Nickname is required.");
        }

        var trimmed = nickname.Trim();
        if (trimmed.Length < 3 || trimmed.Length > 40)
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Nickname length is invalid.");
        }
    }

    private string NormalizeAndValidateLanguage(string? language)
    {
        var normalized = EmailRegistrationNormalization.NormalizeLanguage(language, options.Value);
        if (!EmailRegistrationNormalization.IsSupportedLanguage(normalized))
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Language is invalid.");
        }

        return normalized;
    }

    private static RegistrationResult ValidateLegalDecisions(
        IReadOnlyList<Features.LegalDocuments.Contracts.CurrentLegalDocumentItem> requiredDocs,
        IReadOnlyList<LegalDecisionRequest>? legalDecisions)
    {
        if (requiredDocs.Count == 0)
        {
            return RegistrationResult.Accepted;
        }

        var incoming = (legalDecisions ?? []).ToList();
        var duplicate = incoming
            .GroupBy(x => x.DocumentVersionId)
            .Any(g => g.Count() > 1);
        if (duplicate)
        {
            return RegistrationResult.LegalDecisionsIncomplete;
        }

        var requiredIds = requiredDocs.Select(d => d.DocumentVersionId).ToHashSet();
        var incomingIds = incoming.Select(d => d.DocumentVersionId).ToHashSet();

        if (incoming.Any(x => !string.Equals(x.Action, "accepted", StringComparison.OrdinalIgnoreCase)))
        {
            return RegistrationResult.LegalDecisionsIncomplete;
        }

        if (incoming.Any(x => !requiredIds.Contains(x.DocumentVersionId)))
        {
            return RegistrationResult.LegalDocumentsChanged;
        }

        if (!requiredIds.SetEquals(incomingIds))
        {
            return RegistrationResult.LegalDecisionsIncomplete;
        }

        return RegistrationResult.Accepted;
    }
}
