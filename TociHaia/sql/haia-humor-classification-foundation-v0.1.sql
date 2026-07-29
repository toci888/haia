BEGIN;

-- ==========================================================
-- HAIA Humor Classification Foundation v0.1 (forward patch)
-- Scope: relational taxonomy, versioned classification model,
--        candidate classification revisions, safety, projection foundation.
-- ==========================================================

-- ----------------------------------------------------------
-- 1) TAXONOMY DICTIONARIES
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS humor.classification_axis (
	classification_axis_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	axis_key text NOT NULL,
	display_name text NOT NULL,
	description text,
	axis_type text NOT NULL,
	axis_role text NOT NULL,
	cardinality text NOT NULL,
	is_active boolean NOT NULL DEFAULT true,
	sort_order integer NOT NULL DEFAULT 100,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_classification_axis_axis_key UNIQUE (axis_key),
	CONSTRAINT ck_classification_axis_type CHECK (axis_type IN ('categorical','ordinal','scalar')),
	CONSTRAINT ck_classification_axis_role CHECK (axis_role IN ('affinity','context','routing','gating','analytics')),
	CONSTRAINT ck_classification_axis_cardinality CHECK (cardinality IN ('single','multi'))
);

COMMENT ON TABLE humor.classification_axis IS 'Słownik osi klasyfikacji humoru dla materiału (meme/joke).';
COMMENT ON COLUMN humor.classification_axis.axis_key IS 'Stabilny klucz techniczny osi (EN), używany w integracjach i seedach.';
COMMENT ON COLUMN humor.classification_axis.display_name IS 'Nazwa prezentacyjna osi (PL).';
COMMENT ON COLUMN humor.classification_axis.axis_type IS 'Typ sygnału osi: categorical/ordinal/scalar.';
COMMENT ON COLUMN humor.classification_axis.axis_role IS 'Rola osi w modelu: affinity/context/routing/gating/analytics.';
COMMENT ON COLUMN humor.classification_axis.cardinality IS 'Dopuszczalna liczba przypisań: single lub multi.';

CREATE TABLE IF NOT EXISTS humor.classification_value (
	classification_value_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	classification_axis_id uuid NOT NULL REFERENCES humor.classification_axis(classification_axis_id) ON DELETE RESTRICT,
	value_key text NOT NULL,
	display_name text NOT NULL,
	description text,
	ai_description text,
	editorial_guidance text,
	parent_classification_value_id uuid,
	sort_order integer NOT NULL DEFAULT 100,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_classification_value_axis_value_key UNIQUE (classification_axis_id, value_key),
	CONSTRAINT uq_classification_value_id_axis UNIQUE (classification_value_id, classification_axis_id)
);

COMMENT ON TABLE humor.classification_value IS 'Słownik wartości dla osi klasyfikacji humoru.';
COMMENT ON COLUMN humor.classification_value.value_key IS 'Stabilny klucz techniczny wartości (EN).';
COMMENT ON COLUMN humor.classification_value.parent_classification_value_id IS 'Opcjonalna hierarchia wartości (parent-child).';
COMMENT ON COLUMN humor.classification_value.ai_description IS 'Krótka wskazówka semantyczna dla pipeline AI.';
COMMENT ON COLUMN humor.classification_value.editorial_guidance IS 'Krótka wskazówka redakcyjna do review klasyfikacji.';

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1
		FROM pg_constraint
		WHERE conname = 'fk_classification_value_parent_same_axis'
		AND conrelid = 'humor.classification_value'::regclass
	) THEN
		ALTER TABLE humor.classification_value
			ADD CONSTRAINT fk_classification_value_parent_same_axis
			FOREIGN KEY (parent_classification_value_id, classification_axis_id)
			REFERENCES humor.classification_value(classification_value_id, classification_axis_id)
			ON DELETE RESTRICT;
	END IF;
END $$;

CREATE INDEX IF NOT EXISTS ix_classification_axis_key_active
	ON humor.classification_axis(axis_key)
	WHERE is_active = true;

CREATE INDEX IF NOT EXISTS ix_classification_value_axis_sort
	ON humor.classification_value(classification_axis_id, sort_order, value_key)
	WHERE is_active = true;

CREATE INDEX IF NOT EXISTS ix_classification_value_key
	ON humor.classification_value(value_key);

-- ----------------------------------------------------------
-- 2) VERSIONED CLASSIFICATION MODEL
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS humor.classification_model_version (
	classification_model_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	model_key text NOT NULL,
	version_no integer NOT NULL,
	version_label text NOT NULL,
	status text NOT NULL,
	description text,
	config jsonb NOT NULL DEFAULT '{}'::jsonb,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	activated_at timestamptz,
	retired_at timestamptz,
	CONSTRAINT uq_classification_model_key_version UNIQUE (model_key, version_no),
	CONSTRAINT ck_classification_model_version_no CHECK (version_no > 0),
	CONSTRAINT ck_classification_model_status CHECK (status IN ('draft','active','retired','archived')),
	CONSTRAINT ck_classification_model_active_ts CHECK (status <> 'active' OR activated_at IS NOT NULL)
);

COMMENT ON TABLE humor.classification_model_version IS 'Wersjonowany model klasyfikacji humoru materiału.';
COMMENT ON COLUMN humor.classification_model_version.config IS 'Konfiguracja niestabilna/eksperymentalna modelu (JSONB).';

CREATE UNIQUE INDEX IF NOT EXISTS ux_classification_model_one_active_per_key
	ON humor.classification_model_version(model_key)
	WHERE status = 'active';

CREATE INDEX IF NOT EXISTS ix_classification_model_key_status
	ON humor.classification_model_version(model_key, status, version_no DESC);

CREATE TABLE IF NOT EXISTS humor.classification_model_axis (
	classification_model_axis_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	classification_model_version_id uuid NOT NULL REFERENCES humor.classification_model_version(classification_model_version_id) ON DELETE CASCADE,
	classification_axis_id uuid NOT NULL REFERENCES humor.classification_axis(classification_axis_id) ON DELETE RESTRICT,
	axis_weight numeric(8,6) NOT NULL DEFAULT 1.0,
	is_required boolean NOT NULL DEFAULT false,
	minimum_assignments integer NOT NULL DEFAULT 0,
	maximum_assignments integer,
	normalization_method text NOT NULL DEFAULT 'none',
	member_status text NOT NULL DEFAULT 'active',
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_classification_model_axis UNIQUE (classification_model_version_id, classification_axis_id),
	CONSTRAINT uq_classification_model_axis_id_model UNIQUE (classification_model_axis_id, classification_model_version_id),
	CONSTRAINT uq_classification_model_axis_triplet UNIQUE (classification_model_axis_id, classification_model_version_id, classification_axis_id),
	CONSTRAINT ck_classification_model_axis_weight CHECK (axis_weight >= 0),
	CONSTRAINT ck_classification_model_axis_min CHECK (minimum_assignments >= 0),
	CONSTRAINT ck_classification_model_axis_max CHECK (maximum_assignments IS NULL OR maximum_assignments > 0),
	CONSTRAINT ck_classification_model_axis_min_max CHECK (maximum_assignments IS NULL OR minimum_assignments <= maximum_assignments),
	CONSTRAINT ck_classification_model_axis_norm CHECK (normalization_method IN ('none','linear_0_1','ranked','weighted_multi')),
	CONSTRAINT ck_classification_model_axis_status CHECK (member_status IN ('active','retired','experimental'))
);

COMMENT ON TABLE humor.classification_model_axis IS 'Członkostwo osi w konkretnej wersji modelu wraz z wagą osi.';
COMMENT ON COLUMN humor.classification_model_axis.axis_weight IS 'Waga osi na poziomie wersji modelu (globalna ważność sygnału).';
COMMENT ON COLUMN humor.classification_model_axis.normalization_method IS 'Metoda normalizacji agregacji przypisań osi.';

CREATE INDEX IF NOT EXISTS ix_classification_model_axis_model
	ON humor.classification_model_axis(classification_model_version_id, member_status, classification_axis_id);

CREATE TABLE IF NOT EXISTS humor.classification_model_value (
	classification_model_value_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	classification_model_version_id uuid NOT NULL,
	classification_model_axis_id uuid NOT NULL,
	classification_axis_id uuid NOT NULL,
	classification_value_id uuid NOT NULL,
	default_weight numeric(8,6) NOT NULL DEFAULT 1.0,
	is_enabled boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_classification_model_value_model_value UNIQUE (classification_model_version_id, classification_value_id),
	CONSTRAINT uq_classification_model_value_id_model UNIQUE (classification_model_value_id, classification_model_version_id),
	CONSTRAINT uq_classification_model_value_id_model_axis UNIQUE (classification_model_value_id, classification_model_version_id, classification_model_axis_id),
	CONSTRAINT ck_classification_model_value_weight CHECK (default_weight >= 0)
);

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_model_value_model_axis_triplet'
		AND conrelid = 'humor.classification_model_value'::regclass
	) THEN
		ALTER TABLE humor.classification_model_value
			ADD CONSTRAINT fk_classification_model_value_model_axis_triplet
			FOREIGN KEY (classification_model_axis_id, classification_model_version_id, classification_axis_id)
			REFERENCES humor.classification_model_axis(classification_model_axis_id, classification_model_version_id, classification_axis_id)
			ON DELETE CASCADE;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_model_value_axis_value'
		AND conrelid = 'humor.classification_model_value'::regclass
	) THEN
		ALTER TABLE humor.classification_model_value
			ADD CONSTRAINT fk_classification_model_value_axis_value
			FOREIGN KEY (classification_value_id, classification_axis_id)
			REFERENCES humor.classification_value(classification_value_id, classification_axis_id)
			ON DELETE RESTRICT;
	END IF;
END $$;

COMMENT ON TABLE humor.classification_model_value IS 'Aktywacja wartości słownikowej w wersji modelu wraz z wagą domyślną wartości.';
COMMENT ON COLUMN humor.classification_model_value.default_weight IS 'Domyślna waga wartości (odrębna od wagi osi, relevance i confidence).';

