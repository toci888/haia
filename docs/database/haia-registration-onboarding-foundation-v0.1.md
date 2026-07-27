# HAIA Registration & Onboarding Database Foundation v0.1

Internal revision: 0.2 — in-place repair

## Revision 0.2 Change Log
- Wprowadzono stabilną tożsamość reakcji: `humor.reaction` + wersjonowanie `humor.reaction_version(reaction_id, version_no)`.
- Rozszerzono `humor.reaction_pack_version` o politykę 12/6 i tryb interakcji Sucharka.
- Przebudowano `humor.reaction_pack_item` pod pełną pulę, sloty początkowe i replacement.
- Dodano `humor.reaction_exposure` (real rendered exposure) i nowy model multi-selection w `humor.reaction_choice`.
- Dodano wersjonowany model Sucharka: `dryness_scale`, `dryness_scale_version`, `dryness_scale_level`, `dryness_rating` + triggery exclusivity.
- Rozdzielono prior vs observed evidence: nowa `humor.onboarding_prior_hypothesis`; `humor.humor_evidence` bez age/profession hints.
- Dodano membership wersji modelu DNA: `humor.humor_dimension_model_member` i relacyjne spięcie working/evidence/snapshot.
- Naprawiono wersjonowanie snapshotu (`UNIQUE(session,snapshot_version)`, supersedes, published constraints) i niemutowalność published snapshot.
- Zastąpiono `onboarding.candidate_media_asset` przez `onboarding.media_asset` + `onboarding.candidate_version_media`.
- Dodano brakujące FK polityk (`selection_decision`, `candidate_presentation`, `recovery_event`) i komentarze dla świadomie polymorphic UUID.
- Utwardzono retencję legal records (`ON DELETE RESTRICT`) oraz append-oriented audit przez trigger blokujący UPDATE/DELETE.

## A. Executive Summary
W rewizji 0.2 naprawiono P0/P1 integralności w szkicu DDL: stabilną tożsamość i wersjonowanie reakcji, model real exposure, multi-reaction selection history, wersjonowany Sucharek 1–6, formalne rozdzielenie prior vs observed evidence, membership modelu Humor DNA, wersjonowanie i niemutowalność published Initial Humor Snapshot, retencję legal records oraz techniczne wymuszenie append-oriented audytu.

## B. Scope
W zakresie: identity, legal acceptance, optional contexts (profession/age), onboarding session state, candidate/reaction versioning, interaction events, explainability trail, working/snapshot Humor DNA, AI generation metadata, studio workflow, learning snapshots, audit, hooki dla Meme Calibration i User Humor Inspiration.

Poza zakresem: social graph, feed, chat, marketplace, płatności, licensing, pełny moderation pipeline platformy, storage/CDN binarek.

## C. Source Documentation
Przeczytane źródła:
- `F:\software\haia\docs\HAIA_WORKING_MEMORY.md`
- `F:\software\haia\raport_v1.txt`
- `F:\software\haia\docs\analysis\haia-foundational-domain-clarification-v0.2.txt`
- `F:\software\haia\docs\analysis\haia-legacy-repository-audit-v0.1.md`
- `F:\software\haia\docs\analysis\haia-legacy-salvage-register-v0.1.md`
- `F:\software\haia\docs\features\registration\haia-user-registration-flow-v0.1.md`
- `F:\software\haia\docs\features\registration\haia-entry-and-adaptive-humor-onboarding-v0.2.md`
- `F:\software\haia\docs\features\registration\haia-onboarding-population-learning-addendum-v0.1.md`
- `F:\software\haia\docs\features\registration\haia-onboarding-studio-v0.1.md`
- `F:\software\haia\docs\features\registration\haia-onboarding-age-context-addendum-v0.1.md`
- `TociHaia/Toci.Haia.AiIntegration.Core/haia_ai_integration_technical_report_v0.1.txt`
- kontrakty `GenerateCustomReactionsFromImageRequest` i `GenerateCustomReactionsFromImageResponse`.

## D. Database Schemas
- `identity` — konto, metody dostępu, e-mail, dokumenty prawne.
- `onboarding` — flow/session, kandydaci, aktywacje, prezentacje, feedback, decyzje selekcji.
- `humor` — reaction packs, reaction versions, second punchline, humor dimensions, snapshot.
- `studio` — role wewnętrzne, review/moderation/activation, decyzje dot. rekomendacji.
- `learning` — policy/cohort definitions i agregaty skuteczności.
- `ai` — prompt/version, operation execution, usage, output.
- `audit` — append-oriented audit trail (DB-enforced immutability of written events).

