# HAIA Humor Classification Foundation v0.1

## 1. Cel zmiany

Celem atomu jest rozszerzenie schematu PostgreSQL o relacyjny i wersjonowany model klasyfikacji humoru materiału (meme/joke), z rozdzieleniem pojęć:

- **axis weight** (waga osi w modelu),
- **value default weight** (waga domyślna wartości),
- **relevance score** (siła wystąpienia cechy w materiale),
- **confidence** (pewność przypisania),
- **projection effect weight** (wpływ cechy materiału na wymiar Humor DNA).

Zakres obejmuje wyłącznie SQL (DDL/DML + komentarze), bez zmian w kodzie aplikacji i bez EF rescaffold.

## 2. Źródła i obiekty selektywnie przejrzane

Selektwnie przejrzano fragmenty:

- `docs/database/haia-registration-onboarding-foundation-v0.1.sql`:
  - `onboarding.candidate_classification`,
  - `onboarding.onboarding_candidate`,
  - `onboarding.onboarding_candidate_version`,
  - `humor.reaction_mechanism`,
  - `humor.dryness_scale*`,
  - `humor.humor_dimension_model_version`,
  - `humor.humor_dimension_model_member`,
  - `ai.ai_operation_execution`,
  - `identity.account`.

## 3. Plik SQL

Utworzono nowy forward patch:

- `docs/database/haia-humor-classification-foundation-v0.1.sql`

Nie modyfikowano oryginalnego pliku foundation DDL.

## 4. Zmienione istniejące tabele

- `onboarding.candidate_classification` (ALTER TABLE + constraints/indexy/komentarze).

## 5. Nowe tabele

W schemacie `humor`:

1. `classification_axis`
2. `classification_value`
3. `classification_model_version`
4. `classification_model_axis`
5. `classification_model_value`
6. `sensitivity_category`
7. `classification_projection_model_version`
8. `classification_value_projection_rule`
9. `classification_measure_projection_rule`

W schemacie `onboarding`:

10. `candidate_classification_value`
11. `candidate_classification_measure`
12. `candidate_classification_sensitivity`

## 6. Nowe kolumny `onboarding.candidate_classification`

Dodane kolumny:

- `classification_model_version_id`
- `revision_no`
- `source_type`
- `classification_status`
- `overall_confidence`
- `ai_operation_execution_id`
- `created_by_account_id`
- `reviewed_by_account_id`
- `supersedes_candidate_classification_id`
- `editorial_note`
- `created_at`
- `reviewed_at`
- `published_at`
- `superseded_at`
- `predicted_dryness_scale_level_id`
- `predicted_dryness_confidence`

## 7. Usunięta stara unikalność i nowe zasady wersjonowania

Usunięto selektywnie wyłącznie legacy unikalność blokującą historię (`UNIQUE(onboarding_candidate_version_id)`), dynamicznie po metadanych `pg_constraint`.

Dodano:

- unikalność rewizji: `(onboarding_candidate_version_id, revision_no)`,
- partial unique: max jedna `published` klasyfikacja na wersję kandydata,
- walidacje statusów i timestampów,
- walidację supersession do tej samej `onboarding_candidate_version_id`,
- immutability published z dopuszczonym jedynie przejściem `published -> superseded`.

## 8. Osie, wartości i model v1

Zasiano model:

- `model_key = haia_humor_classification`
- `version_no = 1`
- `version_label = v1`
- `status = active`

### Osie v1 (12)

- mechanism
- joke_structure
- tone
- emotional_effect
- audience_knowledge
- reference_domain
- cultural_reference
- absurdity
- intensity
- complexity
- universality
- rescue_potential

### Liczba seedowanych wartości (kategoryczne osie)

- mechanism: **20**
- joke_structure: **16**
- tone: **13**
- reference_domain: **23**
- cultural_reference: **12**
- audience_knowledge: **8**
- emotional_effect: **11**

Łącznie wartości kategorycznych: **103**.

Dodatkowo patch dopina brakujące aktywne `humor.reaction_mechanism.mechanism_key` do osi `mechanism`, bez duplikacji istniejących `value_key`.

## 9. Tabela wag osi (v1)

| axis_key | weight |
|---|---:|
| mechanism | 1.00 |
| joke_structure | 0.85 |
| tone | 0.75 |
| emotional_effect | 0.70 |
| audience_knowledge | 0.65 |
| reference_domain | 0.55 |
| cultural_reference | 0.50 |
| absurdity | 0.90 |
| intensity | 0.70 |
| complexity | 0.55 |
| universality | 0.45 |
| rescue_potential | 0.60 |

