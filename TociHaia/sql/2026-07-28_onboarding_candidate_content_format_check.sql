-- Rozszerzenie CHECK dla onboarding_candidate.content_format
-- Cel: akceptować MIME używane przez Studio API: image/jpeg, image/png, image/webp.

DO $$
DECLARE
	v_schema text;
BEGIN
	SELECT n.nspname
	  INTO v_schema
	  FROM pg_constraint c
	  JOIN pg_class t ON t.oid = c.conrelid
	  JOIN pg_namespace n ON n.oid = t.relnamespace
	 WHERE c.conname = 'onboarding_candidate_content_format_check'
	 LIMIT 1;

	IF v_schema IS NULL THEN
		RAISE EXCEPTION 'Nie znaleziono constraintu onboarding_candidate_content_format_check';
	END IF;

	EXECUTE format(
		'ALTER TABLE %I.onboarding_candidate DROP CONSTRAINT IF EXISTS onboarding_candidate_content_format_check',
		v_schema);

	EXECUTE format(
		'ALTER TABLE %I.onboarding_candidate ADD CONSTRAINT onboarding_candidate_content_format_check CHECK (content_format IN (''image/jpeg'', ''image/png'', ''image/webp''))',
		v_schema);
END
$$;