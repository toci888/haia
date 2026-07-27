import styles from './system-status-card.module.css'
import type { HealthIndicator } from '../contracts/systemStatusContracts'
import type { ApiError } from '../../../shared/api/apiError'

function statusLabel(indicator: HealthIndicator): string {
	switch (indicator) {
		case 'available':
			return 'Dostępny'
		case 'unavailable':
			return 'Niedostępny'
		default:
			return 'Sprawdzanie'
	}
}

function statusClass(indicator: HealthIndicator): string {
	switch (indicator) {
		case 'available':
			return styles.available
		case 'unavailable':
			return styles.unavailable
		default:
			return styles.checking
	}
}

export interface SystemStatusCardProps {
	title: string
	indicator: HealthIndicator
	description?: string
	error?: ApiError | null
	children?: React.ReactNode
}

export function SystemStatusCard({ title, indicator, description, error, children }: SystemStatusCardProps): React.ReactElement {
	return (
		<article className={styles.card} aria-live="polite">
			<h2>{title}</h2>
			<p className={styles.statusLine}>
				<span aria-hidden="true">●</span>
				{statusLabel(indicator)}
				<span className={`${styles.badge} ${statusClass(indicator)}`}>{statusLabel(indicator)}</span>
			</p>
			{description ? <p className={styles.meta}>{description}</p> : null}
			{children}
			{error ? (
				<div className={styles.error} role="status">
					<p>Nie udało się odczytać statusu. Spróbuj ponownie.</p>
					{error.correlationId ? <p>Korelacja: {error.correlationId}</p> : null}
				</div>
			) : null}
		</article>
	)
}