Kierunek zależności: `identity` (bazowe) → `onboarding/humor` → `studio/learning/ai` → `audit`.

## E. Naming and PostgreSQL Conventions
- PostgreSQL 16+, `uuid` + `gen_random_uuid()`.
- `timestamptz` dla zdarzeń systemowych.
- `citext` dla e-maili i znormalizowanego pseudonimu.
- `jsonb` tylko dla konfiguracji/payloadów wyjaśnialności (policy config, score components, AI output, metadata).
- `text` + `CHECK` tam, gdzie listy statusów są ewolucyjne produktowo.
- `snake_case`, bez `tbl_`, bez CSV w kolumnie.

## F. Identity and Access Model
Model wspiera wielometodowe logowanie jednego konta:
- `identity.account`
- `identity.account_email`
- `identity.password_credential`
- `identity.external_login`
- `identity.security_token`
- `identity.public_profile`

Invariants DB:
- global unique `(provider, provider_subject)`;
- jeden aktywny hash hasła per konto (partial unique);
- jeden primary email per konto (partial unique);
- brak jawnych tokenów i haseł.

## G. Registration and Verification
Email verification i idempotencja:
- `identity.security_token` z `token_hash`, `expires_at`, `used_at`, `invalidated_at`, `resend_sequence`.
- indeks aktywnych tokenów i unikalność hash.
- aktywacja konta i reguły „last login method” pozostają częściowo domenowe (transakcja aplikacyjna).

## H. Legal Documents
Wersjonowanie i akceptacje:
- `identity.legal_document`
- `identity.legal_document_version`
- `identity.account_document_acceptance`

Każda akceptacja odnosi się do konkretnej wersji dokumentu i typu akcji (accepted/acknowledged/declined).

## I. Profession and Age Context
Rozdzielenie age compliance vs age personalization:
- compliance: `onboarding.age_eligibility_record`
- personalization: `onboarding.account_age_context` + `onboarding.age_range`
- profession context: `onboarding.industry`, `onboarding.profession`, `onboarding.account_profession_context`

## J. Onboarding Session Model
- `onboarding.onboarding_flow_version`
- `onboarding.onboarding_session`
- `onboarding.onboarding_step_state`

Model wspiera resume cross-device, statusy flow, skip optional steps, last activity i versioned flow.

## K. Candidate and Media Model
- `onboarding.onboarding_candidate`
- `onboarding.onboarding_candidate_version`
- `onboarding.media_asset`
- `onboarding.candidate_version_media`
- `onboarding.candidate_activation`
- `onboarding.candidate_classification`

Rozdzielono `content_format` (na kandydacie) od `role_in_flow` (na candidate version). Media mają stabilny asset lifecycle i wielokrotne użycie przez wiele wersji kandydatów.

## L. Custom Reactions and Second Punchlines
- `humor.reaction_pack`
- `humor.reaction_pack_version`
- `humor.reaction`
- `humor.reaction_version`
- `humor.reaction_pack_item`
- `humor.reaction_exposure`
- `humor.reaction_version_mechanism`
- `humor.reaction_choice`
- `humor.second_punchline_view`
- `humor.second_punchline_feedback`

Model wspiera: stable reaction identity, linię wersji reakcji, pulę ~12 / initial ~6, dynamic replacement, „więcej”, multi-select i oddzielenie kliknięcia reakcji od real exposure oraz eventów drugiej puenty.

## M. AI Generation Persistence
- `ai.ai_prompt_template`
- `ai.ai_prompt_template_version`
- `ai.ai_generation_target`
- `ai.ai_operation_execution`
- `ai.ai_operation_usage`
- `ai.ai_generated_output`

Model przechowuje metadata wykonania, nie przechowuje sekretów ani obrazu binarnego.

## N. User Interaction Events
- `onboarding.candidate_presentation`
- `onboarding.candidate_feedback`
- `humor.reaction_exposure`
- `humor.reaction_choice`
- `humor.second_punchline_view`
- `humor.second_punchline_feedback`
- `humor.dryness_rating`

