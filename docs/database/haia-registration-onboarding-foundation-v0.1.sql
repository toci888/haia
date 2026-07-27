-- HAIA Registration & Adaptive Humor Onboarding Foundation
-- PostgreSQL DDL v0.1
-- Internal revision: 0.2 — in-place repair
-- Target: PostgreSQL 16+

-- Revision 0.2 Change Log (in-place):
-- - repaired reaction identity/versioning and multi-reaction selection model
-- - added real reaction exposure tracking and 12/6 pack policy fields
-- - added Sucharek versioned scale and exclusive/coexistence policy enforcement
-- - repaired prior vs observed evidence separation for Humor DNA
-- - added Humor DNA model membership mapping
-- - repaired Initial Humor Snapshot versioning and published immutability
-- - added missing foreign keys and hardened legal retention references
-- - enforced append-oriented audit immutability via trigger

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS citext;

CREATE SCHEMA IF NOT EXISTS identity;
CREATE SCHEMA IF NOT EXISTS onboarding;
CREATE SCHEMA IF NOT EXISTS humor;
CREATE SCHEMA IF NOT EXISTS studio;
CREATE SCHEMA IF NOT EXISTS learning;
CREATE SCHEMA IF NOT EXISTS ai;
CREATE SCHEMA IF NOT EXISTS audit;

-- ==========================================================
-- TIER 1 — REQUIRED FOR FIRST WORKING FLOW
-- ==========================================================

-- ----------------------------
-- identity
-- ----------------------------
CREATE TABLE IF NOT EXISTS identity.account (
	account_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_status text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	activated_at timestamptz,
	status_changed_at timestamptz NOT NULL DEFAULT now(),
	pending_expires_at timestamptz,
	deleted_at timestamptz,
	deletion_reason text,
	row_version bigint NOT NULL DEFAULT 1,
	CHECK (account_status IN ('pending_email_verification','active','suspended','locked','deletion_pending','deleted','expired'))
);
COMMENT ON TABLE identity.account IS 'Główna tożsamość konta HAIA. Status wielowartościowy zamiast pojedynczego is_active.';

CREATE TABLE IF NOT EXISTS identity.account_email (
	account_email_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	email_original text NOT NULL,
	email_normalized citext NOT NULL,
	is_primary boolean NOT NULL DEFAULT false,
	verification_status text NOT NULL DEFAULT 'pending',
	verified_at timestamptz,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	released_at timestamptz,
	CHECK (verification_status IN ('pending','verified','rejected','expired')),
	CHECK (length(trim(email_original)) > 3)
);
COMMENT ON TABLE identity.account_email IS 'Adresy e-mail konta. Wspiera zmianę e-mail i case-insensitive uniqueness.';
CREATE UNIQUE INDEX IF NOT EXISTS ux_account_email_normalized_active
	ON identity.account_email(email_normalized)
	WHERE released_at IS NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_account_email_one_primary
	ON identity.account_email(account_id)
	WHERE is_primary = true AND released_at IS NULL;

CREATE TABLE IF NOT EXISTS identity.password_credential (
	password_credential_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	password_hash text NOT NULL,
	hash_algorithm text NOT NULL,
	hash_format_version text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	changed_at timestamptz NOT NULL DEFAULT now(),
	force_change_required boolean NOT NULL DEFAULT false,
	is_active boolean NOT NULL DEFAULT true,
	CHECK (length(password_hash) >= 40)
);
COMMENT ON TABLE identity.password_credential IS 'Wyłącznie hash hasła; brak jawnych sekretów.';
CREATE UNIQUE INDEX IF NOT EXISTS ux_password_credential_one_active
	ON identity.password_credential(account_id)
	WHERE is_active = true;

CREATE TABLE IF NOT EXISTS identity.external_login (
	external_login_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	provider text NOT NULL,
	provider_subject text NOT NULL,
	provider_email_hint citext,
	provider_email_verified boolean,
	linked_at timestamptz NOT NULL DEFAULT now(),
	last_used_at timestamptz,
	is_active boolean NOT NULL DEFAULT true,
	metadata jsonb,
	CHECK (provider IN ('google','facebook','other'))
);
COMMENT ON TABLE identity.external_login IS 'Powiązanie konta z providerami social login. Brak tokenów dostępowych.';
CREATE UNIQUE INDEX IF NOT EXISTS ux_external_login_provider_subject
	ON identity.external_login(provider, provider_subject);
CREATE INDEX IF NOT EXISTS ix_external_login_account_id ON identity.external_login(account_id);

CREATE TABLE IF NOT EXISTS identity.security_token (
	security_token_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	account_email_id uuid REFERENCES identity.account_email(account_email_id) ON DELETE SET NULL,
	token_purpose text NOT NULL,
	token_hash text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	expires_at timestamptz NOT NULL,
	used_at timestamptz,
	invalidated_at timestamptz,
	resend_sequence integer NOT NULL DEFAULT 0,
	request_correlation_id text,
	CHECK (token_purpose IN ('email_verification','password_reset','email_change')),
	CHECK (expires_at > created_at)
);
COMMENT ON TABLE identity.security_token IS 'Tokeny bezpieczeństwa przechowywane jako hash; wspiera idempotencję aktywacji i resend.';
CREATE UNIQUE INDEX IF NOT EXISTS ux_security_token_hash ON identity.security_token(token_hash);
CREATE INDEX IF NOT EXISTS ix_security_token_active_lookup
	ON identity.security_token(account_id, token_purpose, expires_at)
	WHERE used_at IS NULL AND invalidated_at IS NULL;

CREATE TABLE IF NOT EXISTS identity.public_profile (
	profile_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL UNIQUE REFERENCES identity.account(account_id) ON DELETE CASCADE,
	nickname text NOT NULL,
	nickname_normalized citext NOT NULL,
	profile_visibility text NOT NULL DEFAULT 'draft_private',
	avatar_asset_ref text,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	released_at timestamptz,
	CHECK (profile_visibility IN ('draft_private','public_visible','hidden','retired')),
	CHECK (length(trim(nickname)) BETWEEN 3 AND 40)
);
COMMENT ON TABLE identity.public_profile IS 'Publiczna tożsamość użytkownika. Pseudonim rezerwowany podczas rejestracji.';
CREATE UNIQUE INDEX IF NOT EXISTS ux_public_profile_nickname_active
	ON identity.public_profile(nickname_normalized)
	WHERE released_at IS NULL;

CREATE TABLE IF NOT EXISTS identity.legal_document (
	legal_document_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	document_key text NOT NULL UNIQUE,
	document_type text NOT NULL,
	is_required boolean NOT NULL DEFAULT true,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (document_type IN ('terms_of_service','privacy_notice','marketing_consent','ai_training_consent','age_policy','other'))
);

CREATE TABLE IF NOT EXISTS identity.legal_document_version (
	legal_document_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	legal_document_id uuid NOT NULL REFERENCES identity.legal_document(legal_document_id) ON DELETE CASCADE,
	version_label text NOT NULL,
	content_hash text NOT NULL,
	language_code text NOT NULL,
	valid_from timestamptz NOT NULL,
	valid_to timestamptz,
	status text NOT NULL DEFAULT 'active',
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(legal_document_id, version_label),
	CHECK (status IN ('draft','active','retired','archived')),
	CHECK (valid_to IS NULL OR valid_to > valid_from)
);

CREATE TABLE IF NOT EXISTS identity.account_document_acceptance (
	account_document_acceptance_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE RESTRICT,
	legal_document_version_id uuid NOT NULL REFERENCES identity.legal_document_version(legal_document_version_id) ON DELETE RESTRICT,
	acceptance_type text NOT NULL,
	accepted_at timestamptz NOT NULL DEFAULT now(),
	accepted_via text NOT NULL,
	source_ip inet,
	user_agent text,
	acceptance_context jsonb,
	UNIQUE(account_id, legal_document_version_id, acceptance_type),
	CHECK (acceptance_type IN ('accepted','acknowledged','declined')),
	CHECK (accepted_via IN ('registration_form','settings','api','studio'))
);

