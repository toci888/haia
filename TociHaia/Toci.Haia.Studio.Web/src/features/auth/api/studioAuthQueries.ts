import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { getStudioAuthStatus, loginStudio, logoutStudio } from './studioAuthApi'
import type { StudioLoginRequest } from '../contracts/studioAuthContracts'

const authStatusKey = ['studio', 'auth-status'] as const

export function useStudioAuthStatusQuery() {
	return useQuery({
		queryKey: authStatusKey,
		queryFn: ({ signal }) => getStudioAuthStatus(signal),
		retry: false,
		staleTime: 0,
		refetchOnMount: 'always',
	})
}

export function useStudioLoginMutation() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: (payload: StudioLoginRequest) => loginStudio(payload),
		onSuccess: async () => {
			await queryClient.invalidateQueries({ queryKey: authStatusKey })
		},
	})
}

export function useStudioLogoutMutation() {
	const queryClient = useQueryClient()

	return useMutation({
		mutationFn: () => logoutStudio(),
		onSuccess: () => {
			queryClient.removeQueries({ queryKey: authStatusKey })
		},
	})
}
