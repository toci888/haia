import { useMemo, useState } from 'react'
import { StudioLayout } from '../../../layout/StudioLayout'
import { ApiError } from '../../../shared/api/apiError'
import { useSystemStatusQueries } from '../api/systemStatusQueries'
import { SystemStatusCard } from '../components/SystemStatusCard'
import type { HealthIndicator } from '../contracts/systemStatusContracts'
import styles from './system-status-page.module.css'

function toHealthIndicator(isLoading: boolean, hasError: boolean, status?: string): HealthIndicator {
	if (isLoading) {
		return 'checking'
	}

	if (hasError) {
		return 'unavailable'
	}

	return status?.toLowerCase() === 'ready' || status?.toLowerCase() === 'live' ? 'available' : 'unavailable'
}

function isApiError(value: unknown): value is ApiError {
	return value instanceof ApiError
}

export function SystemStatusPage(): React.ReactElement {
	const [lastSuccessfulRefreshAt, setLastSuccessfulRefreshAt] = useState<Date | null>(null)
	const [systemInfoQuery, liveHealthQuery, readyHealthQuery] = useSystemStatusQueries()

	const isRefreshing = systemInfoQuery.isFetching || liveHealthQuery.isFetching || readyHealthQuery.isFetching
	const hasPartialFailure = [systemInfoQuery, liveHealthQuery, readyHealthQuery].some((q) => q.isError) &&
		[systemInfoQuery, liveHealthQuery, readyHealthQuery].some((q) => q.isSuccess)

	const systemError = isApiError(systemInfoQuery.error) ? systemInfoQuery.error : null
	const liveError = isApiError(liveHealthQuery.error) ? liveHealthQuery.error : null
	const readyError = isApiError(readyHealthQuery.error) ? readyHealthQuery.error : null

	const systemIndicator = useMemo(
		() => toHealthIndicator(systemInfoQuery.isLoading, systemInfoQuery.isError, systemInfoQuery.data?.status),
		[systemInfoQuery.isLoading, systemInfoQuery.isError, systemInfoQuery.data?.status],
	)
	const liveIndicator = useMemo(
		() => toHealthIndicator(liveHealthQuery.isLoading, liveHealthQuery.isError, liveHealthQuery.data?.status),
		[liveHealthQuery.isLoading, liveHealthQuery.isError, liveHealthQuery.data?.status],
	)
	const readyIndicator = useMemo(
		() => toHealthIndicator(readyHealthQuery.isLoading, readyHealthQuery.isError, readyHealthQuery.data?.status),
		[readyHealthQuery.isLoading, readyHealthQuery.isError, readyHealthQuery.data?.status],
	)

	const firstSuccessfulRefreshAt = useMemo(() => {
		if (!systemInfoQuery.isSuccess || !liveHealthQuery.isSuccess || !readyHealthQuery.isSuccess) {
			return null
		}

		const latestQueryUpdate = Math.max(
			systemInfoQuery.dataUpdatedAt,
			liveHealthQuery.dataUpdatedAt,
			readyHealthQuery.dataUpdatedAt,
		)

		return latestQueryUpdate > 0 ? new Date(latestQueryUpdate) : null
	}, [
		systemInfoQuery.isSuccess,
		liveHealthQuery.isSuccess,
		readyHealthQuery.isSuccess,
		systemInfoQuery.dataUpdatedAt,
		liveHealthQuery.dataUpdatedAt,
		readyHealthQuery.dataUpdatedAt,
	])

	const effectiveLastSuccessfulRefreshAt = lastSuccessfulRefreshAt ?? firstSuccessfulRefreshAt

	const onRefreshClick = async (): Promise<void> => {
		const results = await Promise.allSettled([
			systemInfoQuery.refetch(),
			liveHealthQuery.refetch(),
			readyHealthQuery.refetch(),
		])

		const allSuccessful = results.every(
			(r) => r.status === 'fulfilled' && !r.value.isError,
		)

		if (allSuccessful) {
			setLastSuccessfulRefreshAt(new Date())
		}
	}

	return (
		<StudioLayout>
			<section className={styles.page} aria-labelledby="system-status-title">
				<div className={styles.headerRow}>
					<div>
						<h1 className={styles.heading} id="system-status-title">Status systemu HAIA Studio</h1>
						<p className={styles.description}>Widok kontrolny procesu API i gotowości połączenia z bazą danych.</p>
					</div>
					<div className={styles.actions}>
						<button className={styles.refreshButton} type="button" onClick={() => void onRefreshClick()} disabled={isRefreshing}>
							Odśwież status
						</button>
						<span className={styles.timestamp} aria-live="polite">
							{effectiveLastSuccessfulRefreshAt
								? `Ostatnie poprawne odświeżenie: ${effectiveLastSuccessfulRefreshAt.toLocaleTimeString()}`
								: 'Ostatnie poprawne odświeżenie: jeszcze nie wykonano'}
						</span>
					</div>
				</div>

				{hasPartialFailure ? (
					<p className={styles.partialWarning} role="status">
						Częściowa niedostępność: część wskaźników została odczytana, ale co najmniej jeden endpoint zwrócił błąd.
					</p>
				) : null}

				<div className={styles.statusGrid} aria-live="polite">
					<SystemStatusCard
						title="Studio API"
						indicator={systemIndicator}
						description="Kontrakt endpointu /api/v1/system/info"
						error={systemError}
					>
						{systemInfoQuery.data ? (
							<ul className={styles.metaList}>
								<li><span className={styles.metaLabel}>Service:</span> {systemInfoQuery.data.serviceName}</li>
								<li><span className={styles.metaLabel}>API version:</span> {systemInfoQuery.data.apiVersion}</li>
								<li><span className={styles.metaLabel}>Status:</span> {systemInfoQuery.data.status}</li>
							</ul>
						) : null}
					</SystemStatusCard>

					<SystemStatusCard
						title="Proces API"
						indicator={liveIndicator}
						description="Liveness check /health/live"
						error={liveError}
					>
						{liveHealthQuery.data?.status ? <p className={styles.description}>Status endpointu: {liveHealthQuery.data.status}</p> : null}
					</SystemStatusCard>

					<SystemStatusCard
						title="Połączenie z bazą"
						indicator={readyIndicator}
						description="Readiness check /health/ready"
						error={readyError}
					>
						{readyHealthQuery.data?.status ? <p className={styles.description}>Status endpointu: {readyHealthQuery.data.status}</p> : null}
					</SystemStatusCard>
				</div>
			</section>
		</StudioLayout>
	)
}
