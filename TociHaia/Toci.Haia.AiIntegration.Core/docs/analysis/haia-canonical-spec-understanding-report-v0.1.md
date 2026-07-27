# HAIA Canonical Spec — raport zrozumienia v0.1

## 1. Cel raportu
Ten raport opisuje, jak zrozumiałem dokument `HAIA_CANONICAL_PRODUCT_AND_TECHNICAL_SPEC_v0.1.txt` jako obowiązujące źródło prawdy produktowo-technicznej dla HAIA.

## 2. Status źródła i zasady interpretacji
**Zrozumienie:** dokument jest „canonical working specification” i ma pierwszeństwo nad wcześniejszymi raportami, o ile nie ma nowszej jawnej decyzji właściciela produktu. 

**Konsekwencja praktyczna:**
- decyzje oznaczone `[CONFIRMED]` traktuję jako obowiązujące,
- `[RECOMMENDED]` jako preferowany kierunek projektowy,
- `[OPEN]` jako obszar wymagający decyzji, bez zgadywania,
- `[DEFERRED]` jako świadomie odłożony scope,
- `[PARTIALLY IMPLEMENTED]` jako fundament istniejący, ale nie domknięty.

## 3. Jak rozumiem produkt HAIA
### 3.1. Rdzeń produktu
HAIA to **humor-core platforma mobile-first**, nie „social portal z kategorią humor”. Humor jest językiem interfejsu, interakcji, personalizacji i twórczości.

### 3.2. Najważniejsze ograniczenia
- AI nie może być wywoływane per scroll.
- AI nie publikuje automatycznie.
- Wiek i zawód to weak priory, nie cechy potwierdzonego Humor DNA.
- Human-in-the-loop pozostaje obowiązkowy dla aktywacji treści i polityk.

### 3.3. Aktualny priorytet
Bieżący priorytet to: **HAIA User Entry and Initial Humor Onboarding** z pełnym przepływem od konta do Initial Humor DNA Snapshot i wejścia do portalu.

## 4. Jak rozumiem kluczowe domeny funkcjonalne
## 4.1. Rejestracja i tożsamość
- Jedno konto może mieć wiele metod logowania (email/Google/Facebook).
- Brak blind merge po samym e-mailu.
- Pseudonim i dokumenty prawne są częścią wejścia.

## 4.2. Age/Profession context
- Rozdział: **age eligibility** (compliance) vs **optional Age Context** (personalizacja).
- Profession context jest opcjonalny.
- Priory kohortowe mają maleć po pojawieniu się realnych sygnałów zachowania.

## 4.3. Onboarding adaptacyjny
- Około 10 kart z szerokiej biblioteki kandydatów.
- Każda karta pełni jednocześnie funkcję doświadczeniową i pomiarową.
- Wymagany explainability trail dla decyzji selekcji.
- Recovery mode jest integralną częścią flow.
- Sesja ma wspierać resume cross-device i idempotencję.

## 4.4. Humor DNA
- Warstwy są rozdzielone: prior → working profile → observed evidence → snapshot → long-term profile.
- Snapshot musi mieć confidence i odróżniać confirmed vs weak hypotheses.
- Prior bez observed evidence nie potwierdza DNA.

## 4.5. Shared Reaction Engine
To najważniejszy obszar nowej semantyki:
- reakcja = artefakt humorystyczny, nie enum emocji,
- model 12/6 (konfigurowalny),
- **wielokrotny wybór reakcji** przez użytkownika,
- oddzielne ślady: exposure, selection, second punchline view, feedback,
- reaction rescue nie może wynikać z samego kliknięcia,
- onboarding i feed docelowo mają dzielić wspólny fundament silnika reakcji.

## 4.6. Sucharek
- 6 poziomów suchości, wersjonowanych.
- Jeden aktywny poziom na użytkownika/treść.
- Relacja Sucharek vs custom reactions: formalnie OPEN, ale rekomendacja MVP: **exclusivity** z możliwością przyszłego przełączenia na coexistence.

