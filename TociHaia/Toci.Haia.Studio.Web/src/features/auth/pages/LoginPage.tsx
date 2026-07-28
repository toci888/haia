import { useState } from 'react'
import { Navigate, useLocation, useNavigate } from 'react-router-dom'
import { StudioLayout } from '../../../layout/StudioLayout'
import { ApiError } from '../../../shared/api/apiError'
import { useStudioAuthStatusQuery, useStudioLoginMutation } from '../api/studioAuthQueries'
import styles from './login-page.module.css'

type LocationState = {
	from?: {
		pathname?: string
	}
}

function readSafeAuthMessage(error: ApiError): string {
	if (error.code === 'invalid_credentials') {
		return 'Nie udało się zalogować. Sprawdź dane albo skontaktuj się z administratorem.'
	}

	if (error.code === 'csrf_validation_failed') {
		return 'Sesja formularza wygasła. Odśwież stronę i spróbuj ponownie.'
	}

	return 'Logowanie nie powiodło się. Spróbuj ponownie za chwilę.'
}

export function LoginPage(): React.ReactElement {
	const authStatusQuery = useStudioAuthStatusQuery()
	const loginMutation = useStudioLoginMutation()
	const location = useLocation()
	const navigate = useNavigate()
	const [email, setEmail] = useState('')
	const [password, setPassword] = useState('')
	const [submitError, setSubmitError] = useState<string | null>(null)

	const locationState = location.state as LocationState | undefined
	const redirectTo = locationState?.from?.pathname === '/login'
		? '/system'
		: (locationState?.from?.pathname || '/system')

	if (authStatusQuery.isSuccess) {
		return <Navigate to={redirectTo} replace />
	}

	const isSubmitting = loginMutation.isPending

	const onSubmit = async (event: React.FormEvent<HTMLFormElement>): Promise<void> => {
		event.preventDefault()
		setSubmitError(null)

		try {
			await loginMutation.mutateAsync({ email, password })
			navigate(redirectTo, { replace: true })
		} catch (error) {
			if (error instanceof ApiError) {
				setSubmitError(readSafeAuthMessage(error))
				return
			}

			setSubmitError('Logowanie nie powiodło się. Spróbuj ponownie za chwilę.')
		}
	}

	return (
		<StudioLayout>
			<section className={styles.page} aria-labelledby="login-title">
				<div className={styles.card}>
					<h1 id="login-title" className={styles.heading}>Logowanie do HAIA Studio</h1>
					<p className={styles.description}>Zaloguj się kontem administratora Studio. Sesja działa na bezpiecznym cookie HttpOnly.</p>
					<form className={styles.form} onSubmit={(event) => void onSubmit(event)}>
						<label className={styles.label} htmlFor="studio-email">Email</label>
						<input
							id="studio-email"
							type="email"
							autoComplete="username"
							value={email}
							onChange={(event) => setEmail(event.target.value)}
							required
						/>

						<label className={styles.label} htmlFor="studio-password">Hasło</label>
						<input
							id="studio-password"
							type="password"
							autoComplete="current-password"
							value={password}
							onChange={(event) => setPassword(event.target.value)}
							required
						/>

						{submitError ? <p role="alert" className={styles.error}>{submitError}</p> : null}

						<button type="submit" className={styles.submitButton} disabled={isSubmitting}>
							{isSubmitting ? 'Logowanie...' : 'Zaloguj'}
						</button>
					</form>
				</div>
			</section>
		</StudioLayout>
	)
}