CREATE INDEX IF NOT EXISTS ix_classification_model_value_model_axis
	ON humor.classification_model_value(classification_model_version_id, classification_model_axis_id)
	WHERE is_enabled = true;

CREATE INDEX IF NOT EXISTS ix_classification_model_value_lookup
	ON humor.classification_model_value(classification_model_value_id, classification_model_version_id);

-- Cardinality/type guard for model axis.
CREATE OR REPLACE FUNCTION humor.trg_classification_model_axis_validate()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_cardinality text;
	v_axis_type text;
BEGIN
	SELECT a.cardinality, a.axis_type
	INTO v_cardinality, v_axis_type
	FROM humor.classification_axis a
	WHERE a.classification_axis_id = NEW.classification_axis_id;

	IF v_cardinality = 'single' AND NEW.maximum_assignments IS NOT NULL AND NEW.maximum_assignments > 1 THEN
		RAISE EXCEPTION 'single-cardinality axis cannot allow more than one assignment';
	END IF;

	IF v_axis_type IN ('scalar','ordinal') THEN
		IF NEW.maximum_assignments IS NOT NULL AND NEW.maximum_assignments > 1 THEN
			RAISE EXCEPTION 'scalar/ordinal axis cannot allow multiple assignments in model axis';
		END IF;
		IF NEW.minimum_assignments > 1 THEN
			RAISE EXCEPTION 'scalar/ordinal axis cannot require more than one assignment';
		END IF;
	END IF;

	RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trg_classification_model_axis_validate ON humor.classification_model_axis;
CREATE TRIGGER trg_classification_model_axis_validate
	BEFORE INSERT OR UPDATE ON humor.classification_model_axis
	FOR EACH ROW EXECUTE FUNCTION humor.trg_classification_model_axis_validate();

-- ----------------------------------------------------------
-- 3) SAFETY DICTIONARY
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS humor.sensitivity_category (
	sensitivity_category_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	category_key text NOT NULL UNIQUE,
	display_name text NOT NULL,
	description text,
	is_active boolean NOT NULL DEFAULT true,
	sort_order integer NOT NULL DEFAULT 100,
	created_at timestamptz NOT NULL DEFAULT now()
);

COMMENT ON TABLE humor.sensitivity_category IS 'Słownik kategorii wrażliwości/safety dla materiału.';
COMMENT ON COLUMN humor.sensitivity_category.category_key IS 'Stabilny klucz kategorii safety (EN).';

CREATE INDEX IF NOT EXISTS ix_sensitivity_category_active
	ON humor.sensitivity_category(category_key)
	WHERE is_active = true;

-- ----------------------------------------------------------
-- 4) VERSIONED CANDIDATE CLASSIFICATION EXTENSION
-- ----------------------------------------------------------

ALTER TABLE onboarding.candidate_classification
	ADD COLUMN IF NOT EXISTS classification_model_version_id uuid,
	ADD COLUMN IF NOT EXISTS revision_no integer,
	ADD COLUMN IF NOT EXISTS source_type text,
	ADD COLUMN IF NOT EXISTS classification_status text,
	ADD COLUMN IF NOT EXISTS overall_confidence numeric(6,5),
	ADD COLUMN IF NOT EXISTS ai_operation_execution_id uuid,
	ADD COLUMN IF NOT EXISTS created_by_account_id uuid,
	ADD COLUMN IF NOT EXISTS reviewed_by_account_id uuid,
	ADD COLUMN IF NOT EXISTS supersedes_candidate_classification_id uuid,
	ADD COLUMN IF NOT EXISTS editorial_note text,
	ADD COLUMN IF NOT EXISTS created_at timestamptz,
	ADD COLUMN IF NOT EXISTS reviewed_at timestamptz,
	ADD COLUMN IF NOT EXISTS published_at timestamptz,
	ADD COLUMN IF NOT EXISTS superseded_at timestamptz,
	ADD COLUMN IF NOT EXISTS predicted_dryness_scale_level_id uuid,
	ADD COLUMN IF NOT EXISTS predicted_dryness_confidence numeric(6,5);

COMMENT ON COLUMN onboarding.candidate_classification.intensity IS 'LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=intensity).';
COMMENT ON COLUMN onboarding.candidate_classification.complexity IS 'LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=complexity).';
COMMENT ON COLUMN onboarding.candidate_classification.universality_score IS 'LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=universality).';
COMMENT ON COLUMN onboarding.candidate_classification.reaction_rescue_potential IS 'LEGACY: zastępowane przez onboarding.candidate_classification_measure (axis=rescue_potential).';
COMMENT ON COLUMN onboarding.candidate_classification.dryness IS 'LEGACY/DEPRECATED: zastępowane przez predicted_dryness_scale_level_id + predicted_dryness_confidence.';
COMMENT ON COLUMN onboarding.candidate_classification.age_hint_strength IS 'LEGACY: hint kontekstowy, nie jest observed evidence i nie powinien potwierdzać Humor DNA.';
COMMENT ON COLUMN onboarding.candidate_classification.profession_hint_strength IS 'LEGACY: hint kontekstowy, nie jest observed evidence i nie powinien potwierdzać Humor DNA.';
COMMENT ON COLUMN onboarding.candidate_classification.safety_flags IS 'LEGACY/raw AI payload. Kanoniczny safety model jest relacyjny w candidate_classification_sensitivity.';
COMMENT ON COLUMN onboarding.candidate_classification.additional_tags IS 'Pozostaje dla rzadkich/eksperymentalnych metadanych (nie kanoniczna taksonomia).';
COMMENT ON COLUMN onboarding.candidate_classification.predicted_dryness_scale_level_id IS 'Predykcja poziomu Sucharka dla materiału (nie mylić z realną oceną usera w humor.dryness_rating).';

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_candidate_classification_model_version'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT fk_candidate_classification_model_version
			FOREIGN KEY (classification_model_version_id)
			REFERENCES humor.classification_model_version(classification_model_version_id)
			ON DELETE RESTRICT;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_candidate_classification_ai_execution'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT fk_candidate_classification_ai_execution
			FOREIGN KEY (ai_operation_execution_id)
			REFERENCES ai.ai_operation_execution(ai_operation_execution_id)
			ON DELETE SET NULL;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_candidate_classification_created_by'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT fk_candidate_classification_created_by
			FOREIGN KEY (created_by_account_id)
			REFERENCES identity.account(account_id)
			ON DELETE SET NULL;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_candidate_classification_reviewed_by'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT fk_candidate_classification_reviewed_by
			FOREIGN KEY (reviewed_by_account_id)
			REFERENCES identity.account(account_id)
			ON DELETE SET NULL;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_candidate_classification_supersedes'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT fk_candidate_classification_supersedes
			FOREIGN KEY (supersedes_candidate_classification_id)
			REFERENCES onboarding.candidate_classification(candidate_classification_id)
			ON DELETE RESTRICT;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_candidate_classification_predicted_dryness'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT fk_candidate_classification_predicted_dryness
			FOREIGN KEY (predicted_dryness_scale_level_id)
			REFERENCES humor.dryness_scale_level(dryness_scale_level_id)
			ON DELETE SET NULL;
	END IF;
END $$;

DO $$
DECLARE
	r record;
BEGIN
	-- Remove only legacy unique that blocks history: UNIQUE(onboarding_candidate_version_id)
	FOR r IN
		SELECT c.conname
		FROM pg_constraint c
		JOIN pg_class t ON t.oid = c.conrelid
		JOIN pg_namespace n ON n.oid = t.relnamespace
		JOIN unnest(c.conkey) WITH ORDINALITY AS cols(attnum, ord) ON true
		JOIN pg_attribute a ON a.attrelid = t.oid AND a.attnum = cols.attnum
		WHERE n.nspname = 'onboarding'
		AND t.relname = 'candidate_classification'
		AND c.contype = 'u'
		GROUP BY c.conname
		HAVING COUNT(*) = 1
		AND max(a.attname) = 'onboarding_candidate_version_id'
	LOOP
		EXECUTE format('ALTER TABLE onboarding.candidate_classification DROP CONSTRAINT IF EXISTS %I', r.conname);
	END LOOP;
END $$;

CREATE UNIQUE INDEX IF NOT EXISTS ux_candidate_classification_candidate_revision
	ON onboarding.candidate_classification(onboarding_candidate_version_id, revision_no);

CREATE UNIQUE INDEX IF NOT EXISTS ux_candidate_classification_one_published_per_candidate_version
	ON onboarding.candidate_classification(onboarding_candidate_version_id)
	WHERE classification_status = 'published';

CREATE UNIQUE INDEX IF NOT EXISTS ux_candidate_classification_id_model
	ON onboarding.candidate_classification(candidate_classification_id, classification_model_version_id);

CREATE INDEX IF NOT EXISTS ix_candidate_classification_candidate_status
	ON onboarding.candidate_classification(onboarding_candidate_version_id, classification_status, revision_no DESC);

CREATE INDEX IF NOT EXISTS ix_candidate_classification_model_status
	ON onboarding.candidate_classification(classification_model_version_id, classification_status, created_at DESC);

