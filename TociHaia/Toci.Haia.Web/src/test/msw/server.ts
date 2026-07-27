import { setupServer } from 'msw/node'
import { http, HttpResponse } from 'msw'

export const server = setupServer(
	http.get('*/api/v1/legal-documents/current', () =>
		HttpResponse.json({
			documents: [
				{
					documentVersionId: '11111111-1111-1111-1111-111111111111',
					documentKey: 'terms',
					version: '1.0',
					requiredAction: 'accepted',
					effectiveFrom: '2026-01-01T00:00:00Z',
					effectiveTo: null,
					language: 'pl-PL',
				},
			],
		}),
	),
	http.get('*/api/v1/registration/nickname-availability', () =>
		HttpResponse.json({
			available: true,
			normalizedNickname: 'tester',
		}),
	),
	http.post('*/api/v1/registration/email', () => HttpResponse.json({ status: 'accepted' }, { status: 202 })),
	http.post('*/api/v1/registration/email/resend', () => HttpResponse.json({ status: 'accepted' }, { status: 202 })),
	http.post('*/api/v1/registration/email/verify', () => new HttpResponse(null, { status: 204 })),
)
