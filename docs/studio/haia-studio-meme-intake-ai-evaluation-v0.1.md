# HAIA Studio — Meme Intake + AI Evaluation v0.1

## 1. Wynik capability gates
- Persistence alignment po refreshu DB-first: **OK**.
- Model `haia_humor_classification` v1 (`status=active`): potwierdzony read-only smoke.
- Słowniki osi/wartości/sensitivity i tabele klasyfikacji: dostępne przez EF.

## 2. Zmodyfikowane projekty
- `Toci.Haia.Studio.Web`
- `Toci.Haia.Studio.Api`
- `Toci.Haia.Studio.Api.Tests`
- `Toci.Haia.AiIntegration.Core`
- `Toci.Haia.AiIntegration.OpenAI`
- `Toci.Haia.AiIntegration.Tests`

## 3. Route i UX formularza
- Dodano route: `/meme-intakes/new`.
- Dodano pozycję nawigacji: **Dodaj mem**.
- Strona ma 3 fazy: wybór obrazu, podgląd/wysłanie, wynik analizy.

## 4. Przepływ Ctrl+V / file picker
- Obsługa Ctrl+V dla obrazów poza polami tekstowymi.
- Obsługa `<input type="file">` i przycisku „Wybierz z dysku”.
- Dla wielu obrazów ze schowka: używany pierwszy + komunikat informacyjny.
- Tekst ze schowka jest ignorowany.

## 5. Endpointy API
- `POST /api/v1/studio/meme-intakes`
- `POST /api/v1/studio/meme-intakes/{intakeId}/finalize-upload`
- `POST /api/v1/studio/meme-intakes/{intakeId}/evaluate`
- `GET /api/v1/studio/meme-intakes/{intakeId}`

## 6. Model presigned upload i finalize
- Create zwraca intent uploadu (private object key, presigned PUT URL, wymagane nagłówki, expiry).
- Upload wykonywany bezpośrednio przeglądarka -> private R2 (`PUT`).
- Finalize waliduje obecność obiektu, rozmiar i sygnaturę typu pliku.
- Finalize jest idempotentne dla statusu `uploaded`/`awaiting_review`.

## 7. Sposób walidacji pliku
- Frontend: walidacja UX (mime + max size).
- Backend: weryfikacja realnego typu po sygnaturze nagłówka (JPEG/PNG/WEBP), limit rozmiaru, odrzucenie nieobsługiwanych formatów.

## 8. Kontrakt AI
- Dodano typowaną operację: `EvaluateStudioMemeImage`.
- Dodano typed request/response + request validator + response validator.
- Walidowane są: słownikowe keye, zakresy 0..1, safety 0..3, dokładnie 12 reakcji, dokładnie 6 visible, obowiązkowa druga puenta.

## 9. Wykorzystanie słowników klasyfikacji
- Taksonomia do AI i mapowania pochodzi z bazy (aktywny model + active values + sensitivity + dryness + mechanisms).
- Nie akceptuje się dowolnych, nieznanych kluczy spoza słowników.

## 10. Sposób zapisu klasyfikacji
- Zapis do `onboarding.candidate_classification` jako `source_type='ai'`, `classification_status='awaiting_review'`.
- Zapis relacyjny do:
  - `candidate_classification_value`
  - `candidate_classification_measure`
  - `candidate_classification_sensitivity`
- Utrzymany lineage do `ai.ai_operation_execution`.

## 11. Sposób zapisu safety i Sucharka
- Safety zapisuje się relacyjnie przez `candidate_classification_sensitivity` + `sensitivity_category`.
- Predicted dryness mapowany na aktywny `dryness_scale_level` + `predicted_dryness_confidence`.

## 12. Sposób zapisu reakcji i drugich puent
- Draft reaction pack/version + 12 pozycji.
- Dla każdej reakcji zapisywana druga puenta (`second_punchline_text`).
- Mechanizmy reakcji mapowane po stabilnych keyach.

## 13. Statusy draftu
- `creating_draft` -> `uploaded` -> `awaiting_review` / `ready_for_review`.
- Brak auto-publish i brak aktywacji produkcyjnej.

## 14. Liczba i wynik testów backendu
- `Toci.Haia.Studio.Api.Tests`: **28/28 PASS**.
- Dodane testy jednostkowe i integracyjne dla feature Meme Intake (auth/csrf, walidacja wejścia, idempotencja finalize, draft persistence, brak wycieku sekretów).

## 15. Liczba i wynik testów AI
- `Toci.Haia.AiIntegration.Tests`: **6/6 PASS**.
- Pokryto walidację request/response, nieznane keye, zakresy, liczność reakcji i brak drugiej puenty.

## 16. Liczba i wynik testów frontendu
- Studio Web: **18/18 PASS**.
- Pokryto: Ctrl+V, file picker, ignorowanie text paste, preview/remove, kolejność create-upload-finalize-evaluate, błędy upload/evaluate, render klasyfikacji/safety/sucharek/12 reakcji.

## 17. Wyniki build i lint
- `dotnet build Toci.Haia.Studio.Api`: **PASS**
- `dotnet build Toci.Haia.AiIntegration.Core`: **PASS**
- `dotnet build Toci.Haia.AiIntegration.OpenAI`: **PASS**
- `npm run lint` (Studio Web): **PASS**
- `npm run build` (Studio Web): **PASS**

## 18. Wynik runtime R2
- **NOT RUNTIME VERIFIED** (brak bezpiecznej, kompletnej konfiguracji runtime w tym przebiegu).

## 19. Wynik runtime AI
- **NOT RUNTIME VERIFIED** (brak bezpiecznej, kompletnej konfiguracji runtime w tym przebiegu).

## 20. Elementy NOT VERIFIED
- Realne wywołanie private R2 w środowisku runtime z sekretami.
- Realne wywołanie providera AI w środowisku runtime z sekretami.

## 21. Znane ograniczenia
- Adapter OpenAI jest provider-agnostic na poziomie kontraktu, ale runtime provider call oznaczony jako nieweryfikowany bez sekretów.
- Brak manualnej edycji/approve/publish (poza zakresem atomu).

## 22. Rekomendowany następny atom
- **Studio Meme Intake — Editorial Review & Approval v0.2**
  1. ręczna edycja wyniku AI,
  2. workflow review/approve,
  3. dopiero potem controlled publish/activation.
