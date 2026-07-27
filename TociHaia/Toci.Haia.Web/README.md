# Toci.Haia.Web

Frontend MVP dla atomu rejestracji HAIA (`React + TypeScript + Vite`).

## Wymagania lokalne
- Node.js (zgodny z aktualnym Vite)
- npm
- Lokalnie uruchomione HAIA API (domyślnie `http://localhost:5035`)

## Instalacja
```bash
npm install
```

## Uruchomienie
```bash
npm run dev
```
Domyślny adres frontendu: `http://localhost:5173`.

## Konfiguracja `.env`
Skopiuj `.env.example` do `.env` i dostosuj:

- `VITE_API_BASE_URL=`
  - puste w local dev (requesty względne),
  - może wskazywać pełny adres API na wdrożeniu.
- `VITE_API_PROXY_TARGET=http://localhost:5035`
  - używane wyłącznie przez Vite dev server.

## Proxy Vite
Dev proxy przekazuje:
- `/api`
- `/health`
- `/openapi`

do `VITE_API_PROXY_TARGET`.

## Build
```bash
npm run build
```

## Lint
```bash
npm run lint
```

## Testy
```bash
npm run test
npm run test:run
```

## Struktura projektu
- `src/app` — router i providers
- `src/features/registration` — vertical slice rejestracji
- `src/shared/api` — axios client + model błędów
- `src/shared/components` — lekkie komponenty współdzielone
- `src/shared/styles` — tokeny i style globalne
- `src/test` — test setup + MSW

## Dostępne route
- `/` -> redirect do `/register`
- `/register`
- `/register/check-email`
- `/verify-email`
- `*` -> fallback „Nie znaleziono strony”

## Znane ograniczenia
- Brak logowania/JWT/onboardingu (poza zakresem atomu).
- Brak produkcyjnego providera email.
- Brak wizualnego e2e w przeglądarce w tym przebiegu CLI.

## Bezpieczeństwo zmiennych
Wszystkie `VITE_*` są dostępne po stronie przeglądarki. Nie wolno przechowywać w nich sekretów.