CREATE INDEX IF NOT EXISTS ix_candidate_classification_ai_execution
	ON onboarding.candidate_classification(ai_operation_execution_id)
	WHERE ai_operation_execution_id IS NOT NULL;

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_source_type'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_source_type
			CHECK (source_type IN ('ai','human','ai_editorial','imported','system_migration'));
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_status'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_status
			CHECK (classification_status IN ('generated','awaiting_review','approved','published','superseded','rejected'));
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_revision'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_revision
			CHECK (revision_no IS NULL OR revision_no > 0);
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_overall_confidence'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_overall_confidence
			CHECK (overall_confidence IS NULL OR overall_confidence BETWEEN 0 AND 1);
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_predicted_dryness_confidence'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_predicted_dryness_confidence
			CHECK (predicted_dryness_confidence IS NULL OR predicted_dryness_confidence BETWEEN 0 AND 1);
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_supersedes_not_self'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_supersedes_not_self
			CHECK (
				supersedes_candidate_classification_id IS NULL
				OR supersedes_candidate_classification_id <> candidate_classification_id
			);
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'ck_candidate_classification_status_timestamps'
		AND conrelid = 'onboarding.candidate_classification'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification
			ADD CONSTRAINT ck_candidate_classification_status_timestamps
			CHECK (
				(classification_status <> 'published' OR published_at IS NOT NULL)
				AND (classification_status <> 'superseded' OR superseded_at IS NOT NULL)
				AND (reviewed_at IS NULL OR created_at IS NULL OR reviewed_at >= created_at)
				AND (published_at IS NULL OR created_at IS NULL OR published_at >= created_at)
				AND (superseded_at IS NULL OR created_at IS NULL OR superseded_at >= created_at)
			);
	END IF;
END $$;

-- Supersession and publication guards.
CREATE OR REPLACE FUNCTION onboarding.trg_candidate_classification_validate_row()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_superseded_candidate_version_id uuid;
BEGIN
	IF NEW.supersedes_candidate_classification_id IS NOT NULL THEN
		SELECT c.onboarding_candidate_version_id
		INTO v_superseded_candidate_version_id
		FROM onboarding.candidate_classification c
		WHERE c.candidate_classification_id = NEW.supersedes_candidate_classification_id;

		IF v_superseded_candidate_version_id IS NULL THEN
			RAISE EXCEPTION 'superseded candidate_classification does not exist';
		END IF;

		IF v_superseded_candidate_version_id <> NEW.onboarding_candidate_version_id THEN
			RAISE EXCEPTION 'superseded candidate_classification must belong to the same onboarding_candidate_version_id';
		END IF;
	END IF;

	IF NEW.classification_status = 'published' AND NEW.published_at IS NULL THEN
		RAISE EXCEPTION 'published classification requires published_at';
	END IF;

	IF NEW.classification_status = 'superseded' AND NEW.superseded_at IS NULL THEN
		RAISE EXCEPTION 'superseded classification requires superseded_at';
	END IF;

	RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trg_candidate_classification_validate_row ON onboarding.candidate_classification;
CREATE TRIGGER trg_candidate_classification_validate_row
	BEFORE INSERT OR UPDATE ON onboarding.candidate_classification
	FOR EACH ROW EXECUTE FUNCTION onboarding.trg_candidate_classification_validate_row();

CREATE OR REPLACE FUNCTION onboarding.trg_candidate_classification_immutability()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
	IF TG_OP = 'DELETE' THEN
		IF OLD.classification_status = 'published' THEN
			RAISE EXCEPTION 'published candidate_classification cannot be deleted';
		END IF;
		RETURN OLD;
	END IF;

	IF OLD.classification_status = 'published' THEN
		IF NEW.classification_status <> 'superseded' THEN
			RAISE EXCEPTION 'published candidate_classification can only transition to superseded';
		END IF;
		IF NEW.superseded_at IS NULL THEN
			RAISE EXCEPTION 'superseding published classification requires superseded_at';
		END IF;

		IF (
			(to_jsonb(NEW) - 'classification_status' - 'superseded_at')
			IS DISTINCT FROM
			(to_jsonb(OLD) - 'classification_status' - 'superseded_at')
		) THEN
			RAISE EXCEPTION 'published candidate_classification immutable fields cannot be edited';
		END IF;
	END IF;

	RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trg_candidate_classification_immutability ON onboarding.candidate_classification;
CREATE TRIGGER trg_candidate_classification_immutability
	BEFORE UPDATE OR DELETE ON onboarding.candidate_classification
	FOR EACH ROW EXECUTE FUNCTION onboarding.trg_candidate_classification_immutability();

-- ----------------------------------------------------------
-- 5) CATEGORICAL ASSIGNMENTS
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS onboarding.candidate_classification_value (
	candidate_classification_value_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_classification_id uuid NOT NULL,
	classification_model_version_id uuid NOT NULL,
	classification_model_axis_id uuid NOT NULL,
	classification_model_value_id uuid NOT NULL,
	relevance_score numeric(6,5) NOT NULL,
	confidence numeric(6,5),
	is_primary boolean NOT NULL DEFAULT false,
	rank_no integer,
	assignment_source text NOT NULL,
	editorial_note text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_candidate_classification_value_value UNIQUE (candidate_classification_id, classification_model_value_id),
	CONSTRAINT ck_candidate_classification_value_relevance CHECK (relevance_score BETWEEN 0 AND 1),
	CONSTRAINT ck_candidate_classification_value_confidence CHECK (confidence IS NULL OR confidence BETWEEN 0 AND 1),
	CONSTRAINT ck_candidate_classification_value_rank CHECK (rank_no IS NULL OR rank_no > 0),
	CONSTRAINT ck_candidate_classification_value_source CHECK (assignment_source IN ('ai','human','ai_editorial','imported','system_migration'))
);

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_ccv_parent_classification_model'
		AND conrelid = 'onboarding.candidate_classification_value'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification_value
			ADD CONSTRAINT fk_ccv_parent_classification_model
			FOREIGN KEY (candidate_classification_id, classification_model_version_id)
			REFERENCES onboarding.candidate_classification(candidate_classification_id, classification_model_version_id)
			ON DELETE CASCADE;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_ccv_model_axis'
		AND conrelid = 'onboarding.candidate_classification_value'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification_value
			ADD CONSTRAINT fk_ccv_model_axis
			FOREIGN KEY (classification_model_axis_id, classification_model_version_id)
			REFERENCES humor.classification_model_axis(classification_model_axis_id, classification_model_version_id)
			ON DELETE RESTRICT;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_ccv_model_value'
		AND conrelid = 'onboarding.candidate_classification_value'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification_value
			ADD CONSTRAINT fk_ccv_model_value
			FOREIGN KEY (classification_model_value_id, classification_model_version_id, classification_model_axis_id)
			REFERENCES humor.classification_model_value(classification_model_value_id, classification_model_version_id, classification_model_axis_id)
			ON DELETE RESTRICT;
	END IF;
END $$;

CREATE UNIQUE INDEX IF NOT EXISTS ux_candidate_classification_value_primary_per_axis
	ON onboarding.candidate_classification_value(candidate_classification_id, classification_model_axis_id)
	WHERE is_primary = true;

CREATE INDEX IF NOT EXISTS ix_candidate_classification_value_classification
	ON onboarding.candidate_classification_value(candidate_classification_id, classification_model_axis_id, relevance_score DESC);

CREATE INDEX IF NOT EXISTS ix_candidate_classification_value_model_value
	ON onboarding.candidate_classification_value(classification_model_value_id, relevance_score DESC, confidence DESC);

-- ----------------------------------------------------------
-- 6) NUMERIC MEASURES
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS onboarding.candidate_classification_measure (
	candidate_classification_measure_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_classification_id uuid NOT NULL,
	classification_model_version_id uuid NOT NULL,
	classification_model_axis_id uuid NOT NULL,
	normalized_value numeric(6,5) NOT NULL,
	confidence numeric(6,5),
	measurement_source text NOT NULL,
	editorial_note text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_candidate_classification_measure_axis UNIQUE (candidate_classification_id, classification_model_axis_id),
	CONSTRAINT ck_candidate_classification_measure_value CHECK (normalized_value BETWEEN 0 AND 1),
	CONSTRAINT ck_candidate_classification_measure_confidence CHECK (confidence IS NULL OR confidence BETWEEN 0 AND 1),
	CONSTRAINT ck_candidate_classification_measure_source CHECK (measurement_source IN ('ai','human','ai_editorial','imported','system_migration'))
);

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_ccm_parent_classification_model'
		AND conrelid = 'onboarding.candidate_classification_measure'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification_measure
			ADD CONSTRAINT fk_ccm_parent_classification_model
			FOREIGN KEY (candidate_classification_id, classification_model_version_id)
			REFERENCES onboarding.candidate_classification(candidate_classification_id, classification_model_version_id)
			ON DELETE CASCADE;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_ccm_model_axis'
		AND conrelid = 'onboarding.candidate_classification_measure'::regclass
	) THEN
		ALTER TABLE onboarding.candidate_classification_measure
			ADD CONSTRAINT fk_ccm_model_axis
			FOREIGN KEY (classification_model_axis_id, classification_model_version_id)
			REFERENCES humor.classification_model_axis(classification_model_axis_id, classification_model_version_id)
			ON DELETE RESTRICT;
	END IF;
END $$;

CREATE INDEX IF NOT EXISTS ix_candidate_classification_measure_classification
	ON onboarding.candidate_classification_measure(candidate_classification_id, classification_model_axis_id);

CREATE INDEX IF NOT EXISTS ix_candidate_classification_measure_axis_value
	ON onboarding.candidate_classification_measure(classification_model_axis_id, normalized_value);

CREATE OR REPLACE FUNCTION onboarding.trg_candidate_classification_measure_validate_axis_type()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_axis_type text;
BEGIN
	SELECT a.axis_type
	INTO v_axis_type
	FROM humor.classification_model_axis ma
	JOIN humor.classification_axis a ON a.classification_axis_id = ma.classification_axis_id
	WHERE ma.classification_model_axis_id = NEW.classification_model_axis_id
	AND ma.classification_model_version_id = NEW.classification_model_version_id;

	IF v_axis_type IS NULL THEN
		RAISE EXCEPTION 'classification model axis not found for measure row';
	END IF;

	IF v_axis_type NOT IN ('scalar','ordinal') THEN
		RAISE EXCEPTION 'candidate_classification_measure supports only scalar/ordinal axes';
	END IF;

	RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trg_candidate_classification_measure_validate_axis_type ON onboarding.candidate_classification_measure;
CREATE TRIGGER trg_candidate_classification_measure_validate_axis_type
	BEFORE INSERT OR UPDATE ON onboarding.candidate_classification_measure
	FOR EACH ROW EXECUTE FUNCTION onboarding.trg_candidate_classification_measure_validate_axis_type();

