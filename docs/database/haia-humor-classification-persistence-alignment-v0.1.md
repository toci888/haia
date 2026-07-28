# HAIA — DB-first Persistence Alignment Repair v0.1

## 1. Wynik szybkiego database gate
Wykonano pojedynczy, read-only gate na `information_schema` (timeout połączenia 5s, timeout zapytania 30s).
Wynik: **DATABASE_GATE_OK**.

Potwierdzono obecność tabel:
- `humor.classification_axis`
- `humor.classification_value`
- `humor.classification_model_version`
- `humor.classification_model_axis`
- `humor.classification_model_value`
- `humor.sensitivity_category`
- `humor.classification_projection_model_version`
- `humor.classification_value_projection_rule`
- `humor.classification_measure_projection_rule`
- `onboarding.candidate_classification_value`
- `onboarding.candidate_classification_measure`
- `onboarding.candidate_classification_sensitivity`

Potwierdzono też wymagane kolumny `onboarding.candidate_classification`:
- `classification_model_version_id`, `revision_no`, `source_type`, `classification_status`,
- `overall_confidence`, `ai_operation_execution_id`,
- `created_by_account_id`, `reviewed_by_account_id`,
- `supersedes_candidate_classification_id`, `editorial_note`,
- `created_at`, `reviewed_at`, `published_at`, `superseded_at`,
- `predicted_dryness_scale_level_id`, `predicted_dryness_confidence`.

## 2. Przyczyna poprzedniej niespójności scaffoldu
Root cause: niespójny poprzedni refresh modelu.
W `Entities/Generated` występował mieszany namespace (część plików w `...Entities`, część w `...Entities.Generated`), a faktycznie używany `HaiaDbContext` nie miał spójnego odzwierciedlenia nowych tabel klasyfikacji.

## 3. Faktycznie używany `HaiaDbContext`
- Plik: `TociHaia/Toci.Haia.AiIntegration.Core/Toci.Haia.Database.Persistence/Context/Generated/HaiaDbContext.cs`
- Partial: `Context/HaiaDbContext.Extensions.cs`
- Namespace: `Toci.Haia.Database.Persistence.Context`
- Rejestracja DI: `AddDbContext<HaiaDbContext>` w `DependencyInjection/ServiceCollectionExtensions.cs`
- Konsumenci: `Toci.Haia.Studio.Api` i `Toci.Haia.Api` przez `AddHaiaDatabasePersistence(...)`.

## 4. Użyty workflow i komenda scaffoldu (bez sekretów)
Workflow:
1. szybki gate read-only,
2. scaffold do katalogu tymczasowego,
3. porównanie/sprawdzenie,
4. podmiana `Entities/Generated` + `Context/Generated`,
5. usunięcie katalogów tymczasowych,
6. build + smoke test read-only.

Komenda scaffoldu (z ukrytym connection stringiem z ENV):
- `dotnet ef dbcontext scaffold "$env:HAIA_DB_CONNECTION_STRING" Npgsql.EntityFrameworkCore.PostgreSQL --project ".../Toci.Haia.Database.Persistence.csproj" --startup-project ".../Toci.Haia.Database.Persistence.csproj" --context HaiaDbContext --output-dir .tmp-scaffold/Entities/Generated --context-dir .tmp-scaffold/Context/Generated --namespace Toci.Haia.Database.Persistence.Entities --context-namespace Toci.Haia.Database.Persistence.Context --no-onconfiguring --schema identity --schema onboarding --schema humor --schema studio --schema learning --schema ai --schema audit --force`

## 5. Lista nowych mapowanych tabel
W modelu `HaiaDbContext` potwierdzono mapowania wszystkich wymaganych 12 tabel:
1. `classification_axis`
2. `classification_value`
3. `classification_model_version`
4. `classification_model_axis`
5. `classification_model_value`
6. `sensitivity_category`
7. `classification_projection_model_version`
8. `classification_value_projection_rule`
9. `classification_measure_projection_rule`
10. `candidate_classification_value`
11. `candidate_classification_measure`
12. `candidate_classification_sensitivity`