-- ----------------------------
-- onboarding contexts
-- ----------------------------
CREATE TABLE IF NOT EXISTS onboarding.industry (
	industry_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	industry_key text NOT NULL UNIQUE,
	display_name text NOT NULL,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS onboarding.profession (
	profession_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	industry_id uuid REFERENCES onboarding.industry(industry_id) ON DELETE SET NULL,
	profession_key text NOT NULL UNIQUE,
	display_name text NOT NULL,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS onboarding.account_profession_context (
	account_profession_context_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	industry_id uuid REFERENCES onboarding.industry(industry_id) ON DELETE SET NULL,
	profession_id uuid REFERENCES onboarding.profession(profession_id) ON DELETE SET NULL,
	free_text_context text,
	source text NOT NULL DEFAULT 'user_declared',
	confidence numeric(5,4) NOT NULL DEFAULT 0.5000,
	is_active boolean NOT NULL DEFAULT true,
	visibility text NOT NULL DEFAULT 'private',
	provided_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	removed_at timestamptz,
	CHECK (source IN ('user_declared','inferred','imported','unknown')),
	CHECK (visibility IN ('private','public','studio_only')),
	CHECK (confidence BETWEEN 0 AND 1)
);
CREATE INDEX IF NOT EXISTS ix_account_profession_context_account_id ON onboarding.account_profession_context(account_id);
CREATE UNIQUE INDEX IF NOT EXISTS ux_account_profession_context_one_active
	ON onboarding.account_profession_context(account_id)
	WHERE is_active = true AND removed_at IS NULL;

CREATE TABLE IF NOT EXISTS onboarding.age_range (
	age_range_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	range_key text NOT NULL UNIQUE,
	display_name text NOT NULL,
	min_age smallint,
	max_age smallint,
	is_active boolean NOT NULL DEFAULT true,
	CHECK (min_age IS NULL OR max_age IS NULL OR min_age <= max_age)
);

CREATE TABLE IF NOT EXISTS onboarding.account_age_context (
	account_age_context_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	age_range_id uuid NOT NULL REFERENCES onboarding.age_range(age_range_id) ON DELETE RESTRICT,
	source text NOT NULL DEFAULT 'user_declared',
	start_weight numeric(5,4) NOT NULL DEFAULT 0.2000,
	confidence numeric(5,4) NOT NULL DEFAULT 0.5000,
	is_active boolean NOT NULL DEFAULT true,
	provided_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	removed_at timestamptz,
	CHECK (source IN ('user_declared','inferred','imported')),
	CHECK (start_weight BETWEEN 0 AND 1),
	CHECK (confidence BETWEEN 0 AND 1)
);
COMMENT ON TABLE onboarding.account_age_context IS 'Opcjonalny Age Context Seed do personalizacji; odseparowany od compliance.';
CREATE UNIQUE INDEX IF NOT EXISTS ux_account_age_context_one_active
	ON onboarding.account_age_context(account_id)
	WHERE is_active = true AND removed_at IS NULL;

CREATE TABLE IF NOT EXISTS onboarding.age_eligibility_record (
	age_eligibility_record_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE RESTRICT,
	policy_version text NOT NULL,
	eligibility_status text NOT NULL,
	determination_source text NOT NULL,
	birth_date date,
	determined_at timestamptz NOT NULL DEFAULT now(),
	notes text,
	CHECK (eligibility_status IN ('unknown','eligible','ineligible','pending_review')),
	CHECK (determination_source IN ('self_declaration','legal_check','manual_review','provider_claim'))
);
COMMENT ON TABLE onboarding.age_eligibility_record IS 'Warstwa compliance wieku. Nie służy do modelowania preferencji humoru.';
COMMENT ON COLUMN onboarding.age_eligibility_record.account_id IS 'Legal retention: ON DELETE RESTRICT, rekord compliance nie może znikać przez CASCADE konta.';

-- ----------------------------
-- onboarding flow and candidate library
-- ----------------------------
CREATE TABLE IF NOT EXISTS onboarding.onboarding_flow_version (
	onboarding_flow_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	flow_key text NOT NULL,
	version_label text NOT NULL,
	status text NOT NULL DEFAULT 'draft',
	config jsonb NOT NULL DEFAULT '{}'::jsonb,
	activated_at timestamptz,
	retired_at timestamptz,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(flow_key, version_label),
	CHECK (status IN ('draft','active','retired','archived'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_onboarding_flow_one_active
	ON onboarding.onboarding_flow_version(flow_key)
	WHERE status = 'active';

CREATE TABLE IF NOT EXISTS onboarding.onboarding_session (
	onboarding_session_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	onboarding_flow_version_id uuid NOT NULL REFERENCES onboarding.onboarding_flow_version(onboarding_flow_version_id) ON DELETE RESTRICT,
	humor_dimension_model_version_id uuid NOT NULL,
	source_registration text NOT NULL,
	current_step_key text NOT NULL,
	session_status text NOT NULL DEFAULT 'started',
	optional_context_skipped boolean NOT NULL DEFAULT false,
	meme_calibration_skipped boolean NOT NULL DEFAULT false,
	humor_inspiration_skipped boolean NOT NULL DEFAULT false,
	started_at timestamptz NOT NULL DEFAULT now(),
	completed_at timestamptz,
	interrupted_at timestamptz,
	resumed_at timestamptz,
	last_activity_at timestamptz NOT NULL DEFAULT now(),
	resume_token_hash text,
	last_completed_position integer,
	scoring_policy_version_id uuid,
	row_version bigint NOT NULL DEFAULT 1,
	UNIQUE(onboarding_session_id, humor_dimension_model_version_id),
	CHECK (source_registration IN ('email','google','facebook','other')),
	CHECK (session_status IN ('started','in_progress','paused','completed','abandoned','cancelled'))
);
CREATE INDEX IF NOT EXISTS ix_onboarding_session_account_id ON onboarding.onboarding_session(account_id, started_at DESC);
CREATE INDEX IF NOT EXISTS ix_onboarding_session_status ON onboarding.onboarding_session(session_status, last_activity_at DESC);

CREATE TABLE IF NOT EXISTS onboarding.onboarding_step_state (
	onboarding_step_state_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	step_key text NOT NULL,
	step_status text NOT NULL,
	entered_at timestamptz NOT NULL DEFAULT now(),
	completed_at timestamptz,
	skipped_at timestamptz,
	step_payload jsonb,
	UNIQUE(onboarding_session_id, step_key),
	CHECK (step_status IN ('pending','active','completed','skipped','failed'))
);

CREATE TABLE IF NOT EXISTS onboarding.onboarding_candidate (
	onboarding_candidate_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_key text NOT NULL UNIQUE,
	content_format text NOT NULL,
	candidate_status text NOT NULL DEFAULT 'draft',
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	approved_version_id uuid,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	CHECK (content_format IN ('meme','image_caption','text_joke','dialogue','retort','absurd_question','scene','observational')),
	CHECK (candidate_status IN ('draft','awaiting_ai_analysis','ai_analysis_ready','reactions_generated','editorial_review','moderation_review','approved','scheduled','active','paused','retired','rejected','archived'))
);

CREATE TABLE IF NOT EXISTS onboarding.onboarding_candidate_version (
	onboarding_candidate_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_candidate_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate(onboarding_candidate_id) ON DELETE CASCADE,
	version_no integer NOT NULL,
	language_code text NOT NULL DEFAULT 'pl-PL',
	title text,
	body_text text,
	caption_text text,
	situation_description text,
	role_in_flow text NOT NULL DEFAULT 'exploration',
	editorial_status text NOT NULL DEFAULT 'draft',
	moderation_status text NOT NULL DEFAULT 'pending',
	safety_classification text NOT NULL DEFAULT 'unknown',
	rights_status text NOT NULL DEFAULT 'pending_review',
	valid_from timestamptz,
	valid_to timestamptz,
	is_significant_change boolean NOT NULL DEFAULT true,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(onboarding_candidate_id, version_no),
	CHECK (role_in_flow IN ('start','exploration','exploitation','recovery','final','fallback')),
	CHECK (editorial_status IN ('draft','awaiting_ai_analysis','ai_analysis_ready','editorial_review','approved','scheduled','active','paused','retired','rejected','archived')),
	CHECK (moderation_status IN ('pending','approved','restricted','rejected','escalated')),
	CHECK (safety_classification IN ('unknown','safe','caution','unsafe'))
);
CREATE INDEX IF NOT EXISTS ix_candidate_version_candidate ON onboarding.onboarding_candidate_version(onboarding_candidate_id, version_no DESC);

CREATE TABLE IF NOT EXISTS onboarding.media_asset (
	media_asset_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	storage_key text NOT NULL,
	mime_type text NOT NULL,
	byte_size bigint NOT NULL,
	sha256_hash text NOT NULL,
	width_px integer,
	height_px integer,
	processing_status text NOT NULL DEFAULT 'ready',
	rights_status text NOT NULL DEFAULT 'unknown',
	moderation_status text NOT NULL DEFAULT 'pending',
	alt_text text,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	CHECK (byte_size > 0),
	CHECK (processing_status IN ('pending','processing','ready','failed')),
	CHECK (moderation_status IN ('pending','approved','restricted','rejected'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_media_asset_storage_key ON onboarding.media_asset(storage_key);
CREATE UNIQUE INDEX IF NOT EXISTS ux_media_asset_sha256 ON onboarding.media_asset(sha256_hash);

CREATE TABLE IF NOT EXISTS onboarding.candidate_version_media (
	candidate_version_media_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE CASCADE,
	media_asset_id uuid NOT NULL REFERENCES onboarding.media_asset(media_asset_id) ON DELETE RESTRICT,
	media_role text NOT NULL DEFAULT 'primary',
	display_order integer NOT NULL DEFAULT 1,
	caption_override text,
	presentation_metadata jsonb,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(onboarding_candidate_version_id, media_asset_id, media_role),
	UNIQUE(onboarding_candidate_version_id, media_role, display_order),
	CHECK (media_role IN ('primary','secondary','thumbnail','reference')),
	CHECK (display_order > 0)
);

CREATE TABLE IF NOT EXISTS onboarding.candidate_activation (
	candidate_activation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE CASCADE,
	activation_scope text NOT NULL DEFAULT 'global',
	cohort_definition_version_id uuid,
	starts_at timestamptz NOT NULL,
	ends_at timestamptz,
	max_exposures bigint,
	max_frequency_per_user integer,
	experiment_key text,
	is_enabled boolean NOT NULL DEFAULT true,
	kill_switch boolean NOT NULL DEFAULT false,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (activation_scope IN ('global','cohort','experiment')),
	CHECK (ends_at IS NULL OR ends_at > starts_at)
);
CREATE INDEX IF NOT EXISTS ix_candidate_activation_active
	ON onboarding.candidate_activation(onboarding_candidate_version_id, starts_at)
	WHERE is_enabled = true AND kill_switch = false;

-- ----------------------------
-- reactions
-- ----------------------------
CREATE TABLE IF NOT EXISTS humor.reaction_mechanism (
	reaction_mechanism_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	mechanism_key text NOT NULL UNIQUE,
	display_name text NOT NULL,
	is_active boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS humor.reaction_pack (
	reaction_pack_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE CASCADE,
	pack_key text NOT NULL,
	language_code text NOT NULL DEFAULT 'pl-PL',
	source_type text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(onboarding_candidate_version_id, pack_key, language_code),
	CHECK (source_type IN ('ai','human','ai_editorial'))
);

CREATE TABLE IF NOT EXISTS humor.reaction_pack_version (
	reaction_pack_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	reaction_pack_id uuid NOT NULL REFERENCES humor.reaction_pack(reaction_pack_id) ON DELETE CASCADE,
	version_no integer NOT NULL,
	status text NOT NULL DEFAULT 'generated',
	target_pool_size integer NOT NULL DEFAULT 12,
	initial_visible_count integer NOT NULL DEFAULT 6,
	max_active_selections integer DEFAULT 12,
	dryness_interaction_mode text NOT NULL DEFAULT 'exclusive',
	prompt_template_version_id uuid,
	editorial_status text NOT NULL DEFAULT 'awaiting_editorial_review',
	moderation_status text NOT NULL DEFAULT 'pending',
	activated_at timestamptz,
	withdrawn_at timestamptz,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(reaction_pack_id, version_no),
	CHECK (target_pool_size > 0),
	CHECK (initial_visible_count > 0),
	CHECK (initial_visible_count <= target_pool_size),
	CHECK (max_active_selections IS NULL OR max_active_selections BETWEEN 1 AND target_pool_size),
	CHECK (dryness_interaction_mode IN ('disabled','exclusive','coexistence')),
	CHECK (status IN ('generated','awaiting_editorial_review','approved','scheduled','active','paused','retired','rejected','archived')),
	CHECK (moderation_status IN ('pending','approved','restricted','rejected','escalated'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_reaction_pack_version_one_active
	ON humor.reaction_pack_version(reaction_pack_id)
	WHERE status = 'active';

CREATE TABLE IF NOT EXISTS humor.reaction (
	reaction_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	reaction_key text NOT NULL UNIQUE,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS humor.reaction_version (
	reaction_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	reaction_id uuid NOT NULL REFERENCES humor.reaction(reaction_id) ON DELETE RESTRICT,
	version_no integer NOT NULL,
	reaction_label text NOT NULL,
	emoji text,
	style_key text NOT NULL,
	intensity smallint NOT NULL,
	is_reaction_rescue boolean NOT NULL DEFAULT false,
	second_punchline_text text,
	editorial_note text,
	safety_flags jsonb,
	status text NOT NULL DEFAULT 'generated',
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(reaction_id, version_no),
	CHECK (intensity BETWEEN 1 AND 5),
	CHECK (status IN ('generated','awaiting_editorial_review','approved','active','retired','rejected')),
	CHECK (length(trim(reaction_label)) BETWEEN 1 AND 80)
);

CREATE TABLE IF NOT EXISTS humor.reaction_version_mechanism (
	reaction_version_id uuid NOT NULL REFERENCES humor.reaction_version(reaction_version_id) ON DELETE CASCADE,
	reaction_mechanism_id uuid NOT NULL REFERENCES humor.reaction_mechanism(reaction_mechanism_id) ON DELETE RESTRICT,
	PRIMARY KEY (reaction_version_id, reaction_mechanism_id)
);

CREATE TABLE IF NOT EXISTS humor.reaction_pack_item (
	reaction_pack_item_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	reaction_pack_version_id uuid NOT NULL REFERENCES humor.reaction_pack_version(reaction_pack_version_id) ON DELETE CASCADE,
	reaction_version_id uuid NOT NULL REFERENCES humor.reaction_version(reaction_version_id) ON DELETE RESTRICT,
	pool_order_no integer NOT NULL,
	initial_slot_no integer,
	replacement_priority integer,
	is_available_in_more boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(reaction_pack_version_id, pool_order_no),
	UNIQUE(reaction_pack_version_id, reaction_version_id),
	CHECK (pool_order_no > 0),
	CHECK (initial_slot_no IS NULL OR initial_slot_no > 0),
	CHECK (replacement_priority IS NULL OR replacement_priority > 0)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_reaction_pack_item_initial_slot
	ON humor.reaction_pack_item(reaction_pack_version_id, initial_slot_no)
	WHERE initial_slot_no IS NOT NULL;

-- ----------------------------
-- presentation and interaction events
-- ----------------------------
CREATE TABLE IF NOT EXISTS onboarding.selection_decision (
	selection_decision_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	decision_position_no integer NOT NULL,
	scoring_policy_version_id uuid,
	selected_onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE RESTRICT,
	selected_reaction_pack_version_id uuid REFERENCES humor.reaction_pack_version(reaction_pack_version_id) ON DELETE RESTRICT,
	decision_reason text,
	ranking_mode text NOT NULL,
	recovery_mode_enabled boolean NOT NULL DEFAULT false,
	prior_sources jsonb,
	score_components jsonb,
	predicted_enjoyment numeric(6,5),
	predicted_information_gain numeric(6,5),
	confidence numeric(6,5),
	top_n_count smallint,
	correlation_id text,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(onboarding_session_id, decision_position_no),
	CHECK (ranking_mode IN ('exploration','exploitation','balanced','recovery')),
	CHECK (predicted_enjoyment IS NULL OR predicted_enjoyment BETWEEN 0 AND 1),
	CHECK (predicted_information_gain IS NULL OR predicted_information_gain BETWEEN 0 AND 1)
);

CREATE TABLE IF NOT EXISTS onboarding.selection_decision_alternative (
	selection_decision_alternative_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	selection_decision_id uuid NOT NULL REFERENCES onboarding.selection_decision(selection_decision_id) ON DELETE CASCADE,
	rank_no integer NOT NULL,
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE RESTRICT,
	reaction_pack_version_id uuid REFERENCES humor.reaction_pack_version(reaction_pack_version_id) ON DELETE RESTRICT,
	total_score numeric(10,6),
	explanation jsonb,
	UNIQUE(selection_decision_id, rank_no)
);

CREATE TABLE IF NOT EXISTS onboarding.candidate_presentation (
	candidate_presentation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	selection_decision_id uuid REFERENCES onboarding.selection_decision(selection_decision_id) ON DELETE SET NULL,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	position_no integer NOT NULL,
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE RESTRICT,
	reaction_pack_version_id uuid REFERENCES humor.reaction_pack_version(reaction_pack_version_id) ON DELETE RESTRICT,
	displayed_at timestamptz NOT NULL DEFAULT now(),
	finished_at timestamptz,
	scoring_policy_version_id uuid,
	selected_by_reason text,
	ranking_mode text,
	predicted_enjoyment numeric(6,5),
	predicted_information_gain numeric(6,5),
	prior_sources jsonb,
	is_recovery_presentation boolean NOT NULL DEFAULT false,
	UNIQUE(onboarding_session_id, position_no)
);
CREATE INDEX IF NOT EXISTS ix_candidate_presentation_session_time ON onboarding.candidate_presentation(onboarding_session_id, displayed_at);
CREATE INDEX IF NOT EXISTS ix_candidate_presentation_candidate ON onboarding.candidate_presentation(onboarding_candidate_version_id);

CREATE TABLE IF NOT EXISTS onboarding.candidate_feedback (
	candidate_feedback_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_presentation_id uuid NOT NULL REFERENCES onboarding.candidate_presentation(candidate_presentation_id) ON DELETE CASCADE,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	feedback_revision_no integer NOT NULL DEFAULT 1,
	rating_value smallint,
	laugh_score smallint,
	skip_flag boolean,
	not_my_style_flag boolean,
	predictable_flag boolean,
	too_long_flag boolean,
	too_obvious_flag boolean,
	too_intense_flag boolean,
	dry_flag boolean,
	feedback_reason text,
	explicit_text_feedback text,
	submitted_at timestamptz NOT NULL DEFAULT now(),
	is_current boolean NOT NULL DEFAULT true,
	CHECK (rating_value IS NULL OR rating_value BETWEEN 1 AND 5),
	CHECK (laugh_score IS NULL OR laugh_score BETWEEN 1 AND 6)
);
CREATE INDEX IF NOT EXISTS ix_candidate_feedback_presentation ON onboarding.candidate_feedback(candidate_presentation_id, submitted_at DESC);
CREATE UNIQUE INDEX IF NOT EXISTS ux_candidate_feedback_current
	ON onboarding.candidate_feedback(candidate_presentation_id)
	WHERE is_current = true;

CREATE TABLE IF NOT EXISTS humor.reaction_exposure (
	reaction_exposure_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_presentation_id uuid NOT NULL REFERENCES onboarding.candidate_presentation(candidate_presentation_id) ON DELETE CASCADE,
	reaction_pack_item_id uuid NOT NULL REFERENCES humor.reaction_pack_item(reaction_pack_item_id) ON DELETE RESTRICT,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	slot_no integer NOT NULL,
	exposure_source text NOT NULL,
	exposure_sequence_no integer NOT NULL,
	visible_from timestamptz NOT NULL DEFAULT now(),
	visible_to timestamptz,
	idempotency_key text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(candidate_presentation_id, exposure_sequence_no),
	UNIQUE(candidate_presentation_id, idempotency_key),
	UNIQUE(reaction_exposure_id, candidate_presentation_id, reaction_pack_item_id, account_id),
	CHECK (slot_no > 0),
	CHECK (exposure_sequence_no > 0),
	CHECK (visible_to IS NULL OR visible_to >= visible_from),
	CHECK (exposure_source IN ('initial','replacement','more','adaptive_reveal'))
);
CREATE INDEX IF NOT EXISTS ix_reaction_exposure_presentation_time
	ON humor.reaction_exposure(candidate_presentation_id, visible_from);
CREATE INDEX IF NOT EXISTS ix_reaction_exposure_item_time
	ON humor.reaction_exposure(reaction_pack_item_id, visible_from);
CREATE INDEX IF NOT EXISTS ix_reaction_exposure_account_time
	ON humor.reaction_exposure(account_id, visible_from);

CREATE TABLE IF NOT EXISTS humor.reaction_choice (
	reaction_choice_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_presentation_id uuid NOT NULL REFERENCES onboarding.candidate_presentation(candidate_presentation_id) ON DELETE CASCADE,
	reaction_pack_item_id uuid NOT NULL REFERENCES humor.reaction_pack_item(reaction_pack_item_id) ON DELETE RESTRICT,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	source_reaction_exposure_id uuid NOT NULL,
	selection_order_no integer NOT NULL DEFAULT 1,
	selection_context text NOT NULL,
	selected_at timestamptz NOT NULL DEFAULT now(),
	unselected_at timestamptz,
	unselected_reason text,
	idempotency_key text NOT NULL,
	UNIQUE(candidate_presentation_id, account_id, idempotency_key),
	CHECK (selection_order_no > 0),
	CHECK (selection_context IN ('initial','replacement','more','adaptive_reveal')),
	CHECK (unselected_at IS NULL OR unselected_at >= selected_at)
);
ALTER TABLE humor.reaction_choice
	ADD CONSTRAINT fk_reaction_choice_source_exposure
	FOREIGN KEY (source_reaction_exposure_id, candidate_presentation_id, reaction_pack_item_id, account_id)
	REFERENCES humor.reaction_exposure(reaction_exposure_id, candidate_presentation_id, reaction_pack_item_id, account_id)
	ON DELETE RESTRICT;
CREATE INDEX IF NOT EXISTS ix_reaction_choice_presentation ON humor.reaction_choice(candidate_presentation_id, selected_at);
CREATE UNIQUE INDEX IF NOT EXISTS ux_reaction_choice_active_per_item
	ON humor.reaction_choice(candidate_presentation_id, account_id, reaction_pack_item_id)
	WHERE unselected_at IS NULL;

CREATE TABLE IF NOT EXISTS humor.second_punchline_view (
	second_punchline_view_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	reaction_choice_id uuid NOT NULL REFERENCES humor.reaction_choice(reaction_choice_id) ON DELETE CASCADE,
	viewed_at timestamptz NOT NULL DEFAULT now(),
	dwell_ms integer,
	source_action text NOT NULL DEFAULT 'reaction_click',
	CHECK (source_action IN ('reaction_click','auto_expand','manual_expand'))
);
CREATE INDEX IF NOT EXISTS ix_second_punchline_view_choice ON humor.second_punchline_view(reaction_choice_id);

CREATE TABLE IF NOT EXISTS humor.second_punchline_feedback (
	second_punchline_feedback_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	second_punchline_view_id uuid NOT NULL REFERENCES humor.second_punchline_view(second_punchline_view_id) ON DELETE CASCADE,
	feedback_type text NOT NULL,
	feedback_value smallint,
	feedback_text text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (feedback_type IN ('positive','negative','neutral','rating')),
	CHECK (feedback_value IS NULL OR feedback_value BETWEEN 1 AND 5)
);

CREATE TABLE IF NOT EXISTS humor.dryness_scale (
	dryness_scale_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	scale_key text NOT NULL UNIQUE,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS humor.dryness_scale_version (
	dryness_scale_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	dryness_scale_id uuid NOT NULL REFERENCES humor.dryness_scale(dryness_scale_id) ON DELETE CASCADE,
	version_no integer NOT NULL,
	status text NOT NULL DEFAULT 'active',
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(dryness_scale_id, version_no),
	CHECK (version_no > 0),
	CHECK (status IN ('draft','active','retired','archived'))
);

CREATE TABLE IF NOT EXISTS humor.dryness_scale_level (
	dryness_scale_level_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	dryness_scale_version_id uuid NOT NULL REFERENCES humor.dryness_scale_version(dryness_scale_version_id) ON DELETE CASCADE,
	level_no smallint NOT NULL,
	label text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(dryness_scale_version_id, level_no),
	CHECK (level_no BETWEEN 1 AND 6)
);

CREATE TABLE IF NOT EXISTS humor.dryness_rating (
	dryness_rating_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	candidate_presentation_id uuid NOT NULL REFERENCES onboarding.candidate_presentation(candidate_presentation_id) ON DELETE CASCADE,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	dryness_scale_level_id uuid NOT NULL REFERENCES humor.dryness_scale_level(dryness_scale_level_id) ON DELETE RESTRICT,
	selected_at timestamptz NOT NULL DEFAULT now(),
	unselected_at timestamptz,
	idempotency_key text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(candidate_presentation_id, account_id, idempotency_key),
	CHECK (unselected_at IS NULL OR unselected_at >= selected_at)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_dryness_rating_active
	ON humor.dryness_rating(candidate_presentation_id, account_id)
	WHERE unselected_at IS NULL;
CREATE INDEX IF NOT EXISTS ix_dryness_rating_presentation_time
	ON humor.dryness_rating(candidate_presentation_id, selected_at);

-- Default Sucharek 1-6 seed (idempotent for clean bootstrap)
INSERT INTO humor.dryness_scale (scale_key)
SELECT 'default_sucharek'
WHERE NOT EXISTS (
	SELECT 1 FROM humor.dryness_scale WHERE scale_key = 'default_sucharek'
);

INSERT INTO humor.dryness_scale_version (dryness_scale_id, version_no, status)
SELECT ds.dryness_scale_id, 1, 'active'
FROM humor.dryness_scale ds
WHERE ds.scale_key = 'default_sucharek'
AND NOT EXISTS (
	SELECT 1
	FROM humor.dryness_scale_version dsv
	WHERE dsv.dryness_scale_id = ds.dryness_scale_id
	AND dsv.version_no = 1
);

INSERT INTO humor.dryness_scale_level (dryness_scale_version_id, level_no, label)
SELECT dsv.dryness_scale_version_id, v.level_no, v.label
FROM humor.dryness_scale ds
JOIN humor.dryness_scale_version dsv
	ON dsv.dryness_scale_id = ds.dryness_scale_id
	AND dsv.version_no = 1
JOIN (VALUES
	(1::smallint, 'Lekko podsuszony'::text),
	(2::smallint, 'Klasyczny suchar'::text),
	(3::smallint, 'Poproszę wodę'::text),
	(4::smallint, 'Wilgotność 3%'::text),
	(5::smallint, 'Sahara zgłasza plagiat'::text),
	(6::smallint, 'Wezwij cysternę'::text)
) AS v(level_no, label) ON true
WHERE ds.scale_key = 'default_sucharek'
AND NOT EXISTS (
	SELECT 1
	FROM humor.dryness_scale_level l
	WHERE l.dryness_scale_version_id = dsv.dryness_scale_version_id
	AND l.level_no = v.level_no
);

-- ----------------------------
-- working and initial Humor DNA
-- ----------------------------
CREATE TABLE IF NOT EXISTS humor.humor_dimension_definition (
	humor_dimension_definition_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	dimension_key text NOT NULL UNIQUE,
	display_name text NOT NULL,
	dimension_group text,
	is_active boolean NOT NULL DEFAULT true,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS humor.humor_dimension_model_version (
	humor_dimension_model_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	model_key text NOT NULL,
	version_label text NOT NULL,
	status text NOT NULL DEFAULT 'draft',
	config jsonb NOT NULL DEFAULT '{}'::jsonb,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(model_key, version_label),
	CHECK (status IN ('draft','active','retired','archived'))
);

CREATE TABLE IF NOT EXISTS humor.humor_dimension_model_member (
	humor_dimension_model_member_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	humor_dimension_model_version_id uuid NOT NULL REFERENCES humor.humor_dimension_model_version(humor_dimension_model_version_id) ON DELETE CASCADE,
	humor_dimension_definition_id uuid NOT NULL REFERENCES humor.humor_dimension_definition(humor_dimension_definition_id) ON DELETE RESTRICT,
	weight numeric(8,6) NOT NULL DEFAULT 1.0,
	min_supported_value numeric(6,5) NOT NULL DEFAULT 0,
	max_supported_value numeric(6,5) NOT NULL DEFAULT 1,
	member_status text NOT NULL DEFAULT 'active',
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(humor_dimension_model_version_id, humor_dimension_definition_id),
	UNIQUE(humor_dimension_model_member_id, humor_dimension_model_version_id),
	CHECK (weight >= 0),
	CHECK (min_supported_value <= max_supported_value),
	CHECK (member_status IN ('active','retired','experimental'))
);

CREATE TABLE IF NOT EXISTS humor.onboarding_working_humor_dimension (
	onboarding_working_humor_dimension_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	humor_dimension_model_version_id uuid NOT NULL,
	humor_dimension_model_member_id uuid NOT NULL,
	estimated_value numeric(6,5) NOT NULL DEFAULT 0.50000,
	confidence numeric(6,5) NOT NULL DEFAULT 0.10000,
	positive_evidence_count integer NOT NULL DEFAULT 0,
	negative_evidence_count integer NOT NULL DEFAULT 0,
	last_update_source text,
	updated_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(onboarding_session_id, humor_dimension_model_member_id),
	CHECK (estimated_value BETWEEN 0 AND 1),
	CHECK (confidence BETWEEN 0 AND 1)
);
ALTER TABLE humor.onboarding_working_humor_dimension
	ADD CONSTRAINT fk_working_dimension_model_member
	FOREIGN KEY (humor_dimension_model_member_id, humor_dimension_model_version_id)
	REFERENCES humor.humor_dimension_model_member(humor_dimension_model_member_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;
ALTER TABLE humor.onboarding_working_humor_dimension
	ADD CONSTRAINT fk_working_dimension_session_model
	FOREIGN KEY (onboarding_session_id, humor_dimension_model_version_id)
	REFERENCES onboarding.onboarding_session(onboarding_session_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;

CREATE TABLE IF NOT EXISTS humor.onboarding_prior_hypothesis (
	onboarding_prior_hypothesis_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	humor_dimension_model_version_id uuid NOT NULL,
	humor_dimension_model_member_id uuid NOT NULL,
	source_type text NOT NULL,
	account_age_context_id uuid REFERENCES onboarding.account_age_context(account_age_context_id) ON DELETE SET NULL,
	account_profession_context_id uuid REFERENCES onboarding.account_profession_context(account_profession_context_id) ON DELETE SET NULL,
	cohort_definition_version_id uuid,
	estimated_value numeric(6,5) NOT NULL,
	confidence numeric(6,5) NOT NULL,
	applied_weight numeric(6,5) NOT NULL,
	policy_version_id uuid,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (source_type IN ('global_prior','cohort_prior','age_prior','profession_prior')),
	CHECK (estimated_value BETWEEN 0 AND 1),
	CHECK (confidence BETWEEN 0 AND 1),
	CHECK (applied_weight BETWEEN 0 AND 1),
	CHECK (
		(source_type = 'age_prior' AND account_age_context_id IS NOT NULL) OR
		(source_type = 'profession_prior' AND account_profession_context_id IS NOT NULL) OR
		(source_type = 'cohort_prior' AND cohort_definition_version_id IS NOT NULL) OR
		(source_type = 'global_prior')
	),
	CHECK (
		(source_type <> 'age_prior' OR account_profession_context_id IS NULL)
		AND (source_type <> 'profession_prior' OR account_age_context_id IS NULL)
		AND (source_type <> 'global_prior' OR (account_age_context_id IS NULL AND account_profession_context_id IS NULL AND cohort_definition_version_id IS NULL))
	)
);
ALTER TABLE humor.onboarding_prior_hypothesis
	ADD CONSTRAINT fk_prior_member
	FOREIGN KEY (humor_dimension_model_member_id, humor_dimension_model_version_id)
	REFERENCES humor.humor_dimension_model_member(humor_dimension_model_member_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;
ALTER TABLE humor.onboarding_prior_hypothesis
	ADD CONSTRAINT fk_prior_session_model
	FOREIGN KEY (onboarding_session_id, humor_dimension_model_version_id)
	REFERENCES onboarding.onboarding_session(onboarding_session_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;

CREATE TABLE IF NOT EXISTS humor.humor_evidence (
	humor_evidence_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	humor_dimension_model_version_id uuid NOT NULL,
	humor_dimension_model_member_id uuid NOT NULL,
	evidence_type text NOT NULL,
	candidate_presentation_id uuid REFERENCES onboarding.candidate_presentation(candidate_presentation_id) ON DELETE SET NULL,
	candidate_feedback_id uuid REFERENCES onboarding.candidate_feedback(candidate_feedback_id) ON DELETE SET NULL,
	reaction_choice_id uuid REFERENCES humor.reaction_choice(reaction_choice_id) ON DELETE SET NULL,
	second_punchline_feedback_id uuid REFERENCES humor.second_punchline_feedback(second_punchline_feedback_id) ON DELETE SET NULL,
	dryness_rating_id uuid REFERENCES humor.dryness_rating(dryness_rating_id) ON DELETE SET NULL,
	evidence_direction text NOT NULL,
	evidence_weight numeric(6,5) NOT NULL,
	explanation text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (evidence_type IN ('candidate_feedback','reaction_choice','second_punchline_feedback','dryness_rating','meme_calibration','inspiration')),
	CHECK (evidence_direction IN ('positive','negative','neutral')),
	CHECK (evidence_weight BETWEEN 0 AND 1)
);
ALTER TABLE humor.humor_evidence
	ADD CONSTRAINT fk_humor_evidence_member
	FOREIGN KEY (humor_dimension_model_member_id, humor_dimension_model_version_id)
	REFERENCES humor.humor_dimension_model_member(humor_dimension_model_member_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;
ALTER TABLE humor.humor_evidence
	ADD CONSTRAINT fk_humor_evidence_session_model
	FOREIGN KEY (onboarding_session_id, humor_dimension_model_version_id)
	REFERENCES onboarding.onboarding_session(onboarding_session_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;
CREATE INDEX IF NOT EXISTS ix_humor_evidence_session ON humor.humor_evidence(onboarding_session_id, created_at);

CREATE TABLE IF NOT EXISTS humor.initial_humor_snapshot (
	initial_humor_snapshot_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	humor_dimension_model_version_id uuid NOT NULL REFERENCES humor.humor_dimension_model_version(humor_dimension_model_version_id) ON DELETE RESTRICT,
	snapshot_version integer NOT NULL DEFAULT 1,
	supersedes_snapshot_id uuid REFERENCES humor.initial_humor_snapshot(initial_humor_snapshot_id) ON DELETE SET NULL,
	overall_confidence numeric(6,5) NOT NULL,
	confirmed_traits_summary text,
	weak_hypothesis_summary text,
	unexplored_areas_summary text,
	presentation_json jsonb,
	created_at timestamptz NOT NULL DEFAULT now(),
	is_published boolean NOT NULL DEFAULT false,
	published_at timestamptz,
	UNIQUE(onboarding_session_id, snapshot_version),
	UNIQUE(initial_humor_snapshot_id, humor_dimension_model_version_id),
	CHECK (snapshot_version > 0),
	CHECK ((is_published = true AND published_at IS NOT NULL) OR (is_published = false)),
	CHECK (overall_confidence BETWEEN 0 AND 1)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_initial_snapshot_one_published
	ON humor.initial_humor_snapshot(onboarding_session_id)
	WHERE is_published = true;

CREATE TABLE IF NOT EXISTS humor.initial_humor_snapshot_dimension (
	initial_humor_snapshot_dimension_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	initial_humor_snapshot_id uuid NOT NULL REFERENCES humor.initial_humor_snapshot(initial_humor_snapshot_id) ON DELETE CASCADE,
	humor_dimension_model_version_id uuid NOT NULL,
	humor_dimension_model_member_id uuid NOT NULL,
	estimated_value numeric(6,5) NOT NULL,
	confidence numeric(6,5) NOT NULL,
	confirmation_level text NOT NULL,
	evidence_count integer NOT NULL DEFAULT 0,
	UNIQUE(initial_humor_snapshot_id, humor_dimension_model_member_id),
	CHECK (estimated_value BETWEEN 0 AND 1),
	CHECK (confidence BETWEEN 0 AND 1),
	CHECK (confirmation_level IN ('confirmed','weak_hypothesis','unexplored'))
);
ALTER TABLE humor.initial_humor_snapshot_dimension
	ADD CONSTRAINT fk_snapshot_dimension_snapshot_model
	FOREIGN KEY (initial_humor_snapshot_id, humor_dimension_model_version_id)
	REFERENCES humor.initial_humor_snapshot(initial_humor_snapshot_id, humor_dimension_model_version_id)
	ON DELETE CASCADE;
ALTER TABLE humor.initial_humor_snapshot_dimension
	ADD CONSTRAINT fk_snapshot_dimension_member
	FOREIGN KEY (humor_dimension_model_member_id, humor_dimension_model_version_id)
	REFERENCES humor.humor_dimension_model_member(humor_dimension_model_member_id, humor_dimension_model_version_id)
	ON DELETE RESTRICT;

CREATE TABLE IF NOT EXISTS humor.humor_verdict (
	humor_verdict_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	initial_humor_snapshot_id uuid NOT NULL UNIQUE REFERENCES humor.initial_humor_snapshot(initial_humor_snapshot_id) ON DELETE CASCADE,
	verdict_text text NOT NULL,
	language_code text NOT NULL DEFAULT 'pl-PL',
	confidence numeric(6,5),
	generated_by text NOT NULL DEFAULT 'system',
	ai_operation_execution_id uuid,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (generated_by IN ('system','ai','editorial'))
);

-- ==========================================================
-- TIER 2 — REQUIRED FOR EDITORIAL AND LEARNING
-- ==========================================================

CREATE TABLE IF NOT EXISTS onboarding.candidate_classification (
	candidate_classification_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_candidate_version_id uuid NOT NULL UNIQUE REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE CASCADE,
	format_key text,
	intensity smallint,
	dryness smallint,
	complexity smallint,
	universality_score numeric(6,5),
	reaction_rescue_potential numeric(6,5),
	age_hint_strength numeric(6,5),
	profession_hint_strength numeric(6,5),
	safety_flags jsonb,
	additional_tags jsonb,
	CHECK (intensity IS NULL OR intensity BETWEEN 1 AND 5),
	CHECK (dryness IS NULL OR dryness BETWEEN 1 AND 6),
	CHECK (complexity IS NULL OR complexity BETWEEN 1 AND 5)
);

CREATE TABLE IF NOT EXISTS onboarding.recovery_event (
	recovery_event_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	trigger_type text NOT NULL,
	trigger_details jsonb,
	recovery_strategy text NOT NULL,
	selection_decision_id uuid REFERENCES onboarding.selection_decision(selection_decision_id) ON DELETE SET NULL,
	result_type text,
	retained_in_flow boolean,
	improvement_detected boolean,
	recovery_policy_version_id uuid,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (recovery_strategy IN ('change_format','change_mechanism','reduce_intensity','increase_absurdity','neutral_fallback','disable_age_prior','disable_profession_prior','ask_user_direction','shorten_flow'))
);

-- ----------------------------
-- ai
-- ----------------------------
CREATE TABLE IF NOT EXISTS ai.ai_prompt_template (
	ai_prompt_template_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	template_key text NOT NULL UNIQUE,
	description text,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS ai.ai_prompt_template_version (
	ai_prompt_template_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	ai_prompt_template_id uuid NOT NULL REFERENCES ai.ai_prompt_template(ai_prompt_template_id) ON DELETE CASCADE,
	version_label text NOT NULL,
	prompt_body text NOT NULL,
	is_active boolean NOT NULL DEFAULT false,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(ai_prompt_template_id, version_label)
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_ai_prompt_template_one_active
	ON ai.ai_prompt_template_version(ai_prompt_template_id)
	WHERE is_active = true;

CREATE TABLE IF NOT EXISTS ai.ai_generation_target (
	ai_generation_target_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	target_type text NOT NULL,
	target_id uuid,
	onboarding_candidate_version_id uuid REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE SET NULL,
	reaction_pack_version_id uuid REFERENCES humor.reaction_pack_version(reaction_pack_version_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (target_type IN ('candidate_reactions_from_image','candidate_reactions_from_text','verdict_generation','other'))
);

CREATE TABLE IF NOT EXISTS ai.ai_operation_execution (
	ai_operation_execution_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	ai_generation_target_id uuid REFERENCES ai.ai_generation_target(ai_generation_target_id) ON DELETE SET NULL,
	operation_name text NOT NULL,
	correlation_id text,
	provider text NOT NULL,
	model text NOT NULL,
	provider_response_id text,
	ai_prompt_template_version_id uuid REFERENCES ai.ai_prompt_template_version(ai_prompt_template_version_id) ON DELETE SET NULL,
	started_at timestamptz NOT NULL,
	finished_at timestamptz,
	duration_ms integer,
	execution_status text NOT NULL,
	retry_count integer NOT NULL DEFAULT 0,
	repair_count integer NOT NULL DEFAULT 0,
	error_code text,
	image_sha256 text,
	source_material_ref text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (execution_status IN ('succeeded','failed','cancelled','refused'))
);
CREATE INDEX IF NOT EXISTS ix_ai_operation_execution_correlation ON ai.ai_operation_execution(correlation_id);
CREATE INDEX IF NOT EXISTS ix_ai_operation_execution_operation_time ON ai.ai_operation_execution(operation_name, started_at DESC);

CREATE TABLE IF NOT EXISTS ai.ai_operation_usage (
	ai_operation_usage_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	ai_operation_execution_id uuid NOT NULL UNIQUE REFERENCES ai.ai_operation_execution(ai_operation_execution_id) ON DELETE CASCADE,
	input_tokens integer,
	output_tokens integer,
	total_tokens integer,
	created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS ai.ai_generated_output (
	ai_generated_output_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	ai_operation_execution_id uuid NOT NULL REFERENCES ai.ai_operation_execution(ai_operation_execution_id) ON DELETE CASCADE,
	output_type text NOT NULL,
	structured_output jsonb,
	mapped_output jsonb,
	retention_until timestamptz,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (output_type IN ('structured_raw','mapped_reaction_proposal','mapped_verdict','other'))
);

-- ----------------------------
-- studio
-- ----------------------------
CREATE TABLE IF NOT EXISTS studio.account_role (
	account_role_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	role_key text NOT NULL UNIQUE,
	role_name text NOT NULL,
	role_scope text NOT NULL,
	is_active boolean NOT NULL DEFAULT true,
	CHECK (role_scope IN ('studio','system','security'))
);

CREATE TABLE IF NOT EXISTS studio.account_role_assignment (
	account_role_assignment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	account_role_id uuid NOT NULL REFERENCES studio.account_role(account_role_id) ON DELETE RESTRICT,
	assigned_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	assigned_at timestamptz NOT NULL DEFAULT now(),
	revoked_at timestamptz,
	is_active boolean NOT NULL DEFAULT true,
	UNIQUE(account_id, account_role_id, assigned_at)
);
CREATE INDEX IF NOT EXISTS ix_role_assignment_active ON studio.account_role_assignment(account_id) WHERE is_active = true;

CREATE TABLE IF NOT EXISTS studio.editorial_review (
	editorial_review_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	target_type text NOT NULL,
	target_id uuid NOT NULL,
	review_status text NOT NULL,
	reviewer_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	summary text,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	CHECK (target_type IN ('candidate_version','reaction_pack_version','reaction_version')),
	CHECK (review_status IN ('pending','in_review','changes_requested','approved','rejected'))
);

CREATE TABLE IF NOT EXISTS studio.moderation_review (
	moderation_review_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	target_type text NOT NULL,
	target_id uuid NOT NULL,
	moderation_status text NOT NULL,
	moderator_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	risk_level text,
	created_at timestamptz NOT NULL DEFAULT now(),
	updated_at timestamptz NOT NULL DEFAULT now(),
	CHECK (target_type IN ('candidate_version','reaction_pack_version','reaction_version')),
	CHECK (moderation_status IN ('pending','approved','restricted','rejected','escalated'))
);

CREATE TABLE IF NOT EXISTS studio.moderation_decision (
	moderation_decision_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	moderation_review_id uuid NOT NULL REFERENCES studio.moderation_review(moderation_review_id) ON DELETE CASCADE,
	decision_type text NOT NULL,
	decision_reason text,
	moderator_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	decided_at timestamptz NOT NULL DEFAULT now(),
	CHECK (decision_type IN ('approve','restrict','reject','escalate','withdraw'))
);

CREATE TABLE IF NOT EXISTS studio.studio_comment (
	studio_comment_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	target_type text NOT NULL,
	target_id uuid NOT NULL,
	author_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	comment_text text NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	is_internal boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS studio.activation_change (
	activation_change_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	target_type text NOT NULL,
	target_id uuid NOT NULL,
	action_type text NOT NULL,
	changed_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	previous_state jsonb,
	new_state jsonb,
	reason text,
	changed_at timestamptz NOT NULL DEFAULT now(),
	CHECK (action_type IN ('schedule','activate','pause','retire','rollback','kill_switch_on','kill_switch_off'))
);

CREATE TABLE IF NOT EXISTS studio.learning_recommendation (
	learning_recommendation_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	recommendation_type text NOT NULL,
	target_type text NOT NULL,
	target_id uuid,
	evidence_range_start timestamptz,
	evidence_range_end timestamptz,
	sample_size integer,
	confidence numeric(6,5),
	predicted_impact numeric(6,5),
	risk_level text,
	recommendation_payload jsonb,
	recommendation_status text NOT NULL DEFAULT 'proposed',
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (recommendation_type IN ('increase_exposure','reduce_exposure','retire_candidate','refresh_reaction_pack','move_candidate_to_middle','recovery_threshold_change','cohort_pool_gap','stereotype_risk','candidate_fatigue')),
	CHECK (recommendation_status IN ('proposed','under_review','accepted','rejected','executed','expired'))
);

CREATE TABLE IF NOT EXISTS studio.recommendation_decision (
	recommendation_decision_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	learning_recommendation_id uuid NOT NULL REFERENCES studio.learning_recommendation(learning_recommendation_id) ON DELETE CASCADE,
	decided_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	decision_status text NOT NULL,
	decision_reason text,
	executed_action text,
	decided_at timestamptz NOT NULL DEFAULT now(),
	CHECK (decision_status IN ('accepted','rejected','deferred','executed'))
);

-- ----------------------------
-- learning
-- ----------------------------
CREATE TABLE IF NOT EXISTS learning.policy_definition (
	policy_definition_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	policy_key text NOT NULL UNIQUE,
	policy_area text NOT NULL,
	description text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (policy_area IN ('onboarding_flow','scoring','candidate_pool','reaction','recovery','cohort','humor_dimension_model','verdict'))
);

CREATE TABLE IF NOT EXISTS learning.policy_version (
	policy_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	policy_definition_id uuid NOT NULL REFERENCES learning.policy_definition(policy_definition_id) ON DELETE CASCADE,
	version_label text NOT NULL,
	status text NOT NULL DEFAULT 'draft',
	config jsonb NOT NULL DEFAULT '{}'::jsonb,
	activated_at timestamptz,
	retired_at timestamptz,
	created_by_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(policy_definition_id, version_label),
	CHECK (status IN ('draft','active','retired','archived'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_policy_version_one_active
	ON learning.policy_version(policy_definition_id)
	WHERE status = 'active';

CREATE TABLE IF NOT EXISTS learning.cohort_definition (
	cohort_definition_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	cohort_key text NOT NULL UNIQUE,
	cohort_type text NOT NULL,
	description text,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (cohort_type IN ('global','profession','age','behavioral','format','early_interaction'))
);

CREATE TABLE IF NOT EXISTS learning.cohort_definition_version (
	cohort_definition_version_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	cohort_definition_id uuid NOT NULL REFERENCES learning.cohort_definition(cohort_definition_id) ON DELETE CASCADE,
	version_label text NOT NULL,
	status text NOT NULL DEFAULT 'draft',
	definition_rules jsonb NOT NULL,
	minimum_sample_size integer NOT NULL,
	active_from timestamptz,
	active_to timestamptz,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(cohort_definition_id, version_label),
	CHECK (status IN ('draft','active','retired','archived')),
	CHECK (minimum_sample_size >= 30)
);

CREATE TABLE IF NOT EXISTS learning.candidate_performance_snapshot (
	candidate_performance_snapshot_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE CASCADE,
	period_start timestamptz NOT NULL,
	period_end timestamptz NOT NULL,
	sample_size integer NOT NULL,
	exposures bigint NOT NULL,
	unique_users bigint NOT NULL,
	avg_rating numeric(6,5),
	median_rating numeric(6,5),
	skip_rate numeric(6,5),
	not_my_style_rate numeric(6,5),
	completion_effect numeric(6,5),
	abandonment_effect numeric(6,5),
	predicted_vs_actual_error numeric(6,5),
	confidence numeric(6,5),
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (period_end > period_start)
);
CREATE INDEX IF NOT EXISTS ix_candidate_perf_snapshot_candidate_time
	ON learning.candidate_performance_snapshot(onboarding_candidate_version_id, period_start DESC);

CREATE TABLE IF NOT EXISTS learning.reaction_performance_snapshot (
	reaction_performance_snapshot_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	reaction_version_id uuid NOT NULL REFERENCES humor.reaction_version(reaction_version_id) ON DELETE CASCADE,
	period_start timestamptz NOT NULL,
	period_end timestamptz NOT NULL,
	sample_size integer NOT NULL,
	exposures bigint NOT NULL,
	clicks bigint NOT NULL,
	ctr numeric(6,5),
	second_punchline_views bigint,
	explicit_positive_feedback bigint,
	reaction_rescue_cases bigint,
	reaction_rescue_rate numeric(6,5),
	confidence numeric(6,5),
	fatigue_index numeric(6,5),
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (period_end > period_start)
);

CREATE TABLE IF NOT EXISTS learning.cohort_candidate_performance_snapshot (
	cohort_candidate_performance_snapshot_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	cohort_definition_version_id uuid NOT NULL REFERENCES learning.cohort_definition_version(cohort_definition_version_id) ON DELETE CASCADE,
	onboarding_candidate_version_id uuid NOT NULL REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id) ON DELETE CASCADE,
	period_start timestamptz NOT NULL,
	period_end timestamptz NOT NULL,
	sample_size integer NOT NULL,
	exposures bigint NOT NULL,
	avg_rating numeric(6,5),
	skip_rate numeric(6,5),
	completion_rate numeric(6,5),
	confidence numeric(6,5),
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (period_end > period_start)
);

CREATE TABLE IF NOT EXISTS learning.onboarding_funnel_snapshot (
	onboarding_funnel_snapshot_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	policy_version_id uuid REFERENCES learning.policy_version(policy_version_id) ON DELETE SET NULL,
	period_start timestamptz NOT NULL,
	period_end timestamptz NOT NULL,
	source_registration text,
	started_count bigint NOT NULL,
	completed_count bigint NOT NULL,
	abandoned_count bigint NOT NULL,
	avg_time_to_complete_sec numeric(10,2),
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (period_end > period_start)
);

CREATE TABLE IF NOT EXISTS learning.recovery_performance_snapshot (
	recovery_performance_snapshot_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	recovery_strategy text NOT NULL,
	period_start timestamptz NOT NULL,
	period_end timestamptz NOT NULL,
	trigger_count bigint NOT NULL,
	retained_in_flow_count bigint NOT NULL,
	improvement_detected_count bigint NOT NULL,
	success_rate numeric(6,5),
	confidence numeric(6,5),
	sample_size integer NOT NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (period_end > period_start)
);

CREATE TABLE IF NOT EXISTS learning.prediction_outcome (
	prediction_outcome_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	selection_decision_id uuid NOT NULL REFERENCES onboarding.selection_decision(selection_decision_id) ON DELETE CASCADE,
	predicted_enjoyment numeric(6,5),
	predicted_information_gain numeric(6,5),
	actual_enjoyment numeric(6,5),
	actual_information_gain numeric(6,5),
	completed_flow boolean,
	mismatch_flag boolean,
	created_at timestamptz NOT NULL DEFAULT now()
);

-- ----------------------------
-- audit
-- ----------------------------
CREATE TABLE IF NOT EXISTS audit.audit_event (
	audit_event_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	actor_account_id uuid REFERENCES identity.account(account_id) ON DELETE SET NULL,
	actor_role_key text,
	action_key text NOT NULL,
	target_type text NOT NULL,
	target_id uuid,
	previous_state jsonb,
	new_state jsonb,
	reason text,
	correlation_id text,
	source_application text,
	metadata jsonb,
	created_at timestamptz NOT NULL DEFAULT now()
);
CREATE INDEX IF NOT EXISTS ix_audit_event_target ON audit.audit_event(target_type, target_id, created_at DESC);
CREATE INDEX IF NOT EXISTS ix_audit_event_actor ON audit.audit_event(actor_account_id, created_at DESC);

CREATE OR REPLACE FUNCTION humor.fn_enforce_reaction_choice_dryness_exclusive()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_dryness_mode text;
BEGIN
	IF NEW.unselected_at IS NOT NULL THEN
		RETURN NEW;
	END IF;

	PERFORM 1
	FROM onboarding.candidate_presentation cp
	WHERE cp.candidate_presentation_id = NEW.candidate_presentation_id
	FOR UPDATE;

	SELECT rpv.dryness_interaction_mode
	INTO v_dryness_mode
	FROM onboarding.candidate_presentation cp
	JOIN humor.reaction_pack_version rpv
		ON rpv.reaction_pack_version_id = cp.reaction_pack_version_id
	WHERE cp.candidate_presentation_id = NEW.candidate_presentation_id;

	IF COALESCE(v_dryness_mode, 'exclusive') <> 'exclusive' THEN
		RETURN NEW;
	END IF;

	IF EXISTS (
		SELECT 1
		FROM humor.dryness_rating dr
		WHERE dr.candidate_presentation_id = NEW.candidate_presentation_id
			AND dr.account_id = NEW.account_id
			AND dr.unselected_at IS NULL
	) THEN
		RAISE EXCEPTION 'exclusive dryness mode: active dryness rating blocks active reaction choice';
	END IF;

	RETURN NEW;
END;
$$;

CREATE TRIGGER tr_reaction_choice_enforce_dryness_exclusive
	BEFORE INSERT OR UPDATE ON humor.reaction_choice
	FOR EACH ROW
	EXECUTE FUNCTION humor.fn_enforce_reaction_choice_dryness_exclusive();

CREATE OR REPLACE FUNCTION humor.fn_enforce_dryness_rating_reaction_exclusive()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_dryness_mode text;
BEGIN
	IF NEW.unselected_at IS NOT NULL THEN
		RETURN NEW;
	END IF;

	PERFORM 1
	FROM onboarding.candidate_presentation cp
	WHERE cp.candidate_presentation_id = NEW.candidate_presentation_id
	FOR UPDATE;

	SELECT rpv.dryness_interaction_mode
	INTO v_dryness_mode
	FROM onboarding.candidate_presentation cp
	JOIN humor.reaction_pack_version rpv
		ON rpv.reaction_pack_version_id = cp.reaction_pack_version_id
	WHERE cp.candidate_presentation_id = NEW.candidate_presentation_id;

	IF COALESCE(v_dryness_mode, 'exclusive') <> 'exclusive' THEN
		RETURN NEW;
	END IF;

	IF EXISTS (
		SELECT 1
		FROM humor.reaction_choice rc
		WHERE rc.candidate_presentation_id = NEW.candidate_presentation_id
			AND rc.account_id = NEW.account_id
			AND rc.unselected_at IS NULL
	) THEN
		RAISE EXCEPTION 'exclusive dryness mode: active reaction choice blocks active dryness rating';
	END IF;

	RETURN NEW;
END;
$$;

CREATE TRIGGER tr_dryness_rating_enforce_reaction_exclusive
	BEFORE INSERT OR UPDATE ON humor.dryness_rating
	FOR EACH ROW
	EXECUTE FUNCTION humor.fn_enforce_dryness_rating_reaction_exclusive();

CREATE OR REPLACE FUNCTION humor.fn_prevent_published_snapshot_row_change()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
	IF (TG_OP = 'DELETE' AND OLD.is_published = true)
		OR (TG_OP = 'UPDATE' AND OLD.is_published = true) THEN
		RAISE EXCEPTION 'published initial_humor_snapshot is immutable';
	END IF;

	RETURN CASE WHEN TG_OP = 'DELETE' THEN OLD ELSE NEW END;
END;
$$;

CREATE TRIGGER tr_initial_humor_snapshot_immutable_published
	BEFORE UPDATE OR DELETE ON humor.initial_humor_snapshot
	FOR EACH ROW
	EXECUTE FUNCTION humor.fn_prevent_published_snapshot_row_change();

CREATE OR REPLACE FUNCTION humor.fn_prevent_published_snapshot_children_change()
RETURNS trigger
LANGUAGE plpgsql
AS $$
DECLARE
	v_snapshot_id uuid;
	v_is_published boolean;
BEGIN
	IF TG_OP = 'DELETE' THEN
		v_snapshot_id := OLD.initial_humor_snapshot_id;
	ELSE
		v_snapshot_id := NEW.initial_humor_snapshot_id;
	END IF;

	SELECT is_published
	INTO v_is_published
	FROM humor.initial_humor_snapshot s
	WHERE s.initial_humor_snapshot_id = v_snapshot_id;

	IF v_is_published = true THEN
		RAISE EXCEPTION 'published initial_humor_snapshot children are immutable';
	END IF;

	RETURN CASE WHEN TG_OP = 'DELETE' THEN OLD ELSE NEW END;
END;
$$;

CREATE TRIGGER tr_initial_snapshot_dimension_immutable_published
	BEFORE INSERT OR UPDATE OR DELETE ON humor.initial_humor_snapshot_dimension
	FOR EACH ROW
	EXECUTE FUNCTION humor.fn_prevent_published_snapshot_children_change();

CREATE TRIGGER tr_humor_verdict_immutable_published
	BEFORE INSERT OR UPDATE OR DELETE ON humor.humor_verdict
	FOR EACH ROW
	EXECUTE FUNCTION humor.fn_prevent_published_snapshot_children_change();

CREATE OR REPLACE FUNCTION audit.fn_block_audit_event_mutation()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
	RAISE EXCEPTION 'audit.audit_event is append-oriented; UPDATE/DELETE is forbidden';
	RETURN NULL;
END;
$$;

CREATE TRIGGER tr_block_audit_event_update
	BEFORE UPDATE ON audit.audit_event
	FOR EACH ROW
	EXECUTE FUNCTION audit.fn_block_audit_event_mutation();

CREATE TRIGGER tr_block_audit_event_delete
	BEFORE DELETE ON audit.audit_event
	FOR EACH ROW
	EXECUTE FUNCTION audit.fn_block_audit_event_mutation();

COMMENT ON TABLE audit.audit_event IS 'Append-oriented audit table. UPDATE/DELETE blocked by trigger. INSERT-only role grants are deployment/infrastructure concern.';

-- ==========================================================
-- TIER 3 — PREPARED HOOKS / FUTURE
-- ==========================================================

CREATE TABLE IF NOT EXISTS humor.meme_calibration_session (
	meme_calibration_session_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid NOT NULL REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE CASCADE,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	media_asset_id uuid REFERENCES onboarding.media_asset(media_asset_id) ON DELETE SET NULL,
	user_declared_funny boolean,
	mechanism_analysis jsonb,
	moderation_status text NOT NULL DEFAULT 'pending',
	privacy_status text NOT NULL DEFAULT 'private',
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (moderation_status IN ('pending','approved','restricted','rejected')),
	CHECK (privacy_status IN ('private','studio_only','public_opt_in'))
);

CREATE TABLE IF NOT EXISTS humor.meme_calibration_variant (
	meme_calibration_variant_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	meme_calibration_session_id uuid NOT NULL REFERENCES humor.meme_calibration_session(meme_calibration_session_id) ON DELETE CASCADE,
	variant_no integer NOT NULL,
	variant_text text,
	variant_style text,
	ai_operation_execution_id uuid REFERENCES ai.ai_operation_execution(ai_operation_execution_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now(),
	UNIQUE(meme_calibration_session_id, variant_no)
);

CREATE TABLE IF NOT EXISTS humor.meme_calibration_feedback (
	meme_calibration_feedback_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	meme_calibration_variant_id uuid NOT NULL REFERENCES humor.meme_calibration_variant(meme_calibration_variant_id) ON DELETE CASCADE,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	rating_value smallint,
	selected_final boolean,
	impact_weight numeric(6,5),
	created_at timestamptz NOT NULL DEFAULT now(),
	CHECK (rating_value IS NULL OR rating_value BETWEEN 1 AND 5),
	CHECK (impact_weight IS NULL OR impact_weight BETWEEN 0 AND 1)
);

CREATE TABLE IF NOT EXISTS humor.user_humor_inspiration (
	user_humor_inspiration_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	onboarding_session_id uuid REFERENCES onboarding.onboarding_session(onboarding_session_id) ON DELETE SET NULL,
	account_id uuid NOT NULL REFERENCES identity.account(account_id) ON DELETE CASCADE,
	inspiration_type text NOT NULL,
	language_code text NOT NULL DEFAULT 'pl-PL',
	content_text text NOT NULL,
	privacy_status text NOT NULL DEFAULT 'private',
	created_at timestamptz NOT NULL DEFAULT now(),
	removed_at timestamptz,
	CHECK (inspiration_type IN ('punchline','retort','short_inspiration','humorous_answer')),
	CHECK (privacy_status IN ('private','studio_only','public_opt_in'))
);

CREATE TABLE IF NOT EXISTS humor.user_humor_inspiration_analysis (
	user_humor_inspiration_analysis_id uuid PRIMARY KEY DEFAULT gen_random_uuid(),
	user_humor_inspiration_id uuid NOT NULL REFERENCES humor.user_humor_inspiration(user_humor_inspiration_id) ON DELETE CASCADE,
	style_analysis jsonb,
	humor_dimension_impact jsonb,
	ai_operation_execution_id uuid REFERENCES ai.ai_operation_execution(ai_operation_execution_id) ON DELETE SET NULL,
	created_at timestamptz NOT NULL DEFAULT now()
);

-- ==========================================================
-- Cross-schema late FKs and optimization indexes
-- ==========================================================

ALTER TABLE onboarding.onboarding_session
	ADD CONSTRAINT fk_onboarding_session_scoring_policy
	FOREIGN KEY (scoring_policy_version_id)
	REFERENCES learning.policy_version(policy_version_id)
	ON DELETE SET NULL;

ALTER TABLE onboarding.onboarding_session
	ADD CONSTRAINT fk_onboarding_session_humor_dimension_model_version
	FOREIGN KEY (humor_dimension_model_version_id)
	REFERENCES humor.humor_dimension_model_version(humor_dimension_model_version_id)
	ON DELETE RESTRICT;

ALTER TABLE onboarding.selection_decision
	ADD CONSTRAINT fk_selection_decision_scoring_policy
	FOREIGN KEY (scoring_policy_version_id)
	REFERENCES learning.policy_version(policy_version_id)
	ON DELETE SET NULL;

ALTER TABLE onboarding.candidate_presentation
	ADD CONSTRAINT fk_candidate_presentation_scoring_policy
	FOREIGN KEY (scoring_policy_version_id)
	REFERENCES learning.policy_version(policy_version_id)
	ON DELETE SET NULL;

ALTER TABLE onboarding.recovery_event
	ADD CONSTRAINT fk_recovery_event_policy_version
	FOREIGN KEY (recovery_policy_version_id)
	REFERENCES learning.policy_version(policy_version_id)
	ON DELETE SET NULL;

ALTER TABLE humor.onboarding_prior_hypothesis
	ADD CONSTRAINT fk_prior_hypothesis_cohort_definition_version
	FOREIGN KEY (cohort_definition_version_id)
	REFERENCES learning.cohort_definition_version(cohort_definition_version_id)
	ON DELETE SET NULL;

ALTER TABLE humor.onboarding_prior_hypothesis
	ADD CONSTRAINT fk_prior_hypothesis_policy_version
	FOREIGN KEY (policy_version_id)
	REFERENCES learning.policy_version(policy_version_id)
	ON DELETE SET NULL;

ALTER TABLE onboarding.candidate_activation
	ADD CONSTRAINT fk_candidate_activation_cohort
	FOREIGN KEY (cohort_definition_version_id)
	REFERENCES learning.cohort_definition_version(cohort_definition_version_id)
	ON DELETE SET NULL;

ALTER TABLE humor.reaction_pack_version
	ADD CONSTRAINT fk_reaction_pack_version_prompt
	FOREIGN KEY (prompt_template_version_id)
	REFERENCES ai.ai_prompt_template_version(ai_prompt_template_version_id)
	ON DELETE SET NULL;

ALTER TABLE humor.humor_verdict
	ADD CONSTRAINT fk_humor_verdict_ai_execution
	FOREIGN KEY (ai_operation_execution_id)
	REFERENCES ai.ai_operation_execution(ai_operation_execution_id)
	ON DELETE SET NULL;

ALTER TABLE onboarding.onboarding_candidate
	ADD CONSTRAINT fk_onboarding_candidate_approved_version
	FOREIGN KEY (approved_version_id)
	REFERENCES onboarding.onboarding_candidate_version(onboarding_candidate_version_id)
	ON DELETE SET NULL;

CREATE INDEX IF NOT EXISTS ix_candidate_feedback_account_time
	ON onboarding.candidate_feedback(account_id, submitted_at DESC);
CREATE INDEX IF NOT EXISTS ix_reaction_choice_account_time
	ON humor.reaction_choice(account_id, selected_at DESC);
CREATE INDEX IF NOT EXISTS ix_ai_generated_output_execution
	ON ai.ai_generated_output(ai_operation_execution_id, created_at DESC);

COMMENT ON COLUMN ai.ai_generation_target.target_id IS 'Intentionally polymorphic typed-id. target_type determines concrete aggregate; no generic FK by design.';
COMMENT ON COLUMN studio.editorial_review.target_id IS 'Typed target pointer; enforced by target_type and application workflow, no single relational FK available.';
COMMENT ON COLUMN studio.moderation_review.target_id IS 'Typed target pointer; enforced by target_type and application workflow, no single relational FK available.';
COMMENT ON COLUMN studio.studio_comment.target_id IS 'Typed target pointer; intentionally loose UUID to support multiple reviewed aggregates.';
COMMENT ON COLUMN studio.activation_change.target_id IS 'Typed target pointer; intentionally loose UUID to support activation history across aggregates.';
COMMENT ON COLUMN studio.learning_recommendation.target_id IS 'Typed target pointer for recommendation scope; generic FK intentionally deferred.';
COMMENT ON COLUMN audit.audit_event.target_id IS 'Audit target pointer; intentionally polymorphic and resolved via target_type.';

COMMENT ON SCHEMA identity IS 'Tożsamość i dostęp konta użytkownika HAIA.';
COMMENT ON SCHEMA onboarding IS 'Stan i przebieg onboardingu oraz biblioteka kandydatów.';
COMMENT ON SCHEMA humor IS 'Reakcje, puenty, sygnały humoru i snapshot Initial Humor DNA.';
COMMENT ON SCHEMA studio IS 'Redakcja, moderacja, aktywacja i decyzje operacyjne Onboarding Studio.';
COMMENT ON SCHEMA learning IS 'Polityki, kohorty i snapshoty agregatów population learning.';
COMMENT ON SCHEMA ai IS 'Ślad operacji AI i artefaktów wygenerowanych technicznie.';
COMMENT ON SCHEMA audit IS 'Append-oriented audyt działań i zmian stanów.';