-- ----------------------------------------------------------
-- 7) RELATIONAL SAFETY ASSIGNMENTS
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS onboarding.candidate_classification_sensitivity (
	candidate_classification_sensitivity_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_classification_id uuid NOT NULL REFERENCES onboarding.candidate_classification(candidate_classification_id) ON DELETE CASCADE,
	sensitivity_category_id uuid NOT NULL REFERENCES humor.sensitivity_category(sensitivity_category_id) ON DELETE RESTRICT,
	severity_level smallint NOT NULL,
	confidence numeric(6,5),
	moderation_relevance boolean NOT NULL DEFAULT true,
	assignment_source text NOT NULL,
	editorial_note text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_candidate_classification_sensitivity UNIQUE (candidate_classification_id, sensitivity_category_id),
	CONSTRAINT ck_candidate_classification_sensitivity_severity CHECK (severity_level BETWEEN 0 AND 3),
	CONSTRAINT ck_candidate_classification_sensitivity_confidence CHECK (confidence IS NULL OR confidence BETWEEN 0 AND 1),
	CONSTRAINT ck_candidate_classification_sensitivity_source CHECK (assignment_source IN ('ai','human','ai_editorial','imported','system_migration'))
);

CREATE INDEX IF NOT EXISTS ix_candidate_classification_sensitivity_moderation
	ON onboarding.candidate_classification_sensitivity(severity_level DESC, moderation_relevance)
	WHERE moderation_relevance = true;

CREATE INDEX IF NOT EXISTS ix_candidate_classification_sensitivity_classification
	ON onboarding.candidate_classification_sensitivity(candidate_classification_id, severity_level DESC);

-- ----------------------------------------------------------
-- 8) CHILD IMMUTABILITY AFTER PUBLICATION
-- ----------------------------------------------------------

CREATE OR REPLACE FUNCTION onboarding.assert_classification_not_published(p_candidate_classification_id uuid)
RETURNS void
LANGUAGE plpgsql
AS $$
DECLARE
	v_status text;
BEGIN
	SELECT c.classification_status
	INTO v_status
	FROM onboarding.candidate_classification c
	WHERE c.candidate_classification_id = p_candidate_classification_id;

	IF v_status = 'published' THEN
		RAISE EXCEPTION 'child rows cannot be modified for published candidate_classification %', p_candidate_classification_id;
	END IF;
END;
$$;

CREATE OR REPLACE FUNCTION onboarding.trg_child_immutable_when_published()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_candidate_classification_id uuid;
BEGIN
	v_candidate_classification_id := COALESCE(NEW.candidate_classification_id, OLD.candidate_classification_id);
	PERFORM onboarding.assert_classification_not_published(v_candidate_classification_id);
	RETURN COALESCE(NEW, OLD);
END;
$$;

DROP TRIGGER IF EXISTS trg_ccv_immutable_when_published ON onboarding.candidate_classification_value;
CREATE TRIGGER trg_ccv_immutable_when_published
	BEFORE INSERT OR UPDATE OR DELETE ON onboarding.candidate_classification_value
	FOR EACH ROW EXECUTE FUNCTION onboarding.trg_child_immutable_when_published();

DROP TRIGGER IF EXISTS trg_ccm_immutable_when_published ON onboarding.candidate_classification_measure;
CREATE TRIGGER trg_ccm_immutable_when_published
	BEFORE INSERT OR UPDATE OR DELETE ON onboarding.candidate_classification_measure
	FOR EACH ROW EXECUTE FUNCTION onboarding.trg_child_immutable_when_published();

DROP TRIGGER IF EXISTS trg_ccs_immutable_when_published ON onboarding.candidate_classification_sensitivity;
CREATE TRIGGER trg_ccs_immutable_when_published
	BEFORE INSERT OR UPDATE OR DELETE ON onboarding.candidate_classification_sensitivity
	FOR EACH ROW EXECUTE FUNCTION onboarding.trg_child_immutable_when_published();

-- ----------------------------------------------------------
-- 9) PROJECTION FOUNDATION TO HUMOR DNA
-- ----------------------------------------------------------

CREATE TABLE IF NOT EXISTS humor.classification_projection_model_version (
	classification_projection_model_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	projection_model_key text NOT NULL,
	version_no integer NOT NULL,
	status text NOT NULL,
	classification_model_version_id uuid NOT NULL REFERENCES humor.classification_model_version(classification_model_version_id) ON DELETE RESTRICT,
	humor_dimension_model_version_id uuid NOT NULL REFERENCES humor.humor_dimension_model_version(humor_dimension_model_version_id) ON DELETE RESTRICT,
	description text,
	config jsonb NOT NULL DEFAULT '{}'::jsonb,
	created_at timestamptz NOT NULL DEFAULT now(),
	activated_at timestamptz,
	retired_at timestamptz,
	CONSTRAINT uq_classification_projection_model_key_version UNIQUE (projection_model_key, version_no),
	CONSTRAINT uq_classification_projection_model_version_triplet UNIQUE (
		classification_projection_model_version_id,
		classification_model_version_id,
		humor_dimension_model_version_id
	),
	CONSTRAINT ck_classification_projection_model_version_no CHECK (version_no > 0),
	CONSTRAINT ck_classification_projection_status CHECK (status IN ('draft','active','retired','archived')),
	CONSTRAINT ck_classification_projection_active_ts CHECK (status <> 'active' OR activated_at IS NOT NULL)
);

CREATE UNIQUE INDEX IF NOT EXISTS ux_classification_projection_one_active_per_key
	ON humor.classification_projection_model_version(projection_model_key)
	WHERE status = 'active';

CREATE INDEX IF NOT EXISTS ix_classification_projection_lookup
	ON humor.classification_projection_model_version(projection_model_key, status, version_no DESC);

CREATE TABLE IF NOT EXISTS humor.classification_value_projection_rule (
	classification_value_projection_rule_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	classification_projection_model_version_id uuid NOT NULL,
	classification_model_version_id uuid NOT NULL,
	humor_dimension_model_version_id uuid NOT NULL,
	classification_model_value_id uuid NOT NULL,
	humor_dimension_model_member_id uuid NOT NULL,
	effect_weight numeric(7,6) NOT NULL,
	confidence numeric(6,5),
	is_enabled boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_classification_value_projection_rule UNIQUE (
		classification_projection_model_version_id,
		classification_model_value_id,
		humor_dimension_model_member_id
	),
	CONSTRAINT ck_classification_value_projection_effect_weight CHECK (effect_weight BETWEEN -1 AND 1),
	CONSTRAINT ck_classification_value_projection_confidence CHECK (confidence IS NULL OR confidence BETWEEN 0 AND 1)
);

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_value_projection_model_triplet'
		AND conrelid = 'humor.classification_value_projection_rule'::regclass
	) THEN
		ALTER TABLE humor.classification_value_projection_rule
			ADD CONSTRAINT fk_classification_value_projection_model_triplet
			FOREIGN KEY (
				classification_projection_model_version_id,
				classification_model_version_id,
				humor_dimension_model_version_id
			)
			REFERENCES humor.classification_projection_model_version(
				classification_projection_model_version_id,
				classification_model_version_id,
				humor_dimension_model_version_id
			)
			ON DELETE CASCADE;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_value_projection_model_value'
		AND conrelid = 'humor.classification_value_projection_rule'::regclass
	) THEN
		ALTER TABLE humor.classification_value_projection_rule
			ADD CONSTRAINT fk_classification_value_projection_model_value
			FOREIGN KEY (classification_model_value_id, classification_model_version_id)
			REFERENCES humor.classification_model_value(classification_model_value_id, classification_model_version_id)
			ON DELETE RESTRICT;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_value_projection_dimension_member'
		AND conrelid = 'humor.classification_value_projection_rule'::regclass
	) THEN
		ALTER TABLE humor.classification_value_projection_rule
			ADD CONSTRAINT fk_classification_value_projection_dimension_member
			FOREIGN KEY (humor_dimension_model_member_id, humor_dimension_model_version_id)
			REFERENCES humor.humor_dimension_model_member(humor_dimension_model_member_id, humor_dimension_model_version_id)
			ON DELETE RESTRICT;
	END IF;
END $$;

CREATE INDEX IF NOT EXISTS ix_classification_value_projection_enabled
	ON humor.classification_value_projection_rule(classification_projection_model_version_id, is_enabled)
	WHERE is_enabled = true;

CREATE TABLE IF NOT EXISTS humor.classification_measure_projection_rule (
	classification_measure_projection_rule_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	classification_projection_model_version_id uuid NOT NULL,
	classification_model_version_id uuid NOT NULL,
	humor_dimension_model_version_id uuid NOT NULL,
	classification_model_axis_id uuid NOT NULL,
	humor_dimension_model_member_id uuid NOT NULL,
	effect_weight numeric(7,6) NOT NULL,
	mapping_mode text NOT NULL,
	center_value numeric(6,5),
	confidence numeric(6,5),
	is_enabled boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	CONSTRAINT uq_classification_measure_projection_rule UNIQUE (
		classification_projection_model_version_id,
		classification_model_axis_id,
		humor_dimension_model_member_id
	),
	CONSTRAINT ck_classification_measure_projection_effect_weight CHECK (effect_weight BETWEEN -1 AND 1),
	CONSTRAINT ck_classification_measure_projection_center_value CHECK (center_value IS NULL OR center_value BETWEEN 0 AND 1),
	CONSTRAINT ck_classification_measure_projection_confidence CHECK (confidence IS NULL OR confidence BETWEEN 0 AND 1),
	CONSTRAINT ck_classification_measure_projection_mode CHECK (mapping_mode IN ('linear','centered_linear','threshold'))
);

