# HAIA Studio API Foundation v0.1

## Cel iteracji
Pierwsza działająca iteracja osobnego REST API dla portalu redakcyjno-administracyjnego HAIA (`Toci.Haia.Studio.Api`) z osobnym projektem testowym (`Toci.Haia.Studio.Api.Tests`).

Zakres obejmuje wyłącznie fundament techniczny hosta i kontraktu HTTP. Brak operacji biznesowych administracyjnych.

## Architektura i granice
- Osobny proces i osobna aplikacja ASP.NET Core (`net10.0`) dla Studio.
- Brak referencji Studio API -> Public API (`Toci.Haia.Api`).
- Jedyna referencja domenowo-danych: `Toci.Haia.Database.Persistence` (DB-first source of truth).
- Host oparty o kontrolery (`AddControllers` + `MapControllers`).

## Konfiguracja hosta
W `Program.cs` dodano:
- `AddOpenApi()` i `MapOpenApi()` tylko w `Development`.
- `UseSwaggerUI()` tylko w `Development` pod `/swagger`.
- `AddProblemDetails()` + `AddExceptionHandler<GlobalExceptionHandler>()`.
- middleware `CorrelationIdMiddleware` przed mapowaniem endpointów.
- `AddRateLimiter()` z policy `studio-default`.
- `AddCors()` z policy `studio-cors` i konfigurowalnym `Cors:AllowedOrigins`.
- `/health/live` oraz `/health/ready` (`HaiaDbContext.Database.CanConnectAsync`).

## Błędy i ProblemDetails
Globalny handler zwraca RFC 7807 z:
- `code = database_unavailable` dla `NpgsqlException` (HTTP 503),
- `code = unexpected_error` dla pozostałych wyjątków (HTTP 500).

Brak stack trace i danych wrażliwych w odpowiedziach.

## Correlation ID
- Nagłówek wejściowy/wyjściowy: `X-Correlation-ID`.
- Walidacja wejścia: do 128 znaków, tylko `[a-zA-Z0-9._-]`.
- Dla niepoprawnych wartości generowany nowy GUID (`N`).
- Scope logowania zawiera correlation id; middleware loguje metodę, ścieżkę, status i czas.

## CORS
Konfiguracja w `appsettings.json`:
- `Cors:AllowedOrigins` (domyślnie `http://localhost:5273`).

Policy `studio-cors` dopuszcza:
- metody: `GET, POST, PUT, PATCH, DELETE, OPTIONS`,
- nagłówki: `Content-Type, X-Correlation-ID, Accept`.

## Rate limiting
Konfiguracja w `appsettings.json`:
- `RateLimiting:StudioDefaultPermitLimit` (domyślnie 60),
- `RateLimiting:WindowSeconds` (domyślnie 60).

Named policy: `studio-default` (fixed window, partycjonowanie po IP).

## Endpoint techniczny
`GET /api/v1/system/info`

Zwraca DTO `SystemInfoResponse`:
- `serviceName` = `HAIA Studio API`
- `apiVersion` = wersja assembly
- `status` = `ok`

Endpoint jest limitowany przez `studio-default` i dostępny w OpenAPI.

## Testy (`Toci.Haia.Studio.Api.Tests`)
Testy integracyjne (`WebApplicationFactory`) pokrywają:
- `/health/live` => 200,
- `/api/v1/system/info` => 200 + poprawny payload,
- obecność `X-Correlation-ID` i echo poprawnej wartości,
- obecność `/api/v1/system/info` w `/openapi/v1.json` (Development),
- brak ekspozycji `HaiaDbContext` i `Toci.Haia.Database.Persistence.Entities` w OpenAPI,
- `/swagger/index.html` dostępne w Development,
- `/swagger/index.html` i `/openapi/v1.json` niedostępne w Production,
- brak endpointu admin-biznesowego (`/api/v1/candidates` => 404).

## Konfiguracja lokalna
`ConnectionStrings:HaiaDatabase` lub `HAIA_DB_CONNECTION_STRING` są wymagane na starcie aplikacji.

## Studio Authentication and Authorization Foundation (zrealizowane)

W kolejnej iteracji dodano fundament bezpieczeństwa dla Studio API:

- `AddAuthentication().AddJwtBearer(...)` z konfiguracją `StudioAuth`.
- `AddAuthorization(...)` z policy `studio-access`.
- Policy `studio-access` wymaga:
  - uwierzytelnionego użytkownika,
  - roli `StudioAdmin` (konfigurowalnej przez `StudioAuth:RequiredRole`),
  - claimu `scope=studio.api` (konfigurowalnego przez `StudioAuth:RequiredScope`).

### Konfiguracja auth

Sekcja `StudioAuth` w `appsettings.json`:

- `Authority` (opcjonalne, dla zewnętrznego IdP),
- `Issuer`, `Audience`,
- `SigningKey` (placeholder; zalecane dostarczać przez env),
- `RequiredScope`, `RequiredRole`.

Obsługiwane źródła klucza podpisu:

- `HAIA_STUDIO_AUTH_SIGNING_KEY` (preferowane),
- `StudioAuth:SigningKey` (fallback).

Brak klucza skutkuje błędem startu aplikacji.

### Endpoint diagnostyczny auth

Dodano zabezpieczony endpoint:

- `GET /api/v1/studio/auth/status`

Wymaga policy `studio-access` i zwraca:

- `user`,
- `roles`,
- `scopes`,
- `status = authorized`.

### Testy auth/authz

Rozszerzono testy integracyjne o scenariusze:

- 401 dla braku tokenu,
- 401 dla niepoprawnego tokenu,
- 403 dla tokenu bez wymaganej roli,
- 200 dla tokenu z poprawną rolą i scope.

Walidacja po zmianach:

- `dotnet build` — sukces,
- `dotnet test Toci.Haia.Studio.Api.Tests` — 14/14 sukces.

## Następny atom
Studio Identity Provider Integration (np. Azure AD / Entra ID) i mapowanie ról/uprawnień na operacje administracyjne.
