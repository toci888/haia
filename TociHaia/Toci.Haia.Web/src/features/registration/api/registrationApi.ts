import { apiClient } from '../../../shared/api/apiClient'
import type {
	AcceptedResponseDto,
	CurrentLegalDocumentsResponseDto,
	NicknameAvailabilityResponseDto,
	RegisterEmailRequestDto,
	ResendEmailRequestDto,
	VerifyEmailRequestDto,
} from '../contracts/registrationContracts'

export async function getCurrentLegalDocuments(language: string, signal?: AbortSignal): Promise<CurrentLegalDocumentsResponseDto> {
	const response = await apiClient.get<CurrentLegalDocumentsResponseDto>('/api/v1/legal-documents/current', {
		params: { language },
		signal,
	})
	return response.data
}

export async function checkNicknameAvailability(nickname: string, signal?: AbortSignal): Promise<NicknameAvailabilityResponseDto> {
	const response = await apiClient.get<NicknameAvailabilityResponseDto>('/api/v1/registration/nickname-availability', {
		params: { nickname },
		signal,
	})
	return response.data
}

export async function registerByEmail(request: RegisterEmailRequestDto, signal?: AbortSignal): Promise<AcceptedResponseDto> {
	const response = await apiClient.post<AcceptedResponseDto>('/api/v1/registration/email', request, { signal })
	return response.data
}

export async function resendEmailVerification(request: ResendEmailRequestDto, signal?: AbortSignal): Promise<AcceptedResponseDto> {
	const response = await apiClient.post<AcceptedResponseDto>('/api/v1/registration/email/resend', request, { signal })
	return response.data
}

export async function verifyEmail(request: VerifyEmailRequestDto, signal?: AbortSignal): Promise<void> {
	await apiClient.post('/api/v1/registration/email/verify', request, { signal })
}