Wolumenowo to tabele kandydackie do przyszłego partycjonowania.

## O. Working Humor DNA
- `humor.humor_dimension_definition`
- `humor.humor_dimension_model_version`
- `humor.humor_dimension_model_member`
- `humor.onboarding_working_humor_dimension`
- `humor.onboarding_prior_hypothesis`
- `humor.humor_evidence`

Working profile, prior i observed evidence wskazują member konkretnej wersji modelu, a nie luźny dimension definition.

## P. Initial Humor DNA Snapshot
- `humor.initial_humor_snapshot`
- `humor.initial_humor_snapshot_dimension`
- `humor.humor_verdict`

Snapshot ma realne wersjonowanie (`snapshot_version` per session), relację `supersedes_snapshot_id`, jeden published na sesję (partial unique) i triggerową niemutowalność published snapshot oraz jego children.

## Q. Humor Contexts
W tym draftcie konteksty wejściowe są rozdzielone jako:
- age context (`onboarding.account_age_context`),
- profession context (`onboarding.account_profession_context`),
- prior hypothesis (`humor.onboarding_prior_hypothesis`),
- observed evidence (`humor.humor_evidence`).

Prior (age/profession/global/cohort) nie jest observed evidence i nie potwierdza samodzielnie wymiaru Humor DNA.

## R. Population Learning
- `learning.policy_definition` / `learning.policy_version`
- `learning.cohort_definition` / `learning.cohort_definition_version`
- snapshoty skuteczności (candidate/reaction/cohort/funnel/recovery)
- `learning.prediction_outcome`

## S. Global and Cohort Priors
Global prior i cohort prior są modelowane przez:
- policy versions,
- cohort definition versions,
- snapshoty agregatów z `sample_size`, `confidence`, `period`.

## T. Selection Explainability
- `onboarding.selection_decision`
- `onboarding.selection_decision_alternative`
- powiązanie do `candidate_presentation`.

Zapisuje score components i prior sources bez zapisu pełnego rankingu wszystkich kandydatów.

## U. Recovery Model
- `onboarding.recovery_event` z triggerem, strategią, wynikiem i powiązaniem do decyzji selekcji.

## V. Onboarding Studio
- role i przypisania (`studio.account_role`, `studio.account_role_assignment`)
- review/moderation (`studio.editorial_review`, `studio.moderation_review`, `studio.moderation_decision`)
- komentarze i zmiany aktywacji (`studio.studio_comment`, `studio.activation_change`)
- rekomendacje (`studio.learning_recommendation`, `studio.recommendation_decision`).

## W. Moderation and Activation
Statusy kandydata, reaction packu i reaction version są rozdzielone.
Aktywacja ma okna czasowe, cohort scope, experiment key i kill switch.

## X. Analytics and Aggregates
Snapshot tables (batch-ready):
- `learning.candidate_performance_snapshot`
- `learning.reaction_performance_snapshot`
- `learning.cohort_candidate_performance_snapshot`
- `learning.onboarding_funnel_snapshot`
- `learning.recovery_performance_snapshot`
- `learning.prediction_outcome`

## Y. Privacy and Retention
Wrażliwe dane:
- `identity.account_email`, `identity.security_token`, `onboarding.age_eligibility_record`, konteksty wieku/zawodu, interaction events.

Rekomendacja retencji:
- legal acceptance: legal retention,
- age eligibility record: legal retention,
- security tokens: short-term,
- raw AI output: configurable,
- eventy onboardingowe: configurable + pseudonimizacja po usunięciu konta,
- snapshoty agregatów: aggregate-only long-term.

W rewizji 0.2 ślady prawne (`identity.account_document_acceptance`, `onboarding.age_eligibility_record`) nie są już usuwane przez CASCADE po koncie.

## Z. Security Decisions
- tokeny i hasła wyłącznie hash,
- brak access/refresh token storage,
- `citext` unika case-based account duplication,
- audyt append-oriented w `audit.audit_event` jest wymuszony triggerami blokującymi UPDATE/DELETE.

## AA. Index Strategy
Kluczowe indeksy:
- active/primary email,
- provider+subject unique,
- active token lookup,
- unique nickname (aktywny),
- one active password,
- one current feedback,
- active reaction choice per (presentation, account, pack item) przez partial unique,
- one active dryness rating per (presentation, account) przez partial unique,
- session/presentation time indices,
- correlation index dla AI execution,
- target/actor/time dla audytu.

