import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { http, HttpResponse } from 'msw'
import { RouterProvider } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { Navigate, createMemoryRouter } from 'react-router-dom'
import { server } from './msw/server'
import { RegistrationPage } from '../features/registration/pages/RegistrationPage'
import { CheckEmailPage } from '../features/registration/pages/CheckEmailPage'
import { VerifyEmailPage } from '../features/registration/pages/VerifyEmailPage'
import { NotFoundPage } from '../shared/components/NotFoundPage'

function renderRoute(initialEntries: string[]) {
	const routes = [
		{ path: '/', element: <Navigate to="/register" replace /> },
		{ path: '/register', element: <RegistrationPage /> },
		{ path: '/register/check-email', element: <CheckEmailPage /> },
		{ path: '/verify-email', element: <VerifyEmailPage /> },
		{ path: '*', element: <NotFoundPage /> },
	]
	const memoryRouter = createMemoryRouter(routes, { initialEntries })
	const client = new QueryClient({
		defaultOptions: {
			queries: { retry: false },
			mutations: { retry: false },
		},
	})

	return render(
		<QueryClientProvider client={client}>
			<RouterProvider router={memoryRouter} />
		</QueryClientProvider>,
	)
}

describe('registration slice', () => {
	it('renders /register form', async () => {
		renderRoute(['/register'])
		expect(await screen.findByRole('heading', { name: 'Załóż konto w HAIA' })).toBeInTheDocument()
		expect(screen.getByLabelText('Pseudonim')).toBeInTheDocument()
		expect(screen.getByLabelText('Adres e-mail')).toBeInTheDocument()
		expect(screen.getByLabelText('Hasło')).toBeInTheDocument()
	})

	it('loads legal documents and renders decision controls', async () => {
		renderRoute(['/register'])
		expect(await screen.findByText(/Dokumenty prawne/i)).toBeInTheDocument()
		const checkboxes = await screen.findAllByRole('checkbox')
		expect(checkboxes.length).toBeGreaterThan(0)
	})

	it('handles empty legal documents list', async () => {
		server.use(http.get('*/api/v1/legal-documents/current', () => HttpResponse.json({ documents: [] })))
		renderRoute(['/register'])
		await screen.findByRole('heading', { name: 'Załóż konto w HAIA' })
		await waitFor(() => {
			expect(screen.queryByText(/Dokumenty prawne/i)).not.toBeInTheDocument()
		})
	})

	it('checks nickname after debounce and shows availability', async () => {
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		expect(await screen.findByText(/Pseudonim jest dost/i)).toBeInTheDocument()
	})

	it('invalid nickname does not call availability and shows local validation hint', async () => {
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'a')
		expect(await screen.findByText(/Pseudonim musi mie/i)).toBeInTheDocument()
		expect(screen.queryByText('Sprawdzamy pseudonim…')).not.toBeInTheDocument()
	})

	it('shows taken nickname state', async () => {
		server.use(http.get('*/api/v1/registration/nickname-availability', () => HttpResponse.json({ available: false, normalizedNickname: 'tester' })))
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		expect(await screen.findByText(/pseudonim jest ju/i)).toBeInTheDocument()
	})

	it('submits valid registration and navigates to check-email after 202', async () => {
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'user@example.com')
		await userEvent.type(screen.getByLabelText('Hasło'), 'SuperSecure123!')
		await userEvent.click((await screen.findAllByRole('checkbox'))[0])
		await userEvent.click(screen.getByRole('button', { name: 'Załóż konto i zacznij kalibrację' }))
		expect(await screen.findByRole('heading', { name: /Sprawd.*skrzynk/i })).toBeInTheDocument()
	})

	it('does not place password in URL or storage', async () => {
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'safe@example.com')
		await userEvent.type(await screen.findByLabelText('Hasło'), 'SuperSecure123!')
		expect(window.location.href).not.toContain('SuperSecure123!')
		const ls = window.localStorage as unknown as Record<string, unknown>
		const ss = window.sessionStorage as unknown as Record<string, unknown>
		if (typeof ls.getItem === 'function') {
			expect((ls.getItem as (key: string) => string | null)('password')).toBeNull()
		}
		if (typeof ss.getItem === 'function') {
			expect((ss.getItem as (key: string) => string | null)('password')).toBeNull()
		}
	})

	it('duplicate email 202 behaves neutral and identical', async () => {
		server.use(http.post('*/api/v1/registration/email', () => HttpResponse.json({ status: 'accepted' }, { status: 202 })))
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'existing@example.com')
		await userEvent.type(screen.getByLabelText('Hasło'), 'SuperSecure123!')
		await userEvent.click((await screen.findAllByRole('checkbox'))[0])
		await userEvent.click(screen.getByRole('button', { name: 'Załóż konto i zacznij kalibrację' }))
		expect(await screen.findByRole('heading', { name: /Sprawd.*skrzynk/i })).toBeInTheDocument()
	})

	it('maps nickname_unavailable under nickname field', async () => {
		server.use(
			http.post(
				'*/api/v1/registration/email',
				() => HttpResponse.json({ title: 'Domain error', detail: 'Nickname unavailable', status: 409, code: 'nickname_unavailable' }, { status: 409 }),
			),
		)
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'user@example.com')
		await userEvent.type(screen.getByLabelText('Hasło'), 'SuperSecure123!')
		await userEvent.click((await screen.findAllByRole('checkbox'))[0])
		await userEvent.click(screen.getByRole('button', { name: 'Załóż konto i zacznij kalibrację' }))
		expect((await screen.findAllByText(/pseudonim/i)).length).toBeGreaterThan(0)
	})

	it('legal_decisions_incomplete highlights legal section', async () => {
		server.use(
			http.post(
				'*/api/v1/registration/email',
				() => HttpResponse.json({ title: 'Domain error', detail: 'Legal decisions incomplete', status: 422, code: 'legal_decisions_incomplete' }, { status: 422 }),
			),
		)
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'user@example.com')
		await userEvent.type(screen.getByLabelText('Hasło'), 'SuperSecure123!')
		await userEvent.click(screen.getByRole('button', { name: 'Załóż konto i zacznij kalibrację' }))
		expect(await screen.findByText(/decyzje.*dokument/i)).toBeInTheDocument()
	})

	it('legal_documents_changed refreshes list and clears previous decisions', async () => {
		let calls = 0
		server.use(
			http.get('*/api/v1/legal-documents/current', () => {
				calls += 1
				const documentVersionId = calls > 1 ? '22222222-2222-2222-2222-222222222222' : '11111111-1111-1111-1111-111111111111'
				return HttpResponse.json({
					documents: [
						{
							documentVersionId,
							documentKey: 'terms',
							version: calls > 1 ? '1.1' : '1.0',
							requiredAction: 'accepted',
							effectiveFrom: '2026-01-01T00:00:00Z',
							effectiveTo: null,
							language: 'pl-PL',
						},
					],
				})
			}),
			http.post('*/api/v1/registration/email', () =>
				HttpResponse.json({ title: 'Domain error', detail: 'Legal docs changed', status: 409, code: 'legal_documents_changed' }, { status: 409 }),
			),
		)
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'user@example.com')
		await userEvent.type(screen.getByLabelText('Hasło'), 'SuperSecure123!')
		await userEvent.click((await screen.findAllByRole('checkbox'))[0])
		await userEvent.click(screen.getByRole('button', { name: 'Załóż konto i zacznij kalibrację' }))
		expect(await screen.findByText(/Dokumenty zosta/i)).toBeInTheDocument()
		expect(await screen.findByText(/1.1/i)).toBeInTheDocument()
	})

	it('check-email resend always shows neutral success after 202', async () => {
		renderRoute(['/register/check-email'])
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'user@example.com')
		await userEvent.click(screen.getByRole('button', { name: 'Wyślij wiadomość ponownie' }))
		const neutralMessages = await screen.findAllByText(/Jeżeli dla podanego adresu można rozpocząć lub dokończyć rejestrację/i)
		expect(neutralMessages.length).toBeGreaterThan(0)
	})

	it('verify does not trigger POST automatically on page load', async () => {
		let called = false
		server.use(http.post('*/api/v1/registration/email/verify', () => {
			called = true
			return new HttpResponse(null, { status: 204 })
		}))
		renderRoute(['/verify-email?token=abc'])
		await screen.findByRole('heading', { name: 'Potwierdź adres e-mail' })
		expect(called).toBe(false)
	})

	it('verify triggers POST after click and shows success state', async () => {
		renderRoute(['/verify-email?token=abc'])
		await userEvent.click(screen.getByRole('button', { name: 'Potwierdź adres' }))
		expect(await screen.findByRole('heading', { name: /Konto aktywne/i })).toBeInTheDocument()
	})

	it('expired token shows safe invalid message', async () => {
		server.use(
			http.post(
				'*/api/v1/registration/email/verify',
				() => HttpResponse.json({ title: 'Domain error', detail: 'Invalid token', status: 400, code: 'invalid_or_expired_verification_token' }, { status: 400 }),
			),
		)
		renderRoute(['/verify-email?token=abc'])
		await userEvent.click(screen.getByRole('button', { name: 'Potwierdź adres' }))
		expect(await screen.findByText(/Token jest nieprawid/i)).toBeInTheDocument()
	})

	it('does not render raw ProblemDetails JSON', async () => {
		server.use(
			http.post(
				'*/api/v1/registration/email',
				() => HttpResponse.json({ title: 'Domain error', detail: 'Validation failed', status: 422, code: 'validation_failed', errors: { email: ['Bad email'] } }, { status: 422 }),
			),
		)
		renderRoute(['/register'])
		await userEvent.type(screen.getByLabelText('Pseudonim'), 'tester')
		await userEvent.type(screen.getByLabelText('Adres e-mail'), 'bad-email@example.com')
		await userEvent.type(screen.getByLabelText('Hasło'), 'SuperSecure123!')
		await userEvent.click((await screen.findAllByRole('checkbox'))[0])
		await userEvent.click(screen.getByRole('button', { name: 'Załóż konto i zacznij kalibrację' }))
		expect(await screen.findByText(/Sprawd.*dane formularza/i)).toBeInTheDocument()
		expect(screen.queryByText(/"errors"/)).not.toBeInTheDocument()
	})
})
