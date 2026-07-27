import styles from '../registration.module.css'

export function RegistrationBrandPanel(): React.ReactElement {
	return (
		<section className={styles.branding} aria-label="Branding HAIA">
			<h2 className={styles.brandWordmark}>HAIA</h2>
			<p>Ĺšwiat humoru dopasowany do Ciebie. Zaczynamy od bezpiecznego startu konta.</p>
			<div className={styles.emojiCloud}>
				<span aria-hidden="true">đź‚</span>
				<span aria-hidden="true">đź†</span>
				<span aria-hidden="true">đźś</span>
				<span aria-hidden="true">đźŠ</span>
			</div>
		</section>
	)
}