## 10. Rozróżnienie wag i metryk

- **axis_weight**: `humor.classification_model_axis.axis_weight`.
- **value default weight**: `humor.classification_model_value.default_weight`.
- **relevance**: `onboarding.candidate_classification_value.relevance_score`.
- **confidence**:
  - `candidate_classification.overall_confidence`,
  - `candidate_classification_value.confidence`,
  - `candidate_classification_measure.confidence`,
  - `candidate_classification_sensitivity.confidence`.
- **projection effect weight**:
  - `humor.classification_value_projection_rule.effect_weight`,
  - `humor.classification_measure_projection_rule.effect_weight`.

## 11. Model safety

Wprowadzono niezależny relacyjny model safety:

- słownik: `humor.sensitivity_category` (17 kategorii),
- przypisania: `onboarding.candidate_classification_sensitivity` z:
  - `severity_level` 0..3,
  - `confidence` 0..1,
  - `moderation_relevance`.

Model safety jest oddzielony od affinity i nie zastępuje istniejącego workflow moderation.

## 12. Integracja Sucharka

Dodano do `onboarding.candidate_classification`:

- `predicted_dryness_scale_level_id` -> FK do `humor.dryness_scale_level`,
- `predicted_dryness_confidence` (0..1).

Legacy `dryness` zachowano i oznaczono jako deprecated.

Backfill mapuje `dryness` 1..6 do poziomów aktywnej skali `default_sucharek`.

## 13. Foundation projekcji do Humor DNA

Dodano strukturę:

- `humor.classification_projection_model_version`,
- `humor.classification_value_projection_rule`,
- `humor.classification_measure_projection_rule`.

Reguły projekcji nie są seedowane arbitralnie w tym atomie (świadoma decyzja), aby uniknąć nieuzasadnionego mapowania materiał -> DNA bez polityki produktowej.

## 14. Strategia migracji istniejących danych

Backfill (idempotentny) dla istniejących `onboarding.candidate_classification`:

1. przypisuje model `haia_humor_classification/v1`,
2. ustawia `revision_no = 1`,
3. ustawia `source_type = system_migration`,
4. ustawia status zachowawczy `approved`,
5. mapuje legacy miary do `candidate_classification_measure`:
   - intensity: `(value - 1) / 4`,
   - complexity: `(value - 1) / 4`,
   - universality_score: bezpośrednio,
   - reaction_rescue_potential: bezpośrednio,
6. mapuje `dryness` 1..6 do `predicted_dryness_scale_level_id` aktywnej skali Sucharka,
7. nie migruje `age_hint_strength` i `profession_hint_strength` do Humor DNA,
8. nie traktuje dowolnych legacy JSONB jako zatwierdzonego słownika.

## 15. Indeksy i constrainty

Patch wprowadza m.in.:

- lookup osi po `axis_key` i aktywności,
- lookup wartości po osi i `value_key`,
- aktywny model per `model_key` (partial unique),
- wersjonowanie klasyfikacji rewizjami,
- pojedynczą publikację per wersja kandydata (partial unique),
- indeksy pod lookupy value/measure/sensitivity,
- indeks lineage pod `ai_operation_execution_id`,
- indeksy lookup dla projection model.

Dodatkowo użyto triggerów dla reguł cross-row/cross-table:

- spójność i statusy rewizji,
- immutability published,
- blokada zmian child po publikacji,
- walidacja typu osi dla miar liczbowych.

## 16. Przykładowe zapytania

1) Aktywny model i słowniki:

```sql
SELECT mv.classification_model_version_id, a.axis_key, v.value_key
FROM humor.classification_model_version mv
JOIN humor.classification_model_axis ma ON ma.classification_model_version_id = mv.classification_model_version_id
JOIN humor.classification_axis a ON a.classification_axis_id = ma.classification_axis_id
LEFT JOIN humor.classification_model_value mvv ON mvv.classification_model_axis_id = ma.classification_model_axis_id
LEFT JOIN humor.classification_value v ON v.classification_value_id = mvv.classification_value_id
WHERE mv.model_key = 'haia_humor_classification' AND mv.status = 'active';
```

2) Opublikowany fingerprint materiału:

```sql
SELECT c.candidate_classification_id, c.onboarding_candidate_version_id, c.published_at,
	   cv.classification_model_value_id, cv.relevance_score, cv.confidence,
	   cm.classification_model_axis_id, cm.normalized_value, cm.confidence AS measure_confidence
FROM onboarding.candidate_classification c
LEFT JOIN onboarding.candidate_classification_value cv ON cv.candidate_classification_id = c.candidate_classification_id
LEFT JOIN onboarding.candidate_classification_measure cm ON cm.candidate_classification_id = c.candidate_classification_id
WHERE c.onboarding_candidate_version_id = $1 AND c.classification_status = 'published';
```

