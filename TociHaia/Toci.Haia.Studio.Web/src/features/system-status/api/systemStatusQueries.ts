import { useQueries, type UseQueryResult } from '@tanstack/react-query'
import { getLiveHealth, getReadyHealth, getSystemInfo } from './systemStatusApi'
import type { HealthStatusResponse, SystemInfoResponse } from '../contracts/systemStatusContracts'

export function useSystemStatusQueries(): [
	UseQueryResult<SystemInfoResponse, Error>,
	UseQueryResult<HealthStatusResponse, Error>,
	UseQueryResult<HealthStatusResponse, Error>,
] {
	return useQueries({
		queries: [
			{
				queryKey: ['studio', 'system-info'],
				queryFn: ({ signal }) => getSystemInfo(signal),
			},
			{
				queryKey: ['studio', 'health-live'],
				queryFn: ({ signal }) => getLiveHealth(signal),
			},
			{
				queryKey: ['studio', 'health-ready'],
				queryFn: ({ signal }) => getReadyHealth(signal),
			},
		],
	}) as [
		UseQueryResult<SystemInfoResponse, Error>,
		UseQueryResult<HealthStatusResponse, Error>,
		UseQueryResult<HealthStatusResponse, Error>,
	]
}
