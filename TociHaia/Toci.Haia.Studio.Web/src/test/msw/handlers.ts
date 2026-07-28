import { http, HttpResponse } from 'msw'

const defaultSystemInfo = {
	serviceName: 'HAIA Studio API',
	apiVersion: '1.0.0.0',
	status: 'ok',
}

export const handlers = [
	http.get('/api/v1/studio/auth/status', () =>
		HttpResponse.json(
			{ title: 'Unauthorized', detail: 'Authentication required.', status: 401, code: 'unauthorized' },
			{ status: 401 },
		),
	),
	http.get('/api/v1/studio/auth/csrf', () => HttpResponse.json({ requestToken: 'csrf-token-test' }, { status: 200 })),
	http.post('/api/v1/studio/auth/login', async ({ request }) => {
		const csrfToken = request.headers.get('X-CSRF-TOKEN')
		if (!csrfToken) {
			return HttpResponse.json(
				{ title: 'CSRF validation failed', detail: 'Request validation failed.', status: 400, code: 'csrf_validation_failed' },
				{ status: 400 },
			)
		}

		const payload = await request.json() as { email?: string; password?: string }
		if (payload.email === 'admin@haia.local' && payload.password === 'Password123!') {
			return new HttpResponse(null, { status: 204 })
		}

		return HttpResponse.json(
			{ title: 'Invalid credentials', detail: 'Nie udało się zalogować.', status: 401, code: 'invalid_credentials' },
			{ status: 401 },
		)
	}),
	http.post('/api/v1/studio/auth/logout', () => new HttpResponse(null, { status: 204 })),
	http.get('/api/v1/system/info', () => HttpResponse.json(defaultSystemInfo, { status: 200 })),
	http.get('/health/live', () => HttpResponse.json({ status: 'live' }, { status: 200 })),
	http.get('/health/ready', () => HttpResponse.json({ status: 'ready' }, { status: 200 })),
]