3) Filtrowanie po mechanizmie + relevance/confidence:

```sql
SELECT c.onboarding_candidate_version_id, cv.relevance_score, cv.confidence
FROM onboarding.candidate_classification c
JOIN onboarding.candidate_classification_value cv ON cv.candidate_classification_id = c.candidate_classification_id
JOIN humor.classification_model_value mvv ON mvv.classification_model_value_id = cv.classification_model_value_id
JOIN humor.classification_value v ON v.classification_value_id = mvv.classification_value_id
JOIN humor.classification_axis a ON a.classification_axis_id = v.classification_axis_id
WHERE c.classification_status = 'published'
  AND a.axis_key = 'mechanism'
  AND v.value_key = 'absurd'
  AND cv.relevance_score >= 0.70
  AND COALESCE(cv.confidence, 0) >= 0.80;
```

4) Filtrowanie po absurdity/intensity:

```sql
SELECT c.onboarding_candidate_version_id
FROM onboarding.candidate_classification c
JOIN onboarding.candidate_classification_measure m_abs ON m_abs.candidate_classification_id = c.candidate_classification_id
JOIN onboarding.candidate_classification_measure m_int ON m_int.candidate_classification_id = c.candidate_classification_id
JOIN humor.classification_model_axis a_abs ON a_abs.classification_model_axis_id = m_abs.classification_model_axis_id
JOIN humor.classification_model_axis a_int ON a_int.classification_model_axis_id = m_int.classification_model_axis_id
JOIN humor.classification_axis ax_abs ON ax_abs.classification_axis_id = a_abs.classification_axis_id
JOIN humor.classification_axis ax_int ON ax_int.classification_axis_id = a_int.classification_axis_id
WHERE c.classification_status = 'published'
  AND ax_abs.axis_key = 'absurdity'
  AND m_abs.normalized_value BETWEEN 0.80 AND 1.00
  AND ax_int.axis_key = 'intensity'
  AND m_int.normalized_value BETWEEN 0.50 AND 0.80;
```

5) Materiały wymagające moderacji:

```sql
SELECT c.onboarding_candidate_version_id, s.severity_level, sc.category_key
FROM onboarding.candidate_classification c
JOIN onboarding.candidate_classification_sensitivity s ON s.candidate_classification_id = c.candidate_classification_id
JOIN humor.sensitivity_category sc ON sc.sensitivity_category_id = s.sensitivity_category_id
WHERE c.classification_status IN ('approved','published')
  AND s.moderation_relevance = true
  AND s.severity_level >= 2;
```

## 17. Wynik walidacji

Wykonano walidację statyczną patcha:

- sprawdzono obecność wymaganych obiektów i reguł,
- sprawdzono brak operacji destrukcyjnych (`DROP TABLE`, `DROP COLUMN`, `TRUNCATE`, `DELETE`),
- potwierdzono idempotentny charakter seeda (ON CONFLICT / NOT EXISTS),
- potwierdzono zakres zmian ograniczony do schematów i obiektów z zadania.

`PATCH NOT APPLIED PERSISTENTLY`

## 18. NOT VERIFIED

- Brak runtime wykonania patcha w bezpiecznej transakcji testowej z rollback na lokalnej bazie użytkownika.
- Brak runtime potwierdzenia działania triggerów i partial unique na rzeczywistych danych.
- Brak runtime pomiaru planów zapytań (`EXPLAIN`) na danych produkcyjnych.

Status: `NOT RUNTIME VERIFIED`.

## 19. Znane ograniczenia

1. Reguły projection -> Humor DNA pozostawiono puste (bez arbitralnych seedów).
2. Backfill opiera się na aktualnie aktywnej skali `default_sucharek`.
3. Twarda walidacja business flow publish/supersede realizowana triggerami (wymaga testów runtime po akceptacji patcha).
4. Brak automatycznego, trwałego bridge-a sync między `humor.reaction_mechanism` i `humor.classification_value` (zastosowano tylko seed pomocniczy).

## 20. Następny krok

1. Manualna akceptacja patcha SQL.
2. Ręczne wykonanie patcha na środowisku DB zgodnie z procedurą release.
3. W osobnym atomie: DB-first persistence refresh/rescaffold i dopiero potem warstwa API/Studio.
