import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { http, HttpResponse } from 'msw'
import { RouterProvider, createMemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { describe, expect, it } from 'vitest'
import { server } from './msw/server'
import { routes } from '../app/router'

function renderAt(initialEntry: string): void {
	const queryClient = new QueryClient({
		defaultOptions: {
			queries: {
				retry: 0,
				staleTime: 0,
				refetchOnWindowFocus: false,
			},
		},
	})

	const memoryRouter = createMemoryRouter(routes, {
		initialEntries: [initialEntry],
	})

	render(
		<QueryClientProvider client={queryClient}>
			<RouterProvider router={memoryRouter} />
		</QueryClientProvider>,
	)
}

describe('Studio system status flow', () => {
	it('renders /system shell and heading', async () => {
		renderAt('/system')
		expect(await screen.findByRole('heading', { name: 'Status systemu HAIA Studio' })).toBeInTheDocument()
		expect(screen.getByText('HAIA Studio')).toBeInTheDocument()
	})

	it('loads and displays system info data', async () => {
		renderAt('/system')
		expect(await screen.findByText(/HAIA Studio API/i)).toBeInTheDocument()
		expect(await screen.findByText(/API version:/i)).toBeInTheDocument()
		expect(await screen.findByText(/Status:/i)).toBeInTheDocument()
	})

	it('shows liveness and readiness as available', async () => {
		renderAt('/system')
		expect(await screen.findAllByText('Dostępny')).not.toHaveLength(0)
	})

	it('keeps partial data when one endpoint fails', async () => {
		server.use(
			http.get('/health/ready', () =>
				HttpResponse.json(
					{ title: 'Database unavailable', detail: 'Database operation failed.', status: 503, code: 'database_unavailable' },
					{ status: 503, headers: { 'X-Correlation-ID': 'corr-ready-503' } },
				),
			),
		)

		renderAt('/system')
		expect(await screen.findByText(/Częściowa niedostępność/i)).toBeInTheDocument()
		expect(await screen.findByText(/Korelacja: corr-ready-503/i)).toBeInTheDocument()
		expect(await screen.findByText(/HAIA Studio API/i)).toBeInTheDocument()
	})

	it('refresh button performs repeated calls', async () => {
		let systemInfoCalls = 0
		server.use(
			http.get('/api/v1/system/info', () => {
				systemInfoCalls += 1
				return HttpResponse.json({ serviceName: 'HAIA Studio API', apiVersion: '1.0.0.0', status: 'ok' })
			}),
		)

		renderAt('/system')
		await screen.findByText(/HAIA Studio API/i)
		const button = screen.getByRole('button', { name: /Odśwież status/i })
		await userEvent.click(button)

		await waitFor(() => expect(systemInfoCalls).toBeGreaterThanOrEqual(2))
	})

	it('maps problem details to safe message', async () => {
		server.use(
			http.get('/health/live', () =>
				HttpResponse.json(
					{ title: 'Unexpected error', detail: 'Unexpected server error.', status: 500, code: 'unexpected_error' },
					{ status: 500, headers: { 'X-Correlation-ID': 'corr-system-500' } },
				),
			),
		)

		renderAt('/system')
		expect(await screen.findByText(/Nie udało się odczytać statusu\. Spróbuj ponownie\./i)).toBeInTheDocument()
		expect(screen.queryByText(/AxiosError/i)).not.toBeInTheDocument()
		expect(screen.queryByText(/stack/i)).not.toBeInTheDocument()
	})

	it('does not render login form or token controls', async () => {
		renderAt('/system')
		await screen.findByRole('heading', { name: 'Status systemu HAIA Studio' })
		expect(screen.queryByLabelText(/email/i)).not.toBeInTheDocument()
		expect(screen.queryByLabelText(/hasło/i)).not.toBeInTheDocument()
		expect(screen.queryByText(/token/i)).not.toBeInTheDocument()
	})

	it('unknown route shows fallback link to /system', async () => {
		renderAt('/not-existing')
		expect(await screen.findByRole('heading', { name: /Nie znaleziono strony/i })).toBeInTheDocument()
		expect(screen.getByRole('link', { name: /Przejdź do statusu systemu/i })).toHaveAttribute('href', '/system')
	})
})
