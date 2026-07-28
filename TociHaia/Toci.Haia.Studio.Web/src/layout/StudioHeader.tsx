import styles from './studio-layout.module.css'
import { ApiError } from '../shared/api/apiError'
import { useStudioAuthStatusQuery, useStudioLogoutMutation } from '../features/auth/api/studioAuthQueries'
import { useNavigate } from 'react-router-dom'

const logoHref = new URL('../../assets/logo.png', import.meta.url).href

export function StudioHeader(): React.ReactElement {
	const navigate = useNavigate()
	const authStatusQuery = useStudioAuthStatusQuery()
	const logoutMutation = useStudioLogoutMutation()

	const isAuthenticated = authStatusQuery.isSuccess

	const onLogoutClick = async (): Promise<void> => {
		try {
			await logoutMutation.mutateAsync()
		} catch (error) {
			if (error instanceof ApiError && error.status === 401) {
				// already signed out on server side
			}
		}

		navigate('/login', { replace: true })
	}

	return (
		<header className={styles.header}>
			<div className={styles.headerInner}>
				<div className={styles.brand}>
					<img className={styles.logo} src={logoHref} alt="Logo HAIA" />
					<div className={styles.brandText}>
						<strong className={styles.brandTitle}>HAIA Studio</strong>
						<span className={styles.brandSubtitle}>Portal redakcyjno-administracyjny</span>
					</div>
				</div>
				<div className={styles.headerActions}>
					{isAuthenticated ? (
						<>
							<p className={styles.headerNotice}>Zalogowano: {authStatusQuery.data.email}</p>
							<button type="button" className={styles.logoutButton} onClick={() => void onLogoutClick()} disabled={logoutMutation.isPending}>
								{logoutMutation.isPending ? 'Wylogowywanie...' : 'Wyloguj'}
							</button>
						</>
					) : (
						<p className={styles.headerNotice}>Sesja wymagana do pracy w HAIA Studio.</p>
					)}
				</div>
			</div>
		</header>
	)
}