DO $$
BEGIN
	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_measure_projection_model_triplet'
		AND conrelid = 'humor.classification_measure_projection_rule'::regclass
	) THEN
		ALTER TABLE humor.classification_measure_projection_rule
			ADD CONSTRAINT fk_classification_measure_projection_model_triplet
			FOREIGN KEY (
				classification_projection_model_version_id,
				classification_model_version_id,
				humor_dimension_model_version_id
			)
			REFERENCES humor.classification_projection_model_version(
				classification_projection_model_version_id,
				classification_model_version_id,
				humor_dimension_model_version_id
			)
			ON DELETE CASCADE;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_measure_projection_model_axis'
		AND conrelid = 'humor.classification_measure_projection_rule'::regclass
	) THEN
		ALTER TABLE humor.classification_measure_projection_rule
			ADD CONSTRAINT fk_classification_measure_projection_model_axis
			FOREIGN KEY (classification_model_axis_id, classification_model_version_id)
			REFERENCES humor.classification_model_axis(classification_model_axis_id, classification_model_version_id)
			ON DELETE RESTRICT;
	END IF;

	IF NOT EXISTS (
		SELECT 1 FROM pg_constraint
		WHERE conname = 'fk_classification_measure_projection_dimension_member'
		AND conrelid = 'humor.classification_measure_projection_rule'::regclass
	) THEN
		ALTER TABLE humor.classification_measure_projection_rule
			ADD CONSTRAINT fk_classification_measure_projection_dimension_member
			FOREIGN KEY (humor_dimension_model_member_id, humor_dimension_model_version_id)
			REFERENCES humor.humor_dimension_model_member(humor_dimension_model_member_id, humor_dimension_model_version_id)
			ON DELETE RESTRICT;
	END IF;
END $$;

CREATE INDEX IF NOT EXISTS ix_classification_measure_projection_enabled
	ON humor.classification_measure_projection_rule(classification_projection_model_version_id, is_enabled)
	WHERE is_enabled = true;

-- ----------------------------------------------------------
-- 10) SEED TAXONOMY AXES (v1)
-- ----------------------------------------------------------

WITH axis_seed(axis_key, display_name, description, axis_type, axis_role, cardinality, sort_order) AS (
	VALUES
		('mechanism', 'Mechanizm humoru', 'Dominujące mechanizmy komediowe obecne w materiale.', 'categorical', 'affinity', 'multi', 10),
		('joke_structure', 'Konstrukcja żartu', 'Strukturalny wzorzec budowy żartu.', 'categorical', 'affinity', 'multi', 20),
		('tone', 'Ton', 'Ton wypowiedzi i klimat przekazu humorystycznego.', 'categorical', 'affinity', 'multi', 30),
		('emotional_effect', 'Efekt emocjonalny', 'Przewidywany efekt emocjonalny u odbiorcy.', 'categorical', 'affinity', 'multi', 40),
		('audience_knowledge', 'Wymagana wiedza odbiorcy', 'Poziom kontekstu potrzebny do zrozumienia żartu.', 'categorical', 'context', 'multi', 50),
		('reference_domain', 'Domena odniesienia', 'Obszar tematyczny, do którego materiał się odnosi.', 'categorical', 'context', 'multi', 60),
		('cultural_reference', 'Odniesienie kulturowe', 'Rodzaj kulturowego zakotwiczenia dowcipu.', 'categorical', 'context', 'multi', 70),
		('absurdity', 'Absurdalność', 'Poziom absurdalności przekazu.', 'scalar', 'affinity', 'single', 80),
		('intensity', 'Intensywność', 'Siła ekspresji humorystycznej.', 'scalar', 'affinity', 'single', 90),
		('complexity', 'Złożoność', 'Złożoność poznawcza żartu.', 'scalar', 'context', 'single', 100),
		('universality', 'Uniwersalność', 'Stopień uniwersalności odbioru między grupami.', 'scalar', 'context', 'single', 110),
		('rescue_potential', 'Potencjał ratunkowy', 'Szansa uratowania odbioru przez alternatywne reakcje.', 'scalar', 'routing', 'single', 120)
)
INSERT INTO humor.classification_axis (axis_key, display_name, description, axis_type, axis_role, cardinality, sort_order, is_active, created_at, updated_at)
SELECT axis_key, display_name, description, axis_type, axis_role, cardinality, sort_order, true, now(), now()
FROM axis_seed
ON CONFLICT (axis_key) DO UPDATE
SET display_name = EXCLUDED.display_name,
	description = EXCLUDED.description,
	axis_type = EXCLUDED.axis_type,
	axis_role = EXCLUDED.axis_role,
	cardinality = EXCLUDED.cardinality,
	sort_order = EXCLUDED.sort_order,
	is_active = true,
	updated_at = now();

-- ----------------------------------------------------------
-- 11) SEED TAXONOMY VALUES (v1)
-- ----------------------------------------------------------