## AB. Concurrency and Idempotency
Wspierane przez:
- unique constraints (provider subject, current feedback, active reaction choice per item, active dryness rating),
- token hash uniqueness,
- row_version na `identity.account` i `onboarding.onboarding_session`,
- idempotency key na `humor.reaction_exposure`, `humor.reaction_choice`, `humor.dryness_rating`,
- kompozytowe FK wymuszające spójność choice → real exposure.

Reguły wymagające logiki serwisowej:
- niedopuszczenie odłączenia ostatniej metody logowania,
- atomowe przełączanie `is_current` i `is_active`,
- conflict resolution równoległej moderacji.

## AC. Tier Classification
- TIER 1: 51 tabel (first working flow + repaired reaction/humor foundation)
- TIER 2: 28 tabel (studio + learning + AI persistence)
- TIER 3: 5 tabel (optional future hooks)
- Razem: 84 tabele

## AD. Table Inventory
Legenda: `vol` (L/M/H/VH), `ret` (AL=account lifetime, LEG=legal, CFG=configurable, ST=short-term, AGG=aggregate-only), `part` (Y/N), `pii` (Y/N), `restricted` (Y/N)

### identity
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| identity.account | konto HAIA | 1 | Identity | root | status check | M | AL | N | Y | Y |
| identity.account_email | e-mail + verify state | 1 | Identity | account | active unique email, one primary | M | AL | N | Y | Y |
| identity.password_credential | hash hasła | 1 | Identity | account | one active per account | M | AL | N | Y | Y |
| identity.external_login | social providers | 1 | Identity | account | unique(provider,subject) | M | AL | N | Y | Y |
| identity.security_token | verification/reset hash token | 1 | Identity | account/account_email | unique token_hash | H | ST | Y | Y | Y |
| identity.public_profile | pseudonim publiczny | 1 | Identity | account | unique active nickname | M | AL | N | Y | N |
| identity.legal_document | typ dokumentu | 1 | Legal | versions | unique document_key | L | LEG | N | N | Y |
| identity.legal_document_version | wersje dokumentu | 1 | Legal | legal_document | unique(doc,version) | L | LEG | N | N | Y |
| identity.account_document_acceptance | akceptacja wersji | 1 | Legal | account, doc_version | unique(account,version,type) | H | LEG | Y | Y | Y |

### onboarding
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| onboarding.industry | katalog branż | 1 | Product | profession | unique key | L | CFG | N | N | N |
| onboarding.profession | katalog zawodów | 1 | Product | industry | unique key | L | CFG | N | N | N |
| onboarding.account_profession_context | opcjonalny kontekst zawodowy | 1 | Onboarding | account/profession | one active context | M | CFG | N | Y | Y |
| onboarding.age_range | katalog przedziałów wieku | 1 | Product | age_context | unique key | L | CFG | N | N | N |
| onboarding.account_age_context | opcjonalny age context | 1 | Onboarding | account/age_range | one active context | M | CFG | N | Y | Y |
| onboarding.age_eligibility_record | compliance wieku | 1 | Legal | account | status/source checks | M | LEG | N | Y | Y |
| onboarding.onboarding_flow_version | wersja flow | 1 | Product | sessions | one active per key | L | CFG | N | N | Y |
| onboarding.onboarding_session | sesja onboardingu | 1 | Onboarding | account/flow | status/source checks | H | CFG | Y | Y | Y |
| onboarding.onboarding_step_state | status kroku | 1 | Onboarding | session | unique(session,step) | H | CFG | Y | Y | Y |
| onboarding.onboarding_candidate | stabilna tożsamość karty | 1 | Studio | versions | lifecycle status check | M | CFG | N | N | Y |
| onboarding.onboarding_candidate_version | wersja karty | 1 | Studio | candidate | unique(candidate,version) | H | CFG | N | N | Y |
| onboarding.media_asset | stabilny asset media | 1 | Studio | candidate_version_media | unique storage_key + sha256 | H | CFG | N | N | Y |
| onboarding.candidate_version_media | mapowanie wersji karty do assetu | 1 | Studio | candidate_version/media_asset | unique(version,asset,role) | H | CFG | N | N | Y |
| onboarding.candidate_activation | aktywacja wersji | 1 | Studio | candidate_version | activation scope/time checks | M | CFG | N | N | Y |
| onboarding.selection_decision | decyzja wyboru i explainability | 1 | Ranking | session/selected candidate | unique(session,position) | H | CFG | Y | Y | Y |
| onboarding.selection_decision_alternative | top N alternatywy | 1 | Ranking | decision | unique(decision,rank) | H | CFG | Y | N | Y |
| onboarding.candidate_presentation | ekspozycja karty | 1 | Onboarding | session/account/candidate/reactions | unique(session,position) | VH | CFG | Y | Y | Y |
| onboarding.candidate_feedback | ocena treści głównej | 1 | Onboarding | presentation/account | one current feedback | VH | CFG | Y | Y | Y |
| onboarding.candidate_classification | klasyfikacja semantyczna | 2 | Studio | candidate_version | unique(candidate_version) | M | CFG | N | N | Y |
| onboarding.recovery_event | recovery trigger i wynik | 2 | Ranking | session/decision | strategy check | H | CFG | Y | Y | Y |

