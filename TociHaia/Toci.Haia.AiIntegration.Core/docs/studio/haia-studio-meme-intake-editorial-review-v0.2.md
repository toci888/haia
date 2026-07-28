# HAIA Studio Meme Intake — Editorial Review & Approval v0.2

## 1. Cel
Dodać workflow redakcyjny po AI evaluation dla intake mema: start review, approve/reject, notatka redakcyjna i podsumowanie review, bez publikacji i bez aktywacji produkcyjnej.

## 2. Zakres
- Backend Studio API:
  - `POST /api/v1/studio/meme-intakes/{intakeId}/editorial-review/start`
  - `POST /api/v1/studio/meme-intakes/{intakeId}/editorial-review/approve`
  - `POST /api/v1/studio/meme-intakes/{intakeId}/editorial-review/reject`
- Frontend Studio Web:
  - nowy ekran `MemeEditorialReviewPage`
  - nawigacja z `MemeIntakePage` do review
  - akcje approve/reject z payload `summary` i `editorialNote`

## 3. Model danych i statusy (bez DDL)
Wykorzystano istniejące pola persistence:
- `onboarding.candidate_classification`:
  - `classification_status`, `reviewed_by_account_id`, `reviewed_at`, `editorial_note`
- `onboarding.onboarding_candidate`:
  - `candidate_status`
- `onboarding.onboarding_candidate_version`:
  - `editorial_status`
- `studio.editorial_review`:
  - `review_status`, `reviewer_account_id`, `summary`

Brak migracji, brak zmian schematu, brak publish/activation.

## 4. Implementacja backend
### 4.1 Kontrakty
Dodano DTO:
- `StartEditorialReviewResponse`
- `MemeEditorialDecisionRequest`
- `MemeEditorialDecisionResponse`

### 4.2 Serwis i store
Rozszerzono:
- `IMemeIntakeService`
- `IMemeIntakeStore`

Nowe operacje:
- `StartEditorialReviewAsync`
- `ApproveAsync`
- `RejectAsync`

`EfMemeIntakeStore` realizuje:
- przejście do `editorial_review` na candidate/version,
- zapis decyzji `approved`/`rejected` na candidate/version/classification,
- zapis metadanych recenzenta i czasu review,
- update/insert `studio.editorial_review`.

### 4.3 Kontroler
`MemeIntakesController` rozszerzono o endpointy review (auth + CSRF + ProblemDetails zgodnie z istniejącym wzorcem).

## 5. Implementacja frontend
### 5.1 API client i kontrakty
Rozszerzono `memeIntakeApi.ts` i `memeIntakeContracts.ts` o operacje/typy review.

### 5.2 Strony i routing
Dodano:
- `MemeEditorialReviewPage.tsx`
- route: `/meme-intakes/:intakeId/review`

Rozszerzono `MemeIntakePage.tsx` o przycisk przejścia do review po zakończonej ewaluacji.

## 6. Testy
### Backend
- `Toci.Haia.Studio.Api.Tests`: PASS (33/33)
  - nowe testy unit dla start/approve/reject
  - testy integracyjne endpointów review + CSRF

### AI Integration
- `Toci.Haia.AiIntegration.Tests`: PASS (6/6)

### Frontend
- `npm run test:run`: PASS (19/19)
  - nowy test flow: przejście do review i approve draftu
- `npm run lint`: PASS
- `npm run build`: PASS

## 7. Build
- `Toci.Haia.Studio.Api`: build PASS
- `Toci.Haia.Studio.Api.Tests`: build PASS
- `Toci.Haia.AiIntegration.Tests`: build PASS

## 8. Ograniczenia i decyzje
- Workflow jest celowo draft-only.
- Brak mechanizmów publikacji/aktywacji w v0.2.
- Brak DDL i migracji.

## 9. Status końcowy
Atom `Editorial Review & Approval v0.2` został zaimplementowany i zweryfikowany testami/build/lint.