WITH value_seed(axis_key, value_key, display_name, description, ai_description, editorial_guidance, sort_order) AS (
	VALUES
		-- mechanism
		('mechanism','absurd','Absurd','Humor oparty na celowej absurdalności.','Nielogiczne, surrealne połączenia znaczeń.','Utrzymaj jasny sygnał intencji komediowej.',10),
		('mechanism','irony','Ironia','Wypowiedź znacząca odwrotność dosłowności.','Kontrast między warstwą literalną i intencją.','Sprawdź czy ironia nie brzmi jak atak personalny.',20),
		('mechanism','sarcasm','Sarkazm','Ostry ironiczny komentarz.','Ironiczna wypowiedź z podwyższoną kąśliwością.','Wysoki risk safety przy ataku na grupy/osoby.',30),
		('mechanism','parody','Parodia','Stylizowana imitacja formy.','Naśladowanie stylu lub gatunku dla efektu komediowego.','Pilnuj czytelności kontekstu parodii.',40),
		('mechanism','satire','Satyra','Krytyka zjawisk społecznych przez humor.','Humor użyty do komentarza społeczno-politycznego.','Zweryfikuj granicę między satyrą a dezinformacją.',50),
		('mechanism','wordplay','Gra słów','Dowcip oparty o wieloznaczność językową.','Pun, homonimy, podwójne znaczenie.','Oceń zależność od języka i tłumaczalność.',60),
		('mechanism','misunderstanding','Nieporozumienie','Humor z błędnej interpretacji.','Komizm wynika z mylnego zrozumienia intencji.','Upewnij się, że pointa wyjaśnia konflikt.',70),
		('mechanism','contrast','Kontrast','Humor przez zestawienie przeciwieństw.','Silny kontrast sytuacji, cech lub oczekiwań.','Dobrze działa z jasnym setupem.',80),
		('mechanism','exaggeration','Wyolbrzymienie','Przesada jako źródło komizmu.','Celowa hiperbola.','Kontroluj intensywność przy tematach wrażliwych.',90),
		('mechanism','understatement','Niedopowiedzenie','Celowe umniejszenie dla komizmu.','Świadome zaniżenie skali problemu.','Wymaga tonu deadpan lub mock-serious.',100),
		('mechanism','unexpected_twist','Nieoczekiwany zwrot','Pointa odwracająca oczekiwanie.','Nagła zmiana interpretacji w puencie.','Kluczowa spójność z setupem.',110),
		('mechanism','literal_interpretation','Dosłowna interpretacja','Komizm z nadmiernej dosłowności.','Fraza metaforyczna odczytana literalnie.','Sprawdź czy dosłowność jest czytelna.',120),
		('mechanism','breaking_expectations','Złamanie oczekiwań','Komizm przez zerwanie z przewidywanym wzorcem.','Antycypacja i celowe jej przełamanie.','Dobrze łączy się z reversal/misdirection.',130),
		('mechanism','self_deprecation','Autoironia','Żartowanie kosztem samego siebie.','Nadawca obniża własny status dla efektu komicznego.','Bez autoagresji wspierającej self-harm.',140),
		('mechanism','observational_humor','Humor obserwacyjny','Komentarz codziennych doświadczeń.','Rozpoznawalne obserwacje życia codziennego.','Wspiera wysoką uniwersalność.',150),
		('mechanism','dark_humor','Czarny humor','Humor na tematy trudne lub tabu.','Dowcip obejmujący trudne, mroczne tematy.','Wymaga wzmożonej oceny safety.',160),
		('mechanism','awkwardness','Niezręczność','Komizm sytuacji niezręcznej społecznie.','Cringe i napięcie społeczne jako źródło śmiechu.','Kontroluj granicę upokorzenia.',170),
		('mechanism','meta_humor','Meta-humor','Humor o humorze lub medium.','Samoodniesienie do formy żartu.','Wymaga wyższego kontekstu odbiorcy.',180),
		('mechanism','anti_humor','Antyhumor','Celowe rozbrojenie klasycznej puenty.','Brak lub złamanie oczekiwanej puenty.','Nie mylić z niską jakością tekstu.',190),
		('mechanism','dry_humor','Suchy humor','Minimalistyczny, oszczędny styl komediowy.','Deadpan i subtelna pointa bez ekspresji.','Może wymagać wyższej koncentracji odbiorcy.',200),

		-- joke_structure
		('joke_structure','setup_punchline','Setup + punchline','Klasyczny układ wprowadzenie-puenta.','Dwuczęściowa konstrukcja zakończona puentą.','Preferowany czytelny podział sekcji.',10),
		('joke_structure','one_liner','One-liner','Jednozdaniowy skondensowany żart.','Krótka forma o wysokiej gęstości znaczeń.','Unikaj przeładowania referencjami.',20),
		('joke_structure','misdirection','Misdirection','Świadome naprowadzenie na fałszywy trop.','Budowanie błędnego oczekiwania przed pointą.','Wymaga mocnego zwrotu końcowego.',30),
		('joke_structure','rule_of_three','Reguła trzech','Trzecia iteracja łamie wzorzec.','Dwa przewidywalne elementy + trzeci przełamujący.','Dbaj o rytm i eskalację.',40),
		('joke_structure','escalation','Eskalacja','Stopniowe wzmacnianie absurdu lub skali.','Kolejne kroki zwiększają intensywność dowcipu.','Kontroluj długość, by nie stracić puenty.',50),
		('joke_structure','callback','Callback','Nawiązanie do wcześniejszego motywu.','Powrót do wcześniejszego elementu z nową puentą.','Wymaga pamięci kontekstu.',60),
		('joke_structure','reversal','Odwrócenie','Zamiana ról lub perspektywy.','Nagły zwrot semantyczny/roli.','Upewnij się, że odwrócenie jest jednoznaczne.',70),
		('joke_structure','comparison','Porównanie','Humor budowany przez porównanie.','Zderzenie dwóch obiektów/sytuacji.','Unikaj porównań dehumanizujących.',80),
		('joke_structure','analogy','Analogia','Analogia prowadząca do puenty.','Przeniesienie sensu między domenami.','Sprawdź czy analogia jest czytelna.',90),
		('joke_structure','fake_definition','Fałszywa definicja','Pseudo-definicja dla efektu komicznego.','Styl słownikowy z przewrotną treścią.','Wysoka skuteczność przy krótkiej formie.',100),
		('joke_structure','fake_instruction','Fałszywa instrukcja','Pseudo-poradnik z komicznym celem.','Format instrukcji użyty humorystycznie.','Nie sugeruj realnie szkodliwych działań.',110),
		('joke_structure','fake_news','Fałszywy news','Forma newsa użyta satyrycznie.','Imitacja komunikatu informacyjnego.','Wyraźnie sygnalizuj humor, by ograniczyć misinformation.',120),
		('joke_structure','fake_quote','Fałszywy cytat','Pseudo-cytat z przewrotną treścią.','Imitacja wypowiedzi osoby/instytucji.','Unikaj naruszeń reputacji realnych osób.',130),
		('joke_structure','before_after','Przed i po','Kontrast stanu przed i po zdarzeniu.','Dwa stany porównawcze budujące puentę.','Dobrze działa z prostą semantyką.',140),
		('joke_structure','expectation_vs_reality','Oczekiwanie vs rzeczywistość','Zderzenie oczekiwań z realnym wynikiem.','Wzorzec expectation-reality.','Jasno oznacz kontrastową parę.',150),
		('joke_structure','question_answer','Pytanie i odpowiedź','Forma Q&A kończąca się puentą.','Pytanie inicjuje setup, odpowiedź dostarcza twist.','Zadbaj o naturalny rytm dialogu.',160),

		-- tone
		('tone','friendly','Przyjazny','Lekki i życzliwy ton wypowiedzi.','Niski poziom agresji, wysoka dostępność odbioru.','Domyślny ton dla szerokiej publiki.',10),
		('tone','playful','Żartobliwy','Swobodny i figlarny ton.','Luźna energia i zabawa formą.','Uważaj na nadmierny chaos semantyczny.',20),
		('tone','deadpan','Deadpan','Bezemocjonalna prezentacja puenty.','Minimalna ekspresja przy komicznej treści.','Wymaga czytelnej struktury i tempa.',30),
		('tone','mock_serious','Udawanie powagi','Poważny styl użyty ironicznie.','Symulacja formalnego tonu dla efektu komediowego.','Dobrze łączy się z fake_definition/news.',40),
		('tone','sarcastic','Sarkastyczny','Ton kąśliwy i uszczypliwy.','Ironiczna krytyka o podwyższonej ostrości.','Weryfikuj safety przy grupach wrażliwych.',50),
		('tone','aggressive','Agresywny','Ton atakujący lub konfrontacyjny.','Wysokie napięcie interpersonalne.','Preferowana ostrożność moderacyjna.',60),
		('tone','wholesome','Ciepły','Ton pozytywny i wspierający.','Buduje bezpieczny, pozytywny odbiór.','Dobry dla szerokiej dystrybucji.',70),
		('tone','cynical','Cyniczny','Ton sceptyczny i zdystansowany.','Komentarz pesymistyczny/ironiczny.','Kontroluj eskalację negatywności.',80),
		('tone','nostalgic','Nostalgiczny','Ton odwołujący się do wspomnień.','Silne osadzenie w pamięci kulturowej.','Zależny od kontekstu pokoleniowego.',90),
		('tone','chaotic','Chaotyczny','Celowo nieuporządkowany styl przekazu.','Szybkie przeskoki skojarzeń i formy.','Ryzyko niskiej czytelności pointy.',100),
		('tone','surreal','Surrealny','Ton oniryczny i odrealniony.','Nierealistyczne zestawienia i logika snu.','Wzmacnia osie absurdity/mechanism.',110),
		('tone','provocative','Prowokacyjny','Ton celowo wywołujący reakcję.','Przekroczenie norm dla wzbudzenia dyskusji.','Wysokie wymagania safety/review.',120),
		('tone','cringe','Cringe','Ton oparty na celowej niezręczności.','Dyskomfort społeczny jako efekt estetyczny.','Monitoruj granicę upokorzenia.',130),

		-- reference_domain
		('reference_domain','everyday_life','Codzienność','Tematy codziennych sytuacji i zwyczajów.','Kontekst powszechnie rozpoznawalny.','Wspiera uniwersalny odbiór.',10),
		('reference_domain','relationships','Relacje','Tematy relacyjne i interpersonalne.','Dynamika partnerów/znajomych/relacji.','Uważaj na stereotypizację.',20),
		('reference_domain','family','Rodzina','Tematy rodzinne i domowe.','Relacje rodzinne jako źródło humoru.','Sprawdź bezpieczeństwo treści o dzieciach.',30),
		('reference_domain','work','Praca','Kontekst zawodowy i biurowy.','Sytuacje firmowe, role i procesy.','Dobrze łączy się z bureaucracy.',40),
		('reference_domain','programming','Programowanie','Humor o kodzie i developerach.','Żarty techniczne software/dev.','Wysoki próg wiedzy dla odbiorcy.',50),
		('reference_domain','technology','Technologia','Szeroko pojęta technologia.','Sprzęt, software, automatyzacja.','Warto oznaczać wymagany kontekst.',60),
		('reference_domain','internet','Internet','Kultura internetowa i platformy.','Zjawiska online, social media, virale.','Szybko starzejące się referencje.',70),
		('reference_domain','school','Szkoła','Tematy edukacyjne i szkolne.','Doświadczenia uczniów i nauczycieli.','Uważaj na bullying.',80),
		('reference_domain','politics','Polityka','Tematy polityczne i publiczne.','Aktorzy/zdarzenia polityczne.','Podwyższony risk polaryzacji.',90),
		('reference_domain','religion','Religia','Tematy religijne i światopoglądowe.','Przekaz odnoszący się do wiary/instytucji religijnych.','Podwyższona wrażliwość safety.',100),
		('reference_domain','health','Zdrowie','Tematy zdrowia fizycznego.','Codzienne konteksty medyczne i wellbeing.','Unikaj szkodliwych pseudo-porad.',110),
		('reference_domain','mental_health','Zdrowie psychiczne','Tematy dobrostanu psychicznego.','Humor odnoszący się do stanu psychicznego.','Wysoka ostrożność przy self-harm.',120),
		('reference_domain','sports','Sport','Tematy sportowe i kibicowskie.','Dyscypliny, rywalizacja, fandom sportowy.','Dobre dla segmentacji zainteresowań.',130),
		('reference_domain','music','Muzyka','Tematy muzyczne i fandomy.','Artyści, gatunki, koncerty.','Często zależne od kontekstu kulturowego.',140),
		('reference_domain','movies','Filmy','Tematy filmowe i kinowe.','Nawiązania do filmów i kina.','Sprawdź czy referencja jest aktualna.',150),
		('reference_domain','gaming','Gaming','Tematy gier i graczy.','Memy i żarty osadzone w kulturze gier.','Często wysoki próg kontekstu.',160),
		('reference_domain','food','Jedzenie','Tematy kulinarne.','Nawyki żywieniowe i kuchnia.','Zwykle niska bariera odbioru.',170),
		('reference_domain','animals','Zwierzęta','Tematy związane ze zwierzętami.','Zachowania i antropomorfizacja zwierząt.','Unikaj normalizacji przemocy wobec zwierząt.',180),
		('reference_domain','history','Historia','Tematy historyczne.','Wydarzenia i postaci historyczne.','Wymaga precyzji kontekstu.',190),
		('reference_domain','science','Nauka','Tematy naukowe i popularnonaukowe.','Pojęcia i odkrycia naukowe w humorze.','Ryzyko niezrozumienia bez kontekstu.',200),
		('reference_domain','business','Biznes','Tematy ekonomiczne i korporacyjne.','Finanse, startupy, strategie, zarządzanie.','Wysoka użyteczność dla segmentów B2B.',210),
		('reference_domain','bureaucracy','Biurokracja','Humor o procedurach i formalnościach.','Kolejki, formularze, regulaminy.','Dobrze współgra z satyrą.',220),
		('reference_domain','transport','Transport','Tematy komunikacji i mobilności.','Drogi, podróże, środki transportu.','Uważaj na treści o niebezpiecznej jeździe.',230),

		-- cultural_reference
		('cultural_reference','no_reference','Brak odniesienia','Materiał bez wyraźnego osadzenia kulturowego.','Treść samowystarczalna kontekstowo.','Najwyższa przenaszalność między grupami.',10),
		('cultural_reference','current_event','Bieżące wydarzenie','Nawiązanie do aktualnych wydarzeń.','Silny kontekst czasowy/newsowy.','Szybka dezaktualizacja - monitoruj świeżość.',20),
		('cultural_reference','internet_meme','Meme internetowy','Bezpośrednie odniesienie do memosfery.','Format lub narracja znana z obiegu memicznego.','Wysoka zmienność cyklu życia.',30),
		('cultural_reference','movie','Film','Nawiązanie do konkretnego filmu.','Cytat, scena lub motyw filmowy.','Wymaga znajomości referencji.',40),
		('cultural_reference','series','Serial','Nawiązanie do serialu.','Motyw lub postać serialowa.','Często zależne od fandomu.',50),
		('cultural_reference','music','Muzyka','Nawiązanie do utworu/artysty.','Referencja liryczna lub popkulturowa.','Uwaga na lokalne różnice odbioru.',60),
		('cultural_reference','game','Gra','Nawiązanie do gry.','Motyw z uniwersum gry lub praktyk graczy.','Wysoki próg kontekstu dla non-gamerów.',70),
		('cultural_reference','public_figure','Postać publiczna','Nawiązanie do osoby publicznej.','Komentarz odnoszący się do znanej osoby.','Wrażliwe pod kątem reputacji.',80),
		('cultural_reference','historical_event','Wydarzenie historyczne','Nawiązanie historyczne.','Kontekst wydarzeń przeszłych.','Wymaga ostrożności interpretacyjnej.',90),
		('cultural_reference','local_reference','Referencja lokalna','Nawiązanie regionalne/lokalne.','Treść zrozumiała głównie lokalnie.','Ograniczona skalowalność globalna.',100),
		('cultural_reference','generational_reference','Referencja pokoleniowa','Nawiązanie specyficzne dla pokolenia.','Wspólne doświadczenia kohorty wiekowej.','Segmentuj dystrybucję wg grupy wiekowej.',110),
		('cultural_reference','professional_reference','Referencja zawodowa','Nawiązanie do profesji/branży.','Specjalistyczny żargon lub realia pracy.','Łączyć z audience_knowledge=professional_knowledge.',120),

		-- audience_knowledge
		('audience_knowledge','universal','Uniwersalna','Brak specjalnego kontekstu wymaganego.','Treść czytelna dla szerokiej grupy.','Preferowane dla feedu generalnego.',10),
		('audience_knowledge','basic_context','Podstawowy kontekst','Wymagane minimum kontekstu społecznego.','Lekki próg wejścia.','Dobrze opisać krótkim setupem.',20),
		('audience_knowledge','internet_familiarity','Znajomość internetu','Wymagana znajomość kultury internetowej.','Meme literacy i formaty online.','Segmentuj do użytkowników online-heavy.',30),
		('audience_knowledge','professional_knowledge','Wiedza zawodowa','Wymagana wiedza branżowa.','Żargon lub doświadczenie zawodowe.','Wysoki próg dla ogólnego feedu.',40),
		('audience_knowledge','specific_fandom','Konkretny fandom','Wymagana znajomość fandomu.','Referencje do zamkniętego uniwersum.','Niska uniwersalność poza fandomem.',50),
		('audience_knowledge','local_knowledge','Wiedza lokalna','Wymagane lokalne realia i kontekst.','Regionalne idiomy i wydarzenia.','Oznacz region dystrybucji.',60),
		('audience_knowledge','historical_knowledge','Wiedza historyczna','Wymagana wiedza historyczna.','Znajomość wydarzeń przeszłych.','Sprawdź ryzyko błędnej interpretacji.',70),
		('audience_knowledge','current_events','Aktualne wydarzenia','Wymagana orientacja w newsach.','Krótki horyzont aktualności.','Szybka utrata aktualności.',80),

		-- emotional_effect
		('emotional_effect','laugh','Śmiech','Silny efekt rozbawienia.','Wysoka szansa jawnej reakcji śmiechu.','Priorytet dla feedu humor-core.',10),
		('emotional_effect','smile','Uśmiech','Lekki pozytywny efekt.','Subtelne rozbawienie bez silnej reakcji.','Dobre dla niskiej intensywności.',20),
		('emotional_effect','surprise','Zaskoczenie','Efekt zaskoczenia odbiorcy.','Nagła zmiana oczekiwania.','Sprawdź spójność z misdirection/twist.',30),
		('emotional_effect','recognition','Rozpoznanie','Efekt "to o mnie"/"znam to".','Empatyczne rozpoznanie sytuacji.','Wspiera retencję i shareability.',40),
		('emotional_effect','embarrassment','Zażenowanie','Efekt społecznego dyskomfortu.','Cringe lub wstyd zastępczy.','Kontroluj granicę upokorzenia.',50),
		('emotional_effect','shock','Szok','Mocny efekt szoku.','Nagły, intensywny bodziec emocjonalny.','Wysoki priorytet safety review.',60),
		('emotional_effect','discomfort','Dyskomfort','Umiarkowany dyskomfort odbioru.','Niepokój bez pełnego szoku.','Monitoruj wpływ na moderation.',70),
		('emotional_effect','nostalgia','Nostalgia','Efekt nostalgii i wspomnień.','Uruchamianie pamięci wspólnych doświadczeń.','Zależny od kohorty wiekowej.',80),
		('emotional_effect','sympathy','Sympatia','Efekt ciepłej empatii.','Pozytywny rezonans interpersonalny.','Wspiera wholesome ton.',90),
		('emotional_effect','curiosity','Ciekawość','Efekt zaciekawienia.','Treść zachęca do dalszej eksploracji.','Przydatne dla follow-up content.',100),
		('emotional_effect','confusion','Dezorientacja','Efekt dezorientacji odbiorcy.','Wysoki poziom niejednoznaczności.','Sprawdź czy to zamierzony efekt, nie błąd jakości.',110)
)
INSERT INTO humor.classification_value (
	classification_axis_id,
	value_key,
	display_name,
	description,
	ai_description,
	editorial_guidance,
	sort_order,
	is_active,
	created_at,
	updated_at
)
SELECT a.classification_axis_id,
	v.value_key,
	v.display_name,
	v.description,
	v.ai_description,
	v.editorial_guidance,
	v.sort_order,
	true,
	now(),
	now()
