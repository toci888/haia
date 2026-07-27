# HAIA API Foundation + Email Registration v0.1

## 1. Executive Summary
Wdrożono pierwszy atom API HAIA oparty o ASP.NET Core Minimal APIs (`/api/v1`) z integracją do `Toci.Haia.Database.Persistence` (DB-first). Zakres obejmuje foundation (OpenAPI, Problem Details, correlation ID, health checks, rate limiting) oraz flow rejestracji email (register/verify/resend), endpoint dostępności pseudonimu i odczyt aktualnych dokumentów prawnych.

## 2. Scope
W zakresie:
- API foundation (`Toci.Haia.Api`),
- test project (`Toci.Haia.Api.Tests`),
- legal documents current,
- nickname availability,
- email registration,
- email verification,
- resend verification.

Poza zakresem (deferred): login/JWT/social login/age eligibility/onboarding/reactions/Humor DNA/provider SMTP.

## 3. Endpoints
- `GET /health/live`
- `GET /health/ready`
- `GET /api/v1/legal-documents/current?language=pl-PL`
- `GET /api/v1/registration/nickname-availability?nickname=...`
- `POST /api/v1/registration/email`
- `POST /api/v1/registration/email/verify`
- `POST /api/v1/registration/email/resend`

## 4. Request/Response Contracts
HTTP DTO są niezależne od encji EF.

Kluczowe kontrakty:
- `RegisterEmailRequest`
- `VerifyEmailRequest`
- `ResendEmailRequest`
- `CurrentLegalDocumentsResponse`
- `NicknameAvailabilityResponse`
- `AcceptedResponse`

## 5. Registration Transaction
Rejestracja działa transakcyjnie w store EF:
1. utworzenie `Account` (`pending_email_verification`),
2. utworzenie `AccountEmail` (`pending`),
3. utworzenie `PasswordCredential` (hash),
4. utworzenie `PublicProfile` (nickname),
5. zapis `AccountDocumentAcceptance`,
6. zapis `SecurityToken` (hash tokenu),
7. commit.

## 6. Duplicate Email Privacy
Dla istniejącego emaila API zwraca ten sam rezultat `202 Accepted` (bez ujawniania istnienia konta).

## 7. Password Hashing
Hasło hashowane przez `PasswordHasher<Account>` (ASP.NET Core Identity). Brak własnej kryptografii i brak plaintext password w bazie.

## 8. Verification Token Security
- plaintext token: `RandomNumberGenerator` (32 bajty), Base64Url,
- w bazie: tylko SHA-256 (`token_hash`),
- brak tokenu w odpowiedziach HTTP.

## 9. Legal Document Validation
Flow rejestracji waliduje wymagane aktywne wersje dokumentów dla języka i zgodność `legalDecisions`.

Mapowanie błędów:
- `legal_documents_changed` -> 409,
- `legal_decisions_incomplete` -> 422.

## 10. Nickname Race Handling
Pre-check dostępności + obsługa race condition przez rozpoznanie unikalnego constraintu `ux_public_profile_nickname_active` i zwrot 409 `nickname_unavailable`.

## 11. Email Sender Boundary
- kontrakt: `IEmailVerificationSender`,
- implementacja dev/test: `InMemoryEmailVerificationSender`,
- brak SMTP/providera zewnętrznego w tym atomie.

## 12. Problem Details
Globalny `IExceptionHandler` mapuje wyjątki do RFC 7807 z kodami domenowymi w `extensions.code`.

## 13. Correlation ID
Middleware obsługuje `X-Correlation-ID`, generuje gdy brak i zwraca w response header.

## 14. Rate Limiting
Wbudowany ASP.NET Core Rate Limiting, nazwane polityki:
- `registration`
- `verification`
- `resend`
- `nickname`

## 15. Persistence Integration
API korzysta z:
- `AddHaiaDatabasePersistence`,
- feature-specific store `IEmailRegistrationStore` / `EfEmailRegistrationStore`.

Brak generycznego repository/UoW.

## 16. Test Strategy
- Unit tests: normalizacja, token security, options basics.
- Integration smoke: liveness endpoint przez `WebApplicationFactory`.

## 17. Build and Test Results
Wyniki należy aktualizować po każdym uruchomieniu `dotnet restore`, `dotnet build`, `dotnet test`.

## 18. Security Review
- brak eksportu encji EF przez HTTP,
- brak endpointu ujawniającego token,
- brak migracji EF i `EnsureCreated`/`Migrate`.

## 19. Deferred Areas
- login/JWT,
- social login,
- real email provider,
- pełny zestaw integration tests z fake store,
- dodatkowe anty-abuse i limity dzienne.

## 20. Known Limitations
- integration tests nie pokrywają jeszcze pełnego matrixu endpointów,
- brak providera produkcyjnego do wysyłki email,
- część walidacji legal docs opiera się na obecności danych referencyjnych w DB.

## 21. Recommended Next Atomic Task
Dodać pełne testy integracyjne registration/verify/resend z fake store i fake sender (bez mutacji głównej bazy) + twardą weryfikację contractów Problem Details.
