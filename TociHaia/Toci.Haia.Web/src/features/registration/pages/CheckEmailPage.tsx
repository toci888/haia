import { useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { ApiError } from '../../../shared/api/apiError'
import { Button } from '../../../shared/components/Button/Button'
import { useResendMutation } from '../api/registrationQueries'
import styles from '../registration.module.css'

export function CheckEmailPage(): React.ReactElement {
	const location = useLocation()
	const navigate = useNavigate()
	const resendMutation = useResendMutation()
	const [emailInput, setEmailInput] = useState<string>(location.state?.email ?? '')
	const [successMessage, setSuccessMessage] = useState<string | undefined>()
	const [errorMessage, setErrorMessage] = useState<string | undefined>()

	const resend = async () => {
		setSuccessMessage(undefined)
		setErrorMessage(undefined)

		if (!emailInput.trim()) {
			setErrorMessage('Podaj adres e-mail, aby ponowi\u0107 wysy\u0142k\u0119.')
			return
		}

		try {
			await resendMutation.mutateAsync({
				email: emailInput.trim().toLowerCase(),
				language: 'pl-PL',
			})

			setSuccessMessage('Je\u017celi dla podanego adresu mo\u017cna rozpocz\u0105\u0107 lub doko\u0144czy\u0107 rejestracj\u0119, wiadomo\u015b\u0107 z potwierdzeniem trafi do skrzynki.')
		} catch (error: unknown) {
			if (error instanceof ApiError && error.status === 429) {
				setErrorMessage('Zbyt wiele pr\u00f3b. Poczekaj chwil\u0119 i spr\u00f3buj ponownie.')
				return
			}

			setErrorMessage('Nie uda\u0142o si\u0119 uko\u0144czy\u0107 operacji. Spr\u00f3buj ponownie.')
		}
	}

	return (
		<main className={styles.formPane}>
			<section className={styles.formCard}>
				<h1 tabIndex={-1}>{'Sprawd\u017a skrzynk\u0119'}</h1>
				<p>
					{'Je\u017celi dla podanego adresu mo\u017cna rozpocz\u0105\u0107 lub doko\u0144czy\u0107 rejestracj\u0119, wiadomo\u015b\u0107 z potwierdzeniem trafi do skrzynki. Sprawd\u017a r\u00f3wnie\u017c folder spam.'}
				</p>

				<div style={{ marginTop: '1rem' }}>
					<label htmlFor="resend-email">Adres e-mail</label>
					<input
						id="resend-email"
						type="email"
						autoComplete="email"
						className={styles.input}
						value={emailInput}
						onChange={(event) => setEmailInput(event.target.value)}
					/>
				</div>

				{successMessage ? <div className={styles.successBox}>{successMessage}</div> : null}
				{errorMessage ? <div className={styles.errorBox}>{errorMessage}</div> : null}

				<div className={styles.actions}>
					<Button type="button" onClick={resend} disabled={resendMutation.isPending}>
						{'Wy\u015blij wiadomo\u015b\u0107 ponownie'}
					</Button>
					<Button type="button" variant="secondary" onClick={() => navigate('/register')}>
						{'Wr\u00f3\u0107 i popraw adres'}
					</Button>
				</div>
			</section>
		</main>
	)
}