FROM value_seed v
JOIN humor.classification_axis a ON a.axis_key = v.axis_key
ON CONFLICT (classification_axis_id, value_key) DO UPDATE
SET display_name = EXCLUDED.display_name,
	description = EXCLUDED.description,
	ai_description = EXCLUDED.ai_description,
	editorial_guidance = EXCLUDED.editorial_guidance,
	sort_order = EXCLUDED.sort_order,
	is_active = true,
	updated_at = now();

-- Include existing stable reaction_mechanism keys (if missing) into mechanism axis.
WITH mechanism_axis AS (
	SELECT classification_axis_id
	FROM humor.classification_axis
	WHERE axis_key = 'mechanism'
),
missing_mechanisms AS (
	SELECT rm.mechanism_key,
		rm.display_name,
		row_number() OVER (ORDER BY rm.mechanism_key) AS rn
	FROM humor.reaction_mechanism rm
	LEFT JOIN humor.classification_value cv
		ON cv.value_key = rm.mechanism_key
		AND cv.classification_axis_id = (SELECT classification_axis_id FROM mechanism_axis)
	WHERE rm.is_active = true
	AND cv.classification_value_id IS NULL
)
INSERT INTO humor.classification_value (
	classification_axis_id,
	value_key,
	display_name,
	description,
	ai_description,
	editorial_guidance,
	sort_order,
	is_active,
	created_at,
	updated_at
)
SELECT (SELECT classification_axis_id FROM mechanism_axis),
	m.mechanism_key,
	COALESCE(NULLIF(trim(m.display_name), ''), m.mechanism_key),
	'Klucz przejęty z humor.reaction_mechanism.',
	'Legacy mechanism bridge seed.',
	'Zweryfikuj mapowanie semantyczne do nowej taksonomii.',
	900 + m.rn,
	true,
	now(),
	now()
FROM missing_mechanisms m;

-- ----------------------------------------------------------
-- 12) SEED MODEL VERSION v1 + AXIS WEIGHTS
-- ----------------------------------------------------------

INSERT INTO humor.classification_model_version (
	model_key,
	version_no,
	version_label,
	status,
	description,
	config,
	created_by_account_id,
	created_at,
	activated_at
)
SELECT
	'haia_humor_classification',
	1,
	'v1',
	'active',
	'Początkowa wersja modelu klasyfikacji humoru. Wagi osi i wartości są hipotezami produktowymi v1.',
	jsonb_build_object(
		'notes', 'v1 hypothesis weights',
		'scope', 'classification only',
		'projection_seeded', false
	),
	NULL,
	now(),
	now()
WHERE NOT EXISTS (
	SELECT 1
	FROM humor.classification_model_version mv
	WHERE mv.model_key = 'haia_humor_classification'
	AND mv.version_no = 1
);

WITH target_model AS (
	SELECT classification_model_version_id
	FROM humor.classification_model_version
	WHERE model_key = 'haia_humor_classification'
	AND version_no = 1
),
axis_weights(axis_key, axis_weight, is_required, min_a, max_a, normalization_method, sort_hint) AS (
	VALUES
		('mechanism', 1.00::numeric, true, 1, NULL, 'weighted_multi', 10),
		('joke_structure', 0.85::numeric, false, 0, NULL, 'weighted_multi', 20),
		('tone', 0.75::numeric, false, 0, NULL, 'weighted_multi', 30),
		('emotional_effect', 0.70::numeric, false, 0, NULL, 'weighted_multi', 40),
		('audience_knowledge', 0.65::numeric, false, 0, NULL, 'weighted_multi', 50),
		('reference_domain', 0.55::numeric, false, 0, NULL, 'weighted_multi', 60),
		('cultural_reference', 0.50::numeric, false, 0, NULL, 'weighted_multi', 70),
		('absurdity', 0.90::numeric, false, 0, 1, 'linear_0_1', 80),
		('intensity', 0.70::numeric, false, 0, 1, 'linear_0_1', 90),
		('complexity', 0.55::numeric, false, 0, 1, 'linear_0_1', 100),
		('universality', 0.45::numeric, false, 0, 1, 'linear_0_1', 110),
		('rescue_potential', 0.60::numeric, false, 0, 1, 'linear_0_1', 120)
)
INSERT INTO humor.classification_model_axis (
	classification_model_version_id,
	classification_axis_id,
	axis_weight,
	is_required,
	minimum_assignments,
	maximum_assignments,
	normalization_method,
	member_status,
	created_at
)
SELECT tm.classification_model_version_id,
	a.classification_axis_id,
	w.axis_weight,
	w.is_required,
	w.min_a,
	w.max_a,
	w.normalization_method,
	'active',
	now()
