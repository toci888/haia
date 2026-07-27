import styles from '../registration.module.css'
import { RegistrationBrandPanel } from '../components/RegistrationBrandPanel'
import { RegistrationForm } from '../components/RegistrationForm'

export function RegistrationPage(): React.ReactElement {
	return (
		<main className={styles.layout}>
			<RegistrationBrandPanel />
			<section className={styles.formPane}>
				<div className={styles.formCard}>
					<RegistrationForm />
				</div>
			</section>
		</main>
	)
}

