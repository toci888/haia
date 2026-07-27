import { apiClient } from '../../../shared/api/apiClient'
import type { HealthStatusResponse, SystemInfoResponse } from '../contracts/systemStatusContracts'

function parseHealthPayload(data: unknown): HealthStatusResponse {
	if (!data || typeof data !== 'object') {
		return {}
	}

	const candidate = data as Record<string, unknown>
	if (typeof candidate.status === 'string') {
		return { status: candidate.status }
	}

	return {}
}

export async function getSystemInfo(signal?: AbortSignal): Promise<SystemInfoResponse> {
	const response = await apiClient.get<SystemInfoResponse>('/api/v1/system/info', { signal })
	return response.data
}

export async function getLiveHealth(signal?: AbortSignal): Promise<HealthStatusResponse> {
	const response = await apiClient.get<unknown>('/health/live', { signal })
	return parseHealthPayload(response.data)
}

export async function getReadyHealth(signal?: AbortSignal): Promise<HealthStatusResponse> {
	const response = await apiClient.get<unknown>('/health/ready', { signal })
	return parseHealthPayload(response.data)
}