## 6. Zmiany `CandidateClassification`
`CandidateClassification` po refreshu zawiera wymagane pola modelu v0.1, m.in.:
- model/revision/source/status,
- `overall_confidence`,
- lineage do `ai_operation_execution`,
- author/reviewer,
- supersession,
- timestampy lifecycle,
- predicted dryness (`scale_level` + confidence),
- relacje/kolekcje do value/measure/sensitivity.

## 7. Najważniejsze nowe nawigacje
Potwierdzone nawigacje związane z klasyfikacją i nowymi tabelami (nazwy wg scaffoldu):
- `CandidateClassification` ↔ `CandidateClassificationValue`
- `CandidateClassification` ↔ `CandidateClassificationMeasure`
- `CandidateClassification` ↔ `CandidateClassificationSensitivity`
- `CandidateClassification` ↔ `ClassificationModelVersion`
- `CandidateClassification` ↔ `AiOperationExecution`
- `CandidateClassification` ↔ `DrynessScaleLevel`
- `CandidateClassification` ↔ `Account` (created/reviewed)
- `CandidateClassification` ↔ self-reference supersession
- oraz odwrotne nawigacje w encjach słownikowych/modelowych.

## 8. Rzeczywista liczba DbSetów i mapowanych tabel
- `DbSet` count w `HaiaDbContext`: **95**
- `modelBuilder.Entity<...>` count: **95**

## 9. Wynik build Persistence
- `dotnet build Toci.Haia.Database.Persistence/Toci.Haia.Database.Persistence.csproj` → **PASS**

## 10. Wynik build Studio API
- `dotnet build Toci.Haia.Studio.Api/Toci.Haia.Studio.Api.csproj` → **PASS**

## 11. Wynik read-only runtime verification
Smoke test przez nowy `HaiaDbContext` (tylko odczyt) zakończony sukcesem.

## 12. Liczba osi i wartości aktywnego modelu
- aktywny model `haia_humor_classification`, `version_no=1`, `status=active`: **FOUND**
- liczba osi modelu: **12**
- liczba wartości modelu: **103**
- liczba sensitivity categories: **17**
- odczyt tabel przypisań (`value/measure/sensitivity`) działa; aktualnie: **0/0/0** rekordów (to poprawne dla pustego zakresu danych kandydatów).

## 13. Database-only constrainty
Pozostają egzekwowane przez PostgreSQL (poza zakresem EF scaffoldu), m.in.:
- jedna published klasyfikacja na candidate version,
- kontrolowane `published -> superseded`,
- niemutowalność opublikowanej klasyfikacji,
- blokady zmian child po publikacji,
- zgodność supersession w obrębie tej samej wersji materiału,
- cross-table consistency model/axis/value,
- zakresy `relevance`, `confidence`, `normalized_value`.

## 14. Wynik kontroli diffu
- Zmiany ograniczone do `Toci.Haia.Database.Persistence` + dokumentacji tego atomu.
- Brak zmian API/Web.
- Brak migracji i brak zmian DDL/SQL.
- Spójny namespace encji po refreshu (`95/95` w `Toci.Haia.Database.Persistence.Entities`).

## 15. Wynik kontroli sekretów
- Brak dodania sekretów do kodu wygenerowanego.
- Brak `OnConfiguring` z `UseNpgsql(...)` i literalnym połączeniem.
- Connection string używany wyłącznie z ENV/runtime.

## 16. Elementy `NOT VERIFIED`
- Brak testu zapisu/transakcji (celowo, tylko read-only smoke).
- Brak weryfikacji zachowania triggerów/partial indexów przez testy DML (poza zakresem tego atomu).

## 17. Nieoczekiwane różnice
- Podczas restore/build pojawiły się ostrzeżenia NuGet `NU1903` (podatności tranzytywne), niezwiązane z tym atomem i bez aktualizacji pakietów.

## 18. Znane ograniczenia
- EF scaffolding nie odwzorowuje wprost pełnej semantyki triggerów/partial indexów.
- Walidacja obejmowała odczyt modelu i mapowań, nie obejmowała operacji zapisu.

## 19. Rekomendacja wznowienia
Persistence alignment jest gotowy do użycia.
Można wznowić następny atom:
**`Studio Meme Intake + AI Evaluation v0.1`**.
