# HAIA Studio — Meme Intake AI Evaluation Retry Repair v0.1

## 1. Przyczyna błędu
Retry w UI uruchamiał tę samą orkiestrację co start procesu, więc po błędzie AI wykonywał ponownie `create -> upload -> finalize -> evaluate` zamiast `evaluate(existingIntakeId)`.

## 2. Stara funkcja uruchamiająca cały flow
`Toci.Haia.Studio.Web/src/features/meme-intakes/pages/MemeIntakePage.tsx` — funkcja `onRunFlow()` (stan przed naprawą) zawsze wykonywała pełny flow.

## 3. Nowa separacja initial flow i retry
W `MemeIntakePage.tsx` rozdzielono:
- `runInitialFlow(file)` = create + upload + finalize + evaluate,
- `retryEvaluationOnly(intakeId)` = tylko evaluate dla istniejącego intake.

Retry jest wywoływany tylko, gdy istnieje `createdIntakeId` i stan to `failed`.

## 4. Endpoint używany przez retry
`POST /api/v1/studio/meme-intakes/{intakeId}/evaluate`

## 5. Statusy pozwalające na retry
Backend (`MemeIntakeService.EvaluateAsync`) dopuszcza evaluate/retry gdy intake jest:
- `uploaded`,
- `awaiting_ai_analysis`.

## 6. Statusy blokujące retry
- `draft`, `creating_draft` -> `meme_intake_not_uploaded`.
- inne niedozwolone statusy -> `meme_evaluation_retry_not_allowed`.
- aktywna analiza dla intake -> `409 meme_evaluation_already_running`.

## 7. Zachowanie po page reload
Frontend zapisuje ostatni `intakeId` w `sessionStorage` i próbuje odtworzyć intake przez `GET /meme-intakes/{intakeId}`. Jeśli brak wyniku evaluate, umożliwia retry evaluate dla tego samego intake.

## 8. Ochrona przed podwójnym requestem
- Frontend: `retryInFlightRef` + disabled podczas `evaluating`.
- Backend: pre-check stanu execution (`HasInProgressExecution`) i `409 Conflict`.

## 9. Tworzenie kolejnych AI executions
`EvaluateAsync` nadal generuje nowy `aiExecutionId = Guid.NewGuid()` dla każdej próby i zapisuje ją przez istniejące metody store (`SaveEvaluationAsync`/`SaveFailedEvaluationAsync`).

## 10. Potwierdzenie tego samego intake/candidate/version/media
Retry nie wykonuje create/finalize/upload, więc pracuje na tym samym `intakeId` i istniejącym candidate/version/media.

## 11. Potwierdzenie braku uploadu podczas retry
W testach frontendu dodano asercje, że po kliknięciu retry nie rośnie licznik `create/upload/finalize`; rośnie tylko `evaluate`.

## 12. Potwierdzenie braku finalize podczas retry
W testach frontendu licznik `finalize` pozostaje bez zmian podczas retry.

## 13. Wyniki testów backendu
Dodano/rozszerzono testy jednostkowe `MemeIntakeServiceTests`:
- reject when evaluation already running,
- return existing result when already completed,
- retry after failure uses same intake without upload flow.

## 14. Wyniki testów frontendu
Dodano testy `MemeIntakePage.test.tsx` (MSW):
- initial flow: create->upload->finalize->evaluate,
- failure pokazuje retry,
- retry wywołuje tylko evaluate,
- podwójne kliknięcie retry nie uruchamia równoległych evaluate,
- reload odtwarza intake i pozwala na retry evaluate.

## 15. Wyniki build i lint
NOT VERIFIED — brak uruchomienia komend w tym przebiegu.

## 16. Elementy NOT VERIFIED
- `dotnet build` Studio API,
- `dotnet test` Studio API Tests,
- `npm run lint` Studio Web,
- `npm run build` Studio Web,
- `npm run test:run` Studio Web,
- `git diff --check`.

## 17. Ograniczenia concurrency
Bez zmiany DDL blokada równoległości opiera się na pre-check stanu execution; to minimalna ochrona aplikacyjna (okno race nadal teoretycznie istnieje między check a zapisem).

## 18. Znane problemy częściowego zapisu AI
Nie wykonano pełnej atomizacji całego pipeline zapisu klasyfikacji/reakcji. Historyczne failed outputs pozostają, ale ten atom nie przebudowuje transakcyjności end-to-end.

## 19. Rekomendowany następny atom
Wzmocnienie idempotencji/concurrency evaluate na poziomie DB (np. dedykowany mechanizm lock/claim attempt bez DDL-change jeśli możliwe, albo planowany DDL atom), plus ujednolicenie pól `GetMemeIntakeResponse` o pełny status ostatniej próby retry.
