import { useMemo, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { ApiError } from '../../../shared/api/apiError'
import { Button } from '../../../shared/components/Button/Button'
import { useVerifyMutation } from '../api/registrationQueries'
import styles from '../registration.module.css'

type VerifyState = 'missing' | 'ready' | 'submitting' | 'verified' | 'invalid' | 'error'

function maskToken(token: string): string {
	if (token.length <= 8) {
		return '\u2022\u2022\u2022\u2022'
	}

	return `${token.slice(0, 4)}\u2022\u2022\u2022\u2022${token.slice(-4)}`
}

export function VerifyEmailPage(): React.ReactElement {
	const [searchParams] = useSearchParams()
	const mutation = useVerifyMutation()
	const [state, setState] = useState<VerifyState>('ready')
	const [message, setMessage] = useState<string | undefined>()

	const token = useMemo(() => searchParams.get('token')?.trim() ?? '', [searchParams])
	const masked = useMemo(() => (token ? maskToken(token) : ''), [token])

	const onVerify = async () => {
		if (!token) {
			setState('missing')
			setMessage('Brakuje tokenu potwierdzenia w linku.')
			return
		}

		setState('submitting')
		setMessage(undefined)

		try {
			await mutation.mutateAsync({ token })
			setState('verified')
			setMessage('Teraz mo\u017cemy zacz\u0105\u0107 sprawdza\u0107, co naprawd\u0119 Ci\u0119 bawi.')
		} catch (error: unknown) {
			if (error instanceof ApiError && error.code === 'invalid_or_expired_verification_token') {
				setState('invalid')
				setMessage('Token jest nieprawid\u0142owy lub wygas\u0142.')
				return
			}

			setState('error')
			setMessage('Nie uda\u0142o si\u0119 uko\u0144czy\u0107 operacji. Spr\u00f3buj ponownie.')
		}
	}

	return (
		<main className={styles.formPane}>
			<section className={styles.formCard}>
				<h1 tabIndex={-1}>{state === 'verified' ? 'Konto aktywne. Kontakt z baz\u0105 nawi\u0105zany.' : 'Potwierd\u017a adres e-mail'}</h1>
				{token ? <p className={styles.secondaryText}>Token: {masked}</p> : null}
				{message ? <div className={state === 'verified' ? styles.successBox : styles.errorBox}>{message}</div> : null}

				<div className={styles.actions}>
					{state !== 'verified' ? (
						<Button type="button" onClick={onVerify} disabled={state === 'submitting'}>
							{state === 'submitting' ? 'Potwierdzamy\u2026' : 'Potwierd\u017a adres'}
						</Button>
					) : (
						<Button type="button" variant="secondary" disabled>
							{'Kalibracja ju\u017c wkr\u00f3tce'}
						</Button>
					)}
				</div>
			</section>
		</main>
	)
}