### humor
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| humor.reaction_mechanism | słownik mechanizmów | 1 | Product | reaction map | unique key | L | CFG | N | N | N |
| humor.reaction_pack | stabilna tożsamość pakietu | 1 | Studio | candidate_version | unique key per candidate/lang | M | CFG | N | N | Y |
| humor.reaction_pack_version | wersja pakietu reakcji | 1 | Studio | reaction_pack | unique(pack,version), one active, 12/6 policy checks | H | CFG | N | N | Y |
| humor.reaction | stabilna tożsamość reakcji | 1 | Studio | reaction_version | unique reaction_key | M | CFG | N | N | Y |
| humor.reaction_version | wersja pojedynczej reakcji | 1 | Studio | reaction/pack items | unique(reaction,version), intensity/status checks | H | CFG | N | N | Y |
| humor.reaction_version_mechanism | M:N reaction↔mechanism | 1 | Studio | reaction/mechanism | PK pair | H | CFG | N | N | N |
| humor.reaction_pack_item | pozycja reakcji w pakiecie | 1 | Studio | pack_version/reaction_version | unique pool_order, unique reaction, partial unique initial_slot | H | CFG | N | N | Y |
| humor.reaction_exposure | realna ekspozycja reakcji | 1 | Onboarding | presentation/item/account | unique(presentation,sequence), idempotency, visibility range | VH | CFG | Y | Y | Y |
| humor.reaction_choice | wybór reakcji przez usera | 1 | Onboarding | presentation/item/account/exposure | active-per-item partial unique, idempotency (per presentation+account), exposure FK consistency | VH | CFG | Y | Y | Y |
| humor.second_punchline_view | odsłonięcie puenty | 1 | Onboarding | reaction_choice | source check | VH | CFG | Y | Y | Y |
| humor.second_punchline_feedback | feedback puenty | 1 | Onboarding | punchline_view | feedback checks | H | CFG | Y | Y | Y |
| humor.humor_dimension_definition | słownik wymiarów DNA | 1 | Product | working/snapshot | unique key | L | CFG | N | N | N |
| humor.humor_dimension_model_version | wersja modelu wymiarów | 1 | Product | members/snapshot | unique(model,version) | L | CFG | N | N | Y |
| humor.humor_dimension_model_member | membership wymiaru w wersji modelu | 1 | Product | model_version/dimension | unique(model_version,dimension), bounds+weight checks | L | CFG | N | N | Y |
| humor.onboarding_working_humor_dimension | working profile value | 1 | Onboarding | session/model_member | unique(session,member), composite FK to session model | H | CFG | N | Y | Y |
| humor.onboarding_prior_hypothesis | prior hipoteza onboardingowa | 1 | Onboarding | session/model_member/context | source-type constraints, optional age/profession/cohort source | H | CFG | N | Y | Y |
| humor.humor_evidence | observed evidence | 1 | Onboarding | feedback/reaction/punchline/dryness | bez age/profession hints, member-based FK | VH | CFG | Y | Y | Y |
| humor.initial_humor_snapshot | snapshot initial DNA | 1 | Onboarding | session/account/model | unique(session,version), one published/session | H | AL | N | Y | Y |
| humor.initial_humor_snapshot_dimension | wartości wymiarów snapshotu | 1 | Onboarding | snapshot/model_member | unique(snapshot,member), composite model consistency FK | H | AL | N | Y | Y |
| humor.dryness_scale | stabilna skala Sucharka | 1 | Product | versions | unique scale_key | L | CFG | N | N | N |
| humor.dryness_scale_version | wersja skali Sucharka | 1 | Product | scale/levels | unique(scale,version) | L | CFG | N | N | N |
| humor.dryness_scale_level | poziom 1..6 w wersji skali | 1 | Product | scale_version | unique(version,level_no) | L | CFG | N | N | N |
| humor.dryness_rating | wybór poziomu Sucharka | 1 | Onboarding | presentation/account/scale_level | one active per presentation+account, idempotency (per presentation+account) | H | CFG | Y | Y | Y |
| humor.humor_verdict | finalny werdykt | 1 | Onboarding | snapshot/ai_exec | unique(snapshot) | M | AL | N | Y | Y |
| humor.meme_calibration_session | opcjonalna sesja kalibracji | 3 | Onboarding | session/account | moderation/privacy checks | M | CFG | N | Y | Y |
| humor.meme_calibration_variant | warianty kalibracji | 3 | Onboarding | calibration_session | unique(session,variant_no) | M | CFG | N | Y | Y |
| humor.meme_calibration_feedback | ocena wariantu | 3 | Onboarding | variant/account | rating checks | M | CFG | N | Y | Y |
| humor.user_humor_inspiration | własna inspiracja usera | 3 | Onboarding | account/session | type/privacy checks | M | CFG | N | Y | Y |
| humor.user_humor_inspiration_analysis | analiza inspiracji | 3 | AI/Onboarding | inspiration/ai_exec | - | M | CFG | N | Y | Y |