## 4.7. Power User / Power Creator i Remix
- To obszar eksperymentalny o wysokim potencjale.
- „Dopal z AI” ma być **draft-first**, koszt jawny przed wykonaniem, usage do ledgera.
- Obowiązkowe provenance i zakaz podszywania się pod autorów komentarzy.
- Granice odpowiedzialności: HAIA (entitlements/usage), operator płatności (transakcja), system fakturowy (faktura formalna).

## 4.8. Studio, learning i governance
- Studio jest centrum redakcji/moderacji/aktywacji/analityki i rollbacku.
- Learning Engine ma 2 pętle: individual + population.
- Rozdzielone Global Prior / Cohort Prior / Individual Profile.
- Decyzje aktywacyjne pozostają po stronie człowieka.

## 5. Jak rozumiem stan techniczny i implementacyjny
## 5.1. AI Integration
W dokumencie potwierdzono fundamenty Core i częściową implementację OpenAI adaptera, ale bez pełnego domknięcia (multimodal request, strict structured output, mapowanie, retry/refusal/timeout, pełne testy, DI extension).

## 5.2. Architektura docelowa
Kierunek: .NET 10, modular monolith, PostgreSQL, EF Core, Redis, object storage/CDN, SignalR, worker/scheduler.

## 5.3. Baza danych
Dokument jasno określa, że DDL v0.1 (75 tabel) to atlas koncepcyjny, nie gotowa migracja; wskazuje też listę wykrytych wad i naprawę reaction foundation przed multi-reaction persistence.

## 6. Najważniejsze decyzje kanoniczne, które uznaję za krytyczne
1. Humor jako rdzeń produktu.
2. Mobile-first.
3. Rozwój przez atomic vertical slices.
4. AI izolowane za kontraktami provider-independent.
5. HAIA kontroluje koszt/pamięć/cache/publikację.
6. Reakcje są artefaktami, a model jest multi-reaction.
7. Ekspozycja reakcji musi być śledzona osobno.
8. Sucharek to odrębny wymiar.
9. Age/profession to weak priory.
10. Prior nie może sam potwierdzić Humor DNA.
11. DDL v0.1 wymaga napraw przed implementacją.

## 7. Najważniejsze obszary OPEN (jak je rozumiem)
- finalna tożsamość Reaction/Reaction Version,
- tożsamość i wersjonowanie Second Punchline,
- definicja real exposure,
- maksymalna liczba aktywnych reakcji,
- finalna definicja Reaction Rescue Rate,
- snapshot: current/published/version semantics,
- finalne polityki retencji eventów i raw AI output,
- pricing i billing semantics dla Power.

## 8. Wnioski wykonawcze (jak to przekładam na kolejne prace)
1. Nie wolno implementować szeroko i równolegle wszystkiego.
2. Priorytetem jest sekwencja atomowa wskazana w roadmapie (reaction foundation przed multi-reaction eventami).
3. Każdy kolejny artefakt (DDL, kod, testy) musi rozdzielać:
   - prior vs observed evidence,
   - exposure vs click,
   - AI proposal vs human activation.
4. Product scope i legal/privacy constraints są równie ważne jak model techniczny.

## 9. Ocena pewności zrozumienia
- **Wysoka pewność**: kierunek produktu, onboarding, reaction engine, Sucharek, role Studio, learning loops, zasady AI governance.
- **Średnia pewność**: szczegóły finalnych decyzji OPEN (bo są celowo nierozstrzygnięte).

## 10. Podsumowanie
Moje zrozumienie dokumentu: HAIA ma być systemem humor-first z kontrolowanym AI, audytowalnym wyborem treści i reakcji, silnym naciskiem na semantykę ekspozycji/interakcji oraz bezpieczne, iteracyjne dojrzewanie modelu przez atomowe slice’y. Dokument nie jest listą życzeń, tylko operacyjnym kontraktem decyzji, statusów i kolejności pracy.