FROM axis_weights w
JOIN humor.classification_axis a ON a.axis_key = w.axis_key
CROSS JOIN target_model tm
ON CONFLICT (classification_model_version_id, classification_axis_id) DO UPDATE
SET axis_weight = EXCLUDED.axis_weight,
	is_required = EXCLUDED.is_required,
	minimum_assignments = EXCLUDED.minimum_assignments,
	maximum_assignments = EXCLUDED.maximum_assignments,
	normalization_method = EXCLUDED.normalization_method,
	member_status = 'active';

-- Seed model values for categorical axes, default_weight = 1.0
WITH target_model AS (
	SELECT classification_model_version_id
	FROM humor.classification_model_version
	WHERE model_key = 'haia_humor_classification'
	AND version_no = 1
),
categorical_axis AS (
	SELECT classification_axis_id
	FROM humor.classification_axis
	WHERE axis_type = 'categorical'
),
joined AS (
	SELECT tm.classification_model_version_id,
		ma.classification_model_axis_id,
		ma.classification_axis_id,
		cv.classification_value_id
	FROM target_model tm
	JOIN humor.classification_model_axis ma
		ON ma.classification_model_version_id = tm.classification_model_version_id
	JOIN categorical_axis ca
		ON ca.classification_axis_id = ma.classification_axis_id
	JOIN humor.classification_value cv
		ON cv.classification_axis_id = ma.classification_axis_id
		AND cv.is_active = true
)
INSERT INTO humor.classification_model_value (
	classification_model_version_id,
	classification_model_axis_id,
	classification_axis_id,
	classification_value_id,
	default_weight,
	is_enabled,
	created_at
)
SELECT classification_model_version_id,
	classification_model_axis_id,
	classification_axis_id,
	classification_value_id,
	1.0,
	true,
	now()
FROM joined
ON CONFLICT (classification_model_version_id, classification_value_id) DO UPDATE
SET classification_model_axis_id = EXCLUDED.classification_model_axis_id,
	classification_axis_id = EXCLUDED.classification_axis_id,
	default_weight = EXCLUDED.default_weight,
	is_enabled = true;

-- ----------------------------------------------------------
-- 13) SEED SAFETY CATEGORIES
-- ----------------------------------------------------------

WITH sensitivity_seed(category_key, display_name, description, sort_order) AS (
	VALUES
		('profanity','Wulgaryzmy','Treści zawierające wulgarne słownictwo.',10),
		('sexual_content','Treści seksualne','Treści o charakterze seksualnym.',20),
		('violence','Przemoc','Treści odnoszące się do przemocy.',30),
		('self_harm','Samookaleczenie','Treści dotyczące samouszkodzeń i autoagresji.',40),
		('mental_health','Zdrowie psychiczne','Treści mogące stygmatyzować lub trywializować zdrowie psychiczne.',50),
		('disability','Niepełnosprawność','Treści dotyczące niepełnosprawności.',60),
		('religion','Religia','Treści związane z religią i przekonaniami.',70),
		('politics','Polityka','Treści polityczne o podwyższonym ryzyku konfliktu.',80),
		('race_ethnicity','Rasa i etniczność','Treści odnoszące się do rasy/etniczności.',90),
		('gender','Płeć i tożsamość','Treści dotyczące płci i tożsamości.',100),
		('body_image','Wizerunek ciała','Treści dotyczące wyglądu i ciała.',110),
		('substance_use','Substancje','Treści o używaniu substancji psychoaktywnych.',120),
		('crime','Przestępczość','Treści promujące/normalizujące przestępczość.',130),
		('death','Śmierć','Treści dotyczące śmierci i żałoby.',140),
		('personal_attack','Atak personalny','Treści atakujące konkretną osobę.',150),
		('harassment','Nękanie','Treści o charakterze nękającym.',160),
		('misinformation','Dezinformacja','Treści mogące wprowadzać odbiorcę w błąd.',170)
)
INSERT INTO humor.sensitivity_category (
	category_key,
	display_name,
	description,
	sort_order,
	is_active,
	created_at
)
SELECT category_key, display_name, description, sort_order, true, now()
FROM sensitivity_seed
ON CONFLICT (category_key) DO UPDATE
SET display_name = EXCLUDED.display_name,
	description = EXCLUDED.description,
	sort_order = EXCLUDED.sort_order,
	is_active = true;

-- ----------------------------------------------------------
-- 14) BACKFILL EXISTING candidate_classification
-- ----------------------------------------------------------

WITH target_model AS (
	SELECT classification_model_version_id
	FROM humor.classification_model_version
	WHERE model_key = 'haia_humor_classification'
	AND version_no = 1
)
UPDATE onboarding.candidate_classification c
SET classification_model_version_id = COALESCE(c.classification_model_version_id, tm.classification_model_version_id),
	revision_no = COALESCE(c.revision_no, 1),
	source_type = COALESCE(c.source_type, 'system_migration'),
	classification_status = COALESCE(c.classification_status, 'approved'),
	created_at = COALESCE(c.created_at, ocv.created_at, now())
FROM target_model tm
JOIN onboarding.onboarding_candidate_version ocv ON true
WHERE ocv.onboarding_candidate_version_id = c.onboarding_candidate_version_id
	AND (
		c.classification_model_version_id IS NULL
		OR c.revision_no IS NULL
		OR c.source_type IS NULL
		OR c.classification_status IS NULL
		OR c.created_at IS NULL
	);

-- Map legacy dryness 1..6 into active default_sucharek scale levels.
WITH active_sucharek AS (
	SELECT dsl.level_no, dsl.dryness_scale_level_id
	FROM humor.dryness_scale ds
	JOIN humor.dryness_scale_version dsv
		ON dsv.dryness_scale_id = ds.dryness_scale_id
		AND dsv.status = 'active'
	JOIN humor.dryness_scale_level dsl
		ON dsl.dryness_scale_version_id = dsv.dryness_scale_version_id
	WHERE ds.scale_key = 'default_sucharek'
)
UPDATE onboarding.candidate_classification c
SET predicted_dryness_scale_level_id = s.dryness_scale_level_id,
	predicted_dryness_confidence = COALESCE(c.predicted_dryness_confidence, 0.55)
FROM active_sucharek s
WHERE c.dryness IS NOT NULL
	AND c.predicted_dryness_scale_level_id IS NULL
	AND c.dryness = s.level_no;

-- Backfill legacy numeric evidence into normalized measure table.
WITH target_model AS (
	SELECT classification_model_version_id
	FROM humor.classification_model_version
	WHERE model_key = 'haia_humor_classification'
	AND version_no = 1
),
axis_map AS (
	SELECT ma.classification_model_axis_id,
		a.axis_key,
		ma.classification_model_version_id
	FROM humor.classification_model_axis ma
	JOIN humor.classification_axis a ON a.classification_axis_id = ma.classification_axis_id
	JOIN target_model tm ON tm.classification_model_version_id = ma.classification_model_version_id
	WHERE a.axis_key IN ('intensity','complexity','universality','rescue_potential')
),
source_rows AS (
	SELECT c.candidate_classification_id,
		c.classification_model_version_id,
		'amplitude'::text AS marker,
		c.intensity,
		c.complexity,
		c.universality_score,
		c.reaction_rescue_potential
	FROM onboarding.candidate_classification c
	JOIN target_model tm ON tm.classification_model_version_id = c.classification_model_version_id
),
measure_rows AS (
	SELECT s.candidate_classification_id,
		s.classification_model_version_id,
		am.classification_model_axis_id,
		CASE am.axis_key
			WHEN 'intensity' THEN CASE WHEN s.intensity IS NULL THEN NULL ELSE (s.intensity - 1)::numeric / 4::numeric END
			WHEN 'complexity' THEN CASE WHEN s.complexity IS NULL THEN NULL ELSE (s.complexity - 1)::numeric / 4::numeric END
			WHEN 'universality' THEN s.universality_score
			WHEN 'rescue_potential' THEN s.reaction_rescue_potential
			ELSE NULL
		END AS normalized_value
	FROM source_rows s
	JOIN axis_map am ON am.classification_model_version_id = s.classification_model_version_id
)
INSERT INTO onboarding.candidate_classification_measure (
	candidate_classification_id,
	classification_model_version_id,
	classification_model_axis_id,
	normalized_value,
	confidence,
	measurement_source,
	editorial_note,
	created_at
)
SELECT mr.candidate_classification_id,
	mr.classification_model_version_id,
	mr.classification_model_axis_id,
	mr.normalized_value,
	NULL,
	'system_migration',
	'Backfill z legacy candidate_classification.',
	now()
FROM measure_rows mr
WHERE mr.normalized_value IS NOT NULL
ON CONFLICT (candidate_classification_id, classification_model_axis_id) DO NOTHING;

-- Safe NOT NULL only where backfill is deterministic.
ALTER TABLE onboarding.candidate_classification
	ALTER COLUMN classification_model_version_id SET NOT NULL,
	ALTER COLUMN revision_no SET NOT NULL,
	ALTER COLUMN source_type SET NOT NULL,
	ALTER COLUMN classification_status SET NOT NULL,
	ALTER COLUMN created_at SET NOT NULL;

-- ----------------------------------------------------------
-- 15) PROJECTION FOUNDATION SEED (structure only)
-- ----------------------------------------------------------

-- Intentionally no default projection rules are seeded in v0.1.
-- Rationale: avoid arbitrary mapping without explicit approved DNA mapping policy.

CREATE INDEX IF NOT EXISTS ix_projection_rule_value_lookup
	ON humor.classification_value_projection_rule(classification_model_value_id, is_enabled)
	WHERE is_enabled = true;

CREATE INDEX IF NOT EXISTS ix_projection_rule_measure_lookup
	ON humor.classification_measure_projection_rule(classification_model_axis_id, is_enabled)
	WHERE is_enabled = true;

COMMIT;
