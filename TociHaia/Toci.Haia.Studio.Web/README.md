# HAIA Studio Web

Portal redakcyjno-administracyjny HAIA (frontend React + TypeScript + Vite) z lokalnym logowaniem administracyjnym opartym o sesję cookie HttpOnly oraz CSRF.

## Wymagania

- Node.js (lokalnie zweryfikowano v25.7.0)
- npm (lokalnie zweryfikowano v11.0.0)
- uruchomione `Toci.Haia.Studio.Api`

## Instalacja

```bash
npm install
```

## Konfiguracja środowiska

Skopiuj `.env.example` do `.env` i dostosuj wartości:

```bash
cp .env.example .env
```

Zmienne:

- `VITE_STUDIO_API_BASE_URL=`
  - pusta wartość oznacza relative URLs (`/api/...`, `/health/...`)
- `VITE_STUDIO_API_PROXY_TARGET=http://localhost:5065`
  - używana tylko przez dev server Vite do proxy

Zmienne `VITE_*` nie służą do sekretów.

## Uruchomienie developerskie

```bash
npm run dev
```

Domyślny adres frontendu: `http://localhost:5173` (lub kolejny wolny port, jeśli zajęty).

## Studio API (wymagane lokalnie)

Uruchom `Toci.Haia.Studio.Api` (Development). Frontend zakłada dostępność endpointów:

- `GET /api/v1/studio/auth/status`
- `GET /api/v1/studio/auth/csrf`
- `POST /api/v1/studio/auth/login`
- `POST /api/v1/studio/auth/logout`
- `GET /api/v1/system/info`
- `GET /health/live`
- `GET /health/ready`

Vite proxy przekazuje lokalne wywołania `/api` i `/health` do `VITE_STUDIO_API_PROXY_TARGET`.

## Routy

- `/` -> redirect do `/system`
- `/login` -> ekran logowania administratora
- `/system` -> ekran statusu systemu HAIA Studio (wymaga aktywnej sesji)
- `*` -> fallback „Nie znaleziono strony”

## Komendy

- `npm run dev`
- `npm run lint`
- `npm run build`
- `npm run test`
- `npm run test:run`

## Struktura (skrót)

- `src/app` – router i providery
- `src/features/auth` – logowanie, wylogowanie, guard sesji, kontrakty auth
- `src/layout` – shell Studio (header + main)
- `src/features/system-status` – API, kontrakty i ekran `/system`
- `src/shared/api` – axios client, Problem Details, ApiError
- `src/shared/styles` – globalne style i tokeny
- `src/test` – setup Vitest + MSW

## Model uwierzytelniania

- brak JWT w localStorage/sessionStorage
- sesja utrzymywana przez cookie HttpOnly po stronie backendu
- operacje `login` i `logout` realizowane z tokenem CSRF (`X-CSRF-TOKEN`)
