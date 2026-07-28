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

function mockAuthorizedSession(): void {
	server.use(
		http.get('/api/v1/studio/auth/status', () =>
			HttpResponse.json(
				{
					accountId: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
					user: 'Studio Admin',
					email: 'admin@haia.local',
					roles: ['StudioAdmin'],
					scopes: ['studio.api'],
					status: 'authorized',
				},
				{ status: 200 },
			),
		),
	)
}

function mockLoginFlow(): void {
	let isAuthenticated = false

	server.use(
		http.get('/api/v1/studio/auth/status', () => {
			if (!isAuthenticated) {
				return HttpResponse.json(
					{ title: 'Unauthorized', detail: 'Authentication required.', status: 401, code: 'unauthorized' },
					{ status: 401 },
				)
			}

			return HttpResponse.json(
				{
					accountId: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
					user: 'Studio Admin',
					email: 'admin@haia.local',
					roles: ['StudioAdmin'],
					scopes: ['studio.api'],
					status: 'authorized',
				},
				{ status: 200 },
			)
		}),
		http.get('/api/v1/studio/auth/csrf', () => HttpResponse.json({ requestToken: 'csrf-token-test' }, { status: 200 })),
		http.post('/api/v1/studio/auth/login', async ({ request }) => {
			const payload = await request.json() as { email?: string; password?: string }
			if (payload.email === 'admin@haia.local' && payload.password === 'Password123!') {
				isAuthenticated = true
				return new HttpResponse(null, { status: 204 })
			}

			return HttpResponse.json(
				{ title: 'Invalid credentials', detail: 'Nie udało się zalogować.', status: 401, code: 'invalid_credentials' },
				{ status: 401 },
			)
		}),
		http.post('/api/v1/studio/auth/logout', () => {
			isAuthenticated = false
			return new HttpResponse(null, { status: 204 })
		}),
	)
}

describe('Studio system status flow', () => {
	it('renders /system shell and heading', async () => {
		mockAuthorizedSession()
		renderAt('/system')
		expect(await screen.findByRole('heading', { name: 'Status systemu HAIA Studio' })).toBeInTheDocument()
		expect(screen.getByText('HAIA Studio')).toBeInTheDocument()
	})

	it('loads and displays system info data', async () => {
		mockAuthorizedSession()
		renderAt('/system')
		expect(await screen.findByText(/HAIA Studio API/i)).toBeInTheDocument()
		expect(await screen.findByText(/API version:/i)).toBeInTheDocument()
		expect(await screen.findByText(/Status:/i)).toBeInTheDocument()
	})

	it('shows liveness and readiness as available', async () => {
		mockAuthorizedSession()
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

		mockAuthorizedSession()
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

		mockAuthorizedSession()
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

		mockAuthorizedSession()
		renderAt('/system')
		expect(await screen.findByText(/Nie udało się odczytać statusu\. Spróbuj ponownie\./i)).toBeInTheDocument()
		expect(screen.queryByText(/AxiosError/i)).not.toBeInTheDocument()
		expect(screen.queryByText(/stack/i)).not.toBeInTheDocument()
	})

	it('does not render login form while authenticated', async () => {
		mockAuthorizedSession()
		renderAt('/system')
		await screen.findByRole('heading', { name: 'Status systemu HAIA Studio' })
		expect(screen.queryByLabelText(/email/i)).not.toBeInTheDocument()
		expect(screen.queryByLabelText(/hasło/i)).not.toBeInTheDocument()
		expect(screen.queryByText(/token/i)).not.toBeInTheDocument()
	})

	it('redirects to /login when status endpoint returns 401', async () => {
		renderAt('/system')
		expect(await screen.findByRole('heading', { name: /Logowanie do HAIA Studio/i })).toBeInTheDocument()
	})

	it('logs in and redirects to protected /system', async () => {
		mockLoginFlow()
		renderAt('/login')

		await userEvent.type(await screen.findByLabelText(/email/i), 'admin@haia.local')
		await userEvent.type(screen.getByLabelText(/hasło/i), 'Password123!')
		await userEvent.click(screen.getByRole('button', { name: /zaloguj/i }))

		expect(await screen.findByRole('heading', { name: 'Status systemu HAIA Studio' })).toBeInTheDocument()
		expect(await screen.findByText(/Zalogowano: admin@haia.local/i)).toBeInTheDocument()
	})

	it('does not store JWT/token in browser storage after login', async () => {
		mockLoginFlow()

		renderAt('/login')
		await userEvent.type(await screen.findByLabelText(/email/i), 'admin@haia.local')
		await userEvent.type(screen.getByLabelText(/hasło/i), 'Password123!')
		await userEvent.click(screen.getByRole('button', { name: /zaloguj/i }))

		expect(await screen.findByRole('heading', { name: 'Status systemu HAIA Studio' })).toBeInTheDocument()

		const localKeys = Object.keys((globalThis.localStorage ?? {}) as Record<string, unknown>)
		const sessionKeys = Object.keys((globalThis.sessionStorage ?? {}) as Record<string, unknown>)
		const allKeys = [...localKeys, ...sessionKeys].map((key) => key.toLowerCase())

		expect(allKeys.some((key) => key.includes('token') || key.includes('jwt'))).toBe(false)
	})

	it('shows generic invalid-credentials message on failed login', async () => {
		mockLoginFlow()
		renderAt('/login')

		await userEvent.type(await screen.findByLabelText(/email/i), 'admin@haia.local')
		await userEvent.type(screen.getByLabelText(/hasło/i), 'WrongPassword!')
		await userEvent.click(screen.getByRole('button', { name: /zaloguj/i }))

		expect(await screen.findByRole('alert')).toHaveTextContent(/Nie udało się zalogować\./i)
	})

	it('logout button returns to login page', async () => {
		mockLoginFlow()
		renderAt('/system')

		await userEvent.type(await screen.findByLabelText(/email/i), 'admin@haia.local')
		await userEvent.type(screen.getByLabelText(/hasło/i), 'Password123!')
		await userEvent.click(screen.getByRole('button', { name: /zaloguj/i }))
		await screen.findByText(/Zalogowano: admin@haia.local/i)
		await userEvent.click(screen.getByRole('button', { name: /wyloguj/i }))

		expect(await screen.findByRole('heading', { name: /Logowanie do HAIA Studio/i })).toBeInTheDocument()
	})

	it('unknown route shows fallback link to /system', async () => {
		mockAuthorizedSession()
		renderAt('/not-existing')
		expect(await screen.findByRole('heading', { name: /Nie znaleziono strony/i })).toBeInTheDocument()
		expect(screen.getByRole('link', { name: /Przejdź do statusu systemu/i })).toHaveAttribute('href', '/system')
	})
})