### ai
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| ai.ai_prompt_template | stabilny klucz promptu | 2 | AI | versions | unique key | L | CFG | N | N | Y |
| ai.ai_prompt_template_version | wersja promptu | 2 | AI | template | unique(template,version), one active | M | CFG | N | N | Y |
| ai.ai_generation_target | mapowanie celu generacji | 2 | AI | candidate/reaction targets | target type check | M | CFG | N | N | Y |
| ai.ai_operation_execution | metadane wykonania operacji AI | 2 | AI | target/prompt | status check, correlation index | H | CFG | Y | Y | Y |
| ai.ai_operation_usage | usage tokenów | 2 | AI | execution | unique(execution) | H | CFG | Y | N | Y |
| ai.ai_generated_output | structured raw + mapped output | 2 | AI | execution | output type check | H | CFG | Y | Y | Y |

### studio
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| studio.account_role | role studio | 2 | Studio Admin | assignments | unique role_key | L | CFG | N | N | Y |
| studio.account_role_assignment | przypisanie roli | 2 | Studio Admin | account/role | assignment history | M | AL | N | Y | Y |
| studio.editorial_review | review redakcyjny | 2 | Studio | target | status check | M | CFG | N | Y | Y |
| studio.moderation_review | review moderacyjny | 2 | Moderation | target | status check | M | CFG | N | Y | Y |
| studio.moderation_decision | decyzja moderacyjna | 2 | Moderation | moderation_review | decision check | M | CFG | N | Y | Y |
| studio.studio_comment | komentarz operacyjny | 2 | Studio | target | - | M | CFG | N | Y | Y |
| studio.activation_change | historia aktywacji/rollback | 2 | Studio | target | action type check | M | CFG | N | Y | Y |
| studio.learning_recommendation | rekomendacja learning engine | 2 | Learning | target | recommendation status/type checks | M | CFG | N | N | Y |
| studio.recommendation_decision | decyzja człowieka | 2 | Studio | recommendation | decision status check | M | CFG | N | Y | Y |

