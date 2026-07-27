# HAIA Studio Web

Pierwsza iteracja portalu redakcyjno-administracyjnego HAIA. Aplikacja udostępnia shell Studio oraz ekran `/system` pokazujący status Studio API.

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

- `GET /api/v1/system/info`
- `GET /health/live`
- `GET /health/ready`

Vite proxy przekazuje lokalne wywołania `/api` i `/health` do `VITE_STUDIO_API_PROXY_TARGET`.

## Routy

- `/` -> redirect do `/system`
- `/system` -> ekran statusu systemu HAIA Studio
- `*` -> fallback „Nie znaleziono strony”

## Komendy

- `npm run dev`
- `npm run lint`
- `npm run build`
- `npm run test`
- `npm run test:run`

## Struktura (skrót)

- `src/app` – router i providery
- `src/layout` – shell Studio (header + main)
- `src/features/system-status` – API, kontrakty i ekran `/system`
- `src/shared/api` – axios client, Problem Details, ApiError
- `src/shared/styles` – globalne style i tokeny
- `src/test` – setup Vitest + MSW

## Aktualne ograniczenie

W tej iteracji brak logowania Studio i brak obsługi JWT po stronie klienta. Integracja z dostawcą tożsamości jest kolejnym atomem.
