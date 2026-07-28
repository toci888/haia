import { apiClient } from '../../../shared/api/apiClient'
import type { StudioAuthStatusResponse, StudioCsrfTokenResponse, StudioLoginRequest } from '../contracts/studioAuthContracts'

export async function getStudioAuthStatus(signal?: AbortSignal): Promise<StudioAuthStatusResponse> {
	const response = await apiClient.get<StudioAuthStatusResponse>('/api/v1/studio/auth/status', { signal })
	return response.data
}

async function getCsrfToken(signal?: AbortSignal): Promise<string> {
	const response = await apiClient.get<StudioCsrfTokenResponse>('/api/v1/studio/auth/csrf', { signal })
	return response.data.requestToken
}

export async function loginStudio(payload: StudioLoginRequest, signal?: AbortSignal): Promise<void> {
	const csrfToken = await getCsrfToken(signal)
	await apiClient.post('/api/v1/studio/auth/login', payload, {
		headers: {
			'X-CSRF-TOKEN': csrfToken,
		},
		signal,
	})
}

export async function logoutStudio(signal?: AbortSignal): Promise<void> {
	const csrfToken = await getCsrfToken(signal)
	await apiClient.post('/api/v1/studio/auth/logout', undefined, {
		headers: {
			'X-CSRF-TOKEN': csrfToken,
		},
		signal,
	})
}
