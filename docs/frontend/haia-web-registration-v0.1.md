# HAIA Web Registration v0.1

## 1. Executive Summary
Utworzono pierwszy frontendowy slice HAIA w `Toci.Haia.Web` jako lekkie SPA React+TypeScript+Vite. Implementacja pokrywa rejestrację email, legal docs, nickname availability, check-email, resend oraz ręczne verify tokenu.

## 2. Scope
W zakresie:
- routing i fundament aplikacji,
- integracja z HAIA API v1,
- formularz rejestracyjny,
- check-email i resend,
- verify-email po świadomym kliknięciu,
- testy behawioralne.

Poza zakresem:
- login/JWT,
- onboarding,
- feed/feature portalowe.

## 3. Technology Stack
- React
- TypeScript (strict)
- Vite
- React Router
- Axios
- TanStack Query
- React Hook Form
- Zod + @hookform/resolvers
- Vitest + React Testing Library + user-event + MSW

## 4. Project Structure
- `src/app` – router + providers
- `src/features/registration` – vertical slice
- `src/shared/api` – api client + error model
- `src/shared/components` – lekkie UI
- `src/shared/styles` – tokeny i globalne style
- `src/test` – setup i MSW

## 5. Routes
- `/` -> redirect `/register`
- `/register`
- `/register/check-email`
- `/verify-email`
- `*` -> not found

## 6. API Integration
Frontend komunikuje się z `/api/v1/...` przez pojedynczą instancję Axios. W dev używany jest proxy Vite do lokalnego backendu.

## 7. Axios Error Model
`ApiError` mapuje:
- status,
- code,
- title,
- detail,
- fieldErrors,
- correlationId,
- problem (w pamięci).

## 8. TypeScript DTO
Zdefiniowano jawne DTO tylko dla używanych endpointów:
- legal docs,
- nickname availability,
- register,
- verify,
- resend,
- accepted response,
- problem details.

## 9. Registration Flow
- RHF + Zod waliduje nickname/email/password,
- legal decisions tworzone dynamicznie z API,
- submit `POST /registration/email`,
- po `202` przejście do check-email,
- hasło czyszczone po sukcesie,
- brak zapisu hasła do storage/URL.

## 10. Nickname Availability
- debounce ~500 ms,
- brak wywołania dla niepoprawnych wartości,
- stany: loading/success/unavailable/error,
- finalny conflict mapowany z backendowego `nickname_unavailable`.

## 11. Legal Documents
- pobranie `GET /legal-documents/current?language=pl-PL`,
- obsługa loading/error/retry,
- empty list nie blokuje formularza,
- `legal_documents_changed` odświeża listę i czyści decyzje.

## 12. Check Email Flow
- neutralny komunikat bezpieczeństwa,
- resend przez `POST /registration/email/resend`,
- neutralny sukces po `202`.

## 13. Verify Email Security
- token czytany z query string,
- brak auto-verify przy wejściu,
- POST verify tylko po kliknięciu,
- token nie trafia do storage/logów,
- UI pokazuje wyłącznie maskowaną formę tokenu.

## 14. Problem Details Mapping
Mapowane kody:
- `validation_failed`
- `nickname_unavailable`
- `legal_decisions_incomplete`
- `legal_documents_changed`
- `invalid_or_expired_verification_token`
- `verification_delivery_unavailable`
- `database_unavailable`
- `unexpected_error`
- + 429/network timeout/no connection.

## 15. Accessibility
- semantyczny `main`,
- label/input,
- `aria-invalid`,
- statusy async przez `aria-live`,
- dekoracyjne emoji z `aria-hidden=true`,
- obsługa klawiatury i focus-visible.

## 16. Responsive Design
Układ mobile-first:
- mobile: jedna kolumna,
- desktop: branding + karta formularza,
- brak poziomego scrolla przy założonych breakpointach.

## 17. Test Strategy
Vitest+RTL+MSW bez realnych połączeń do API/DB. Testy pokrywają główne scenariusze tras i interakcji rejestracyjnych.

## 18. Security Review
- brak sekretów w `VITE_*`,
- brak password/token w storage,
- brak auto-verify,
- brak ujawniania duplicate email,
- brak surowych wyjątków/AxiosError w UI.

## 19. Build Results
Wyniki komend (`lint`, `build`, `test:run`) są raportowane po walidacji końcowej.

## 20. Known Limitations
- brak pełnego e2e browser automation,
- verify success prowadzi do placeholdera „Kalibracja już wkrótce”.

## 21. Deferred Areas
- logowanie i sesja,
- onboarding,
- rozszerzone profile.

## 22. Recommended Next Atomic Task
Dodać atom `Login + Session bootstrap` z zachowaniem neutralnych komunikatów bezpieczeństwa i pełnym contract-test dla ProblemDetails.
