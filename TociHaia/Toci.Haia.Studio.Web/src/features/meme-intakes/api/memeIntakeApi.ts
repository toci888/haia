import { apiClient } from '../../../shared/api/apiClient'
import type {
	CreateMemeIntakeRequest,
	CreateMemeIntakeResponse,
	EvaluateMemeIntakeResponse,
	FinalizeMemeIntakeResponse,
	GetMemeIntakeResponse,
	MemeEditorialDecisionRequest,
	MemeEditorialDecisionResponse,
	StartEditorialReviewResponse,
} from '../contracts/memeIntakeContracts'

async function getCsrfToken(signal?: AbortSignal): Promise<string> {
	const response = await apiClient.get<{ requestToken: string }>('/api/v1/studio/auth/csrf', { signal })
	return response.data.requestToken
}

export async function createMemeIntake(payload: CreateMemeIntakeRequest, signal?: AbortSignal): Promise<CreateMemeIntakeResponse> {
	const csrf = await getCsrfToken(signal)
	const response = await apiClient.post<CreateMemeIntakeResponse>('/api/v1/studio/meme-intakes', payload, {
		headers: { 'X-CSRF-TOKEN': csrf },
		signal,
	})
	return response.data
}

export async function uploadToPrivateStorage(uploadUrl: string, headers: Record<string, string>, file: File): Promise<void> {
	const response = await fetch(uploadUrl, {
		method: 'PUT',
		headers,
		body: file,
		credentials: 'omit',
	})

	if (!response.ok) {
		throw new Error('upload_failed')
	}
}

export async function finalizeMemeIntake(intakeId: string, signal?: AbortSignal): Promise<FinalizeMemeIntakeResponse> {
	const csrf = await getCsrfToken(signal)
	const response = await apiClient.post<FinalizeMemeIntakeResponse>(`/api/v1/studio/meme-intakes/${intakeId}/finalize-upload`, undefined, {
		headers: { 'X-CSRF-TOKEN': csrf },
		signal,
	})
	return response.data
}

export async function evaluateMemeIntake(intakeId: string, signal?: AbortSignal): Promise<EvaluateMemeIntakeResponse> {
	const csrf = await getCsrfToken(signal)
	const response = await apiClient.post<EvaluateMemeIntakeResponse>(`/api/v1/studio/meme-intakes/${intakeId}/evaluate`, undefined, {
		headers: { 'X-CSRF-TOKEN': csrf },
		signal,
	})
	return response.data
}

export async function getMemeIntake(intakeId: string, signal?: AbortSignal): Promise<GetMemeIntakeResponse> {
	const response = await apiClient.get<GetMemeIntakeResponse>(`/api/v1/studio/meme-intakes/${intakeId}`, { signal })
	return response.data
}

export async function startEditorialReview(intakeId: string, signal?: AbortSignal): Promise<StartEditorialReviewResponse> {
	const csrf = await getCsrfToken(signal)
	const response = await apiClient.post<StartEditorialReviewResponse>(`/api/v1/studio/meme-intakes/${intakeId}/editorial-review/start`, undefined, {
		headers: { 'X-CSRF-TOKEN': csrf },
		signal,
	})
	return response.data
}

export async function approveEditorialReview(
	intakeId: string,
	payload: MemeEditorialDecisionRequest,
	signal?: AbortSignal,
): Promise<MemeEditorialDecisionResponse> {
	const csrf = await getCsrfToken(signal)
	const response = await apiClient.post<MemeEditorialDecisionResponse>(`/api/v1/studio/meme-intakes/${intakeId}/editorial-review/approve`, payload, {
		headers: { 'X-CSRF-TOKEN': csrf },
		signal,
	})
	return response.data
}

export async function rejectEditorialReview(
	intakeId: string,
	payload: MemeEditorialDecisionRequest,
	signal?: AbortSignal,
): Promise<MemeEditorialDecisionResponse> {
	const csrf = await getCsrfToken(signal)
	const response = await apiClient.post<MemeEditorialDecisionResponse>(`/api/v1/studio/meme-intakes/${intakeId}/editorial-review/reject`, payload, {
		headers: { 'X-CSRF-TOKEN': csrf },
		signal,
	})
	return response.data
}
