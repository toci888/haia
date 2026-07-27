import type { PropsWithChildren } from 'react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { RouterProvider } from 'react-router-dom'
import { router } from '../router'

const queryClient = new QueryClient({
	defaultOptions: {
		queries: {
			retry: 1,
			staleTime: 30_000,
			refetchOnWindowFocus: false,
		},
		mutations: {
			retry: 0,
		},
	},
})

function ProvidersShell({ children }: PropsWithChildren): React.ReactElement {
	return <>{children}</>
}

export function AppProviders(): React.ReactElement {
	return (
		<QueryClientProvider client={queryClient}>
			<ProvidersShell>
				<RouterProvider router={router} />
			</ProvidersShell>
		</QueryClientProvider>
	)
}

