# HAIA Studio API

REST API dla portalu redakcyjno-administracyjnego HAIA.

## Technologia

- .NET 10
- ASP.NET Core Web API
- Problem Details (RFC 7807)
- Cookie authentication (HttpOnly)
- Antiforgery (CSRF)

## Uruchomienie

```bash
dotnet run --project ./Toci.Haia.Studio.Api/Toci.Haia.Studio.Api.csproj
```

## Konfiguracja

Najważniejsze sekcje `appsettings.json`:

- `ConnectionStrings:HaiaDatabase`
- `Cors:AllowedOrigins`
- `RateLimiting`
- `StudioCookieAuth`
- `StudioPasswordPolicy`

## Bootstrap pierwszego administratora

Tworzenie/aktualizacja pierwszego konta administratora odbywa się wyłącznie przez CLI:

```bash
dotnet run --project ./Toci.Haia.Studio.Api/Toci.Haia.Studio.Api.csproj -- --bootstrap-studio-admin
```

Proces jest idempotentny i nie wystawia endpointu HTTP do rejestracji administratora.

## Endpointy auth

- `GET /api/v1/studio/auth/csrf`
- `POST /api/v1/studio/auth/login`
- `GET /api/v1/studio/auth/status` (wymaga sesji)
- `POST /api/v1/studio/auth/logout` (wymaga sesji + CSRF)

## Endpointy techniczne

- `GET /api/v1/system/info`
- `GET /health/live`
- `GET /health/ready`

## Kontrakt bezpieczeństwa

- brak JWT po stronie klienta
- sesja backendowa oparta o cookie HttpOnly
- CORS ograniczony do `AllowedOrigins` + `AllowCredentials()`
- walidacja sesji podczas żądań względem aktualnego stanu konta i ról w bazie
