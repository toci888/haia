import {
	keepPreviousData,
	queryOptions,
	useMutation,
	useQuery,
	type UseQueryResult,
} from '@tanstack/react-query'
import {
	checkNicknameAvailability,
	getCurrentLegalDocuments,
	registerByEmail,
	resendEmailVerification,
	verifyEmail,
} from './registrationApi'
import type {
	CurrentLegalDocumentsResponseDto,
	NicknameAvailabilityResponseDto,
	RegisterEmailRequestDto,
	ResendEmailRequestDto,
	VerifyEmailRequestDto,
} from '../contracts/registrationContracts'

export function currentLegalDocumentsQueryOptions(language: string) {
	return queryOptions({
		queryKey: ['registration', 'legal-documents', language],
		queryFn: ({ signal }) => getCurrentLegalDocuments(language, signal),
		staleTime: 120_000,
		retry: 1,
	})
}

export function nicknameAvailabilityQueryOptions(nickname: string, enabled: boolean) {
	return queryOptions({
		queryKey: ['registration', 'nickname', nickname],
		queryFn: ({ signal }) => checkNicknameAvailability(nickname, signal),
		enabled,
		placeholderData: keepPreviousData,
		retry: 1,
	})
}

export function useCurrentLegalDocuments(language: string): UseQueryResult<CurrentLegalDocumentsResponseDto> {
	return useQuery(currentLegalDocumentsQueryOptions(language))
}

export function useNicknameAvailability(nickname: string, enabled: boolean): UseQueryResult<NicknameAvailabilityResponseDto> {
	return useQuery(nicknameAvailabilityQueryOptions(nickname, enabled))
}

export function useRegisterMutation() {
	return useMutation({
		mutationFn: (payload: RegisterEmailRequestDto) => registerByEmail(payload),
		retry: 0,
	})
}

export function useResendMutation() {
	return useMutation({
		mutationFn: (payload: ResendEmailRequestDto) => resendEmailVerification(payload),
		retry: 0,
	})
}

export function useVerifyMutation() {
	return useMutation({
		mutationFn: (payload: VerifyEmailRequestDto) => verifyEmail(payload),
		retry: 0,
	})
}
