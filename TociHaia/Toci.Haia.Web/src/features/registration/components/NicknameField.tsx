import { useEffect, useMemo, useState } from 'react'
import { useNicknameAvailability } from '../api/registrationQueries'
import { nicknameSchema } from '../validation/registrationSchema'

type Props = {
	value: string
	onChange: (value: string) => void
	error?: string
	onUnavailable: (message?: string) => void
}

export function NicknameField({ value, onChange, error, onUnavailable }: Props): React.ReactElement {
	const [debounced, setDebounced] = useState('')

	const normalizedCandidate = useMemo(() => value.trim(), [value])
	const localValid = normalizedCandidate.length > 0 && nicknameSchema.safeParse(normalizedCandidate).success

	useEffect(() => {
		const nextValue = localValid ? normalizedCandidate : ''
		const handle = setTimeout(() => setDebounced(nextValue), 500)
		return () => clearTimeout(handle)
	}, [normalizedCandidate, localValid])

	const query = useNicknameAvailability(debounced, debounced.length > 0)

	useEffect(() => {
		if (!debounced || query.isLoading) {
			onUnavailable(undefined)
			return
		}

		if (query.data && !query.data.available) {
			onUnavailable('Ten pseudonim jest juĹĽ zajÄ™ty')
			return
		}

		onUnavailable(undefined)
	}, [debounced, onUnavailable, query.data, query.isLoading])

	return (
		<div style={{ marginBottom: '1rem' }}>
			<label htmlFor="nickname" style={{ display: 'block', marginBottom: '0.35rem', fontWeight: 600 }}>
				Pseudonim
			</label>
			<input
				id="nickname"
				type="text"
				value={value}
				onChange={(e) => onChange(e.target.value)}
				aria-invalid={Boolean(error)}
				style={{ width: '100%', height: '2.75rem', borderRadius: '12px', border: '1px solid var(--haia-border)', padding: '0 0.75rem' }}
			/>
			{error ? <div role="alert" style={{ color: 'var(--haia-error)' }}>{error}</div> : null}
			{!error && !localValid && normalizedCandidate.length > 0 ? <div style={{ color: 'var(--haia-error)' }}>NieprawidĹ‚owy pseudonim</div> : null}
			{localValid && query.isLoading ? <div aria-live="polite">Sprawdzamy pseudonimâ€¦</div> : null}
			{localValid && query.data?.available ? <div aria-live="polite" style={{ color: 'var(--haia-success)' }}>Pseudonim jest dostÄ™pny</div> : null}
			{localValid && query.data && !query.data.available ? <div aria-live="polite" style={{ color: 'var(--haia-error)' }}>Ten pseudonim jest juĹĽ zajÄ™ty</div> : null}
			{localValid && query.isError ? <div aria-live="polite" style={{ color: 'var(--haia-muted)' }}>Nie udaĹ‚o siÄ™ teraz sprawdziÄ‡ dostÄ™pnoĹ›ci</div> : null}
		</div>
	)
}

