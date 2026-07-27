import styles from '../registration.module.css'

export function RegistrationBrandPanel(): React.ReactElement {
	const logoSrc = new URL('../../../../assets/logo.png', import.meta.url).href

	return (
		<section className={styles.branding} aria-label="Branding HAIA">
			<h2 className={styles.brandWordmark}>HAIA</h2>
			<p>{'Świat humoru dopasowany do Ciebie. Zaczynamy od bezpiecznego startu konta.'}</p>
			<img className={styles.brandLogo} src={logoSrc} alt="Logo HAIA" />
		</section>
	)
}