### learning
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| learning.policy_definition | słownik polityk | 2 | Product/ML | versions | unique key | L | CFG | N | N | Y |
| learning.policy_version | wersja polityki | 2 | Product/ML | policy | unique(policy,version), one active | M | CFG | N | N | Y |
| learning.cohort_definition | słownik kohort | 2 | Learning | cohort versions | unique key | L | CFG | N | N | Y |
| learning.cohort_definition_version | wersja definicji kohorty | 2 | Learning | cohort | min sample check | M | CFG | N | N | Y |
| learning.candidate_performance_snapshot | agregaty kandydata | 2 | Learning | candidate_version | period checks | H | AGG | Y | N | Y |
| learning.reaction_performance_snapshot | agregaty reakcji | 2 | Learning | reaction_version | period checks | H | AGG | Y | N | Y |
| learning.cohort_candidate_performance_snapshot | agregaty per kohorta | 2 | Learning | cohort_version + candidate | period checks | H | AGG | Y | N | Y |
| learning.onboarding_funnel_snapshot | lejek onboardingu | 2 | Learning | policy_version | period checks | M | AGG | N | N | Y |
| learning.recovery_performance_snapshot | skuteczność recovery | 2 | Learning | strategy | period checks | M | AGG | N | N | Y |
| learning.prediction_outcome | predicted vs actual | 2 | Learning | selection_decision | - | H | CFG | Y | N | Y |

### audit
| table | purpose | tier | owner | key relations | key constraints | vol | ret | part | pii | restricted |
|---|---|---|---|---|---|---|---|---|---|---|
| audit.audit_event | append-oriented audit | 2 | Security | actor/target | indexed trail + DB trigger blocks UPDATE/DELETE | VH | LEG | Y | Y | Y |

## AE. Key Invariants
1. Multi-login dla jednego konta — DB+app (`identity.external_login`, `identity.password_credential`).
2. Provider subject unikalny globalnie — DB unique.
3. Hasło niejawne — DB contract (`password_hash` only) + app.
4. Token jako hash — DB contract (`identity.security_token.token_hash`) + app.
5. Pseudonim unikalny aktywnie — partial unique.
6. Akceptacja do wersji dokumentu — FK + unique.
7. Age context opcjonalny — nullable/optional rows.
8. Profession context opcjonalny — optional rows.
9. Skip optional context nie jest karą — logika aplikacyjna/scoring policy.
10. Age eligibility oddzielone od age context — osobne tabele.
11. Wiek/zawód nie tworzą cech DNA bez dowodów — logika modelu + `humor_evidence`.
12. Prezentacja wskazuje wersję karty — FK `candidate_presentation`.
13. Prezentacja wskazuje wersję reaction pack — FK `candidate_presentation`.
14. Ocena treści i reakcja oddzielne — `candidate_feedback` vs `reaction_choice`.
15. Klik reakcji i feedback puenty oddzielne — `reaction_choice`, `second_punchline_*`.
16. Wynik AI nie auto-approved — status checks `generated/awaiting_editorial_review`.
17. Istotna zmiana tworzy nową wersję — model `candidate_version` + reguła aplikacyjna.
18. Statystyki nie przepinane do nowej wersji — FK do version IDs.
19. Wycofana wersja nie powinna być wybierana — activation/status + app query filter.
20. Sesja może być wznowiona — `onboarding_session` pola resume.
21. Decyzja selekcji ma policy version — FK opcjonalny + app enforcement.
22. Cohort prior ≠ cecha usera — snapshot/cohort tables bez trwałej etykiety konta.
23. Initial DNA ma confidence — kolumny confidence.
24. Snapshot niemutowalny po publikacji — DB triggers na snapshot + dimensions + verdict.
25. Learning engine nie aktywuje bez człowieka — `studio.recommendation_decision` + proces.
26. Małe kohorty niepublikowane — `minimum_sample_size` + job analityczny.
27. Każda decyzja studio audytowalna — `audit.audit_event`.
28. Usunięcie kontekstu opcjonalnego nie usuwa legal records — oddzielne tabele i retencja.
29. AI execution bez sekretu/obrazu — model `ai_operation_execution`.
30. Event tables gotowe na skalę — indeksy + kandydaci do partycjonowania.

## AF. Deferred Areas
- pełny model feed/social,
- token vault dla provider access/refresh,
- RLS,
- partycjonowanie fizyczne,
- dedykowane słowniki bezpieczeństwa i policy engines,
- pełny long-term Humor DNA poza onboarding.

