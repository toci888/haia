import styles from './studio-layout.module.css'

const logoHref = new URL('../../assets/logo.png', import.meta.url).href

export function StudioHeader(): React.ReactElement {
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
				<p className={styles.headerNotice}>Kolejne moduły pojawią się po integracji logowania Studio.</p>
			</div>
		</header>
	)
}