## AG. Open Product Decisions (max 25)
1. [OPEN PRODUCT DECISION] Finalny lifecycle konta i SLA transition.
2. [OPEN PRODUCT DECISION] Reguła zwalniania pseudonimu po expired/deleted.
3. [PRIVACY OR LEGAL REVIEW] Czy dokładna data urodzenia jest wymagana.
4. [OPEN PRODUCT DECISION] Finalne przedziały age_range.
5. [OPEN PRODUCT DECISION] Minimalny wiek platformy.
6. [OPEN PRODUCT DECISION] Definicja „istotnej zmiany” candidate version.
7. [OPEN PRODUCT DECISION] Czy jeden candidate może mieć >1 active reaction pack w eksperymencie.
8. [OPEN PRODUCT DECISION] Retencja raw AI output.
9. [OPEN PRODUCT DECISION] Retencja interaction events.
10. [OPEN PRODUCT DECISION] Operacyjna definicja Reaction Rescue Rate.
11. [OPEN PRODUCT DECISION] Docelowy katalog wymiarów Humor DNA.
12. [OPEN PRODUCT DECISION] Maksymalna waga cohort prior na starcie.
13. [OPEN PRODUCT DECISION] Reguła cohort mismatch i threshold.
14. [OPEN PRODUCT DECISION] Minimalna liczebność kohort publikowalnych.
15. [DEFERRED] Mechanizm tworzenia behavioral cohorts online/offline.
16. [OPEN PRODUCT DECISION] Zakres top-N alternatyw w explainability.
17. [OPEN PRODUCT DECISION] Rola i ograniczenia staff status dla Studio.
18. [PRIVACY OR LEGAL REVIEW] Pseudonimizacja eventów po usunięciu konta.
19. [PRIVACY OR LEGAL REVIEW] Zakres przechowywania source_ip/user_agent.
20. [DEFERRED] Potrzeba RLS i policy tenancy.
21. [OPEN PRODUCT DECISION] A/B strategy dla activation scope.
22. [OPEN PRODUCT DECISION] Zasady praw do memów onboardingowych.
23. [RECOMMENDED] Próg confidence dla automatycznego recovery trigger.
24. [RECOMMENDED] Katalog safety flag i escalation matrix.
25. [CONFIRMED] Age eligibility i age personalization pozostają rozdzielone.

## AH. Migration Recommendation
Rekomendowana kolejność atomowa:
1. Migration 001 — Identity and Registration.
2. Migration 002 — Optional Context and Onboarding State.
3. Migration 003 — Candidate Library and Reaction Infrastructure.
4. Migration 004 — Adaptive Presentation and Interaction Events.
5. Migration 005 — Working and Initial Humor DNA.
6. Migration 006 — AI Generation and Studio Workflow.
7. Migration 007 — Population Learning.
8. Migration 008 — Optional Calibration Steps.

## AI. Readiness Assessment
- Identity — READY FOR FIRST MIGRATION SPECIFICATION
- External Login — READY FOR FIRST MIGRATION SPECIFICATION
- Email Verification — READY FOR FIRST MIGRATION SPECIFICATION
- Legal Documents — READY FOR REVIEW
- Public Profile — READY FOR FIRST MIGRATION SPECIFICATION
- Profession Context — READY FOR REVIEW
- Age Eligibility — DDL DRAFT READY
- Age Context — DDL DRAFT READY
- Onboarding State — READY FOR REVIEW
- Candidate Library — DDL DRAFT READY
- Media References — DDL DRAFT READY
- Candidate Classification — DDL DRAFT READY
- Custom Reactions — DDL DRAFT READY
- Second Punchlines — DDL DRAFT READY
- AI Execution Records — DDL DRAFT READY
- Interaction Events — READY FOR REVIEW
- Working Humor DNA — DDL DRAFT READY
- Initial Humor Snapshot — DDL DRAFT READY
- Population Learning — DDL DRAFT READY
- Cohort Prior — DDL DRAFT READY
- Recovery — DDL DRAFT READY
- Studio — DDL DRAFT READY
- Moderation — DDL DRAFT READY
- Analytics — DDL DRAFT READY
- Optional Meme Calibration — CONCEPTUAL ONLY
- User Humor Inspiration — CONCEPTUAL ONLY

---
Walidacja wykonania:
- Syntax reviewed: TAK (manual review)
- Executed on disposable local database: NIE
- Status: NOT EXECUTED — PostgreSQL unavailable in bieżącym środowisku zadania
