import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { http, HttpResponse } from 'msw'
import { RouterProvider, createMemoryRouter } from 'react-router-dom'
import { describe, expect, it } from 'vitest'
import { routes } from '../app/router'
import { server } from './msw/server'

function renderAt(initialEntry: string) {
	const queryClient = new QueryClient({
		defaultOptions: {
			queries: {
				retry: 0,
				staleTime: 0,
				refetchOnWindowFocus: false,
			},
		},
	})

	const router = createMemoryRouter(routes, {
		initialEntries: [initialEntry],
	})

	return render(
		<QueryClientProvider client={queryClient}>
			<RouterProvider router={router} />
		</QueryClientProvider>,
	)
}

function mockReviewFlow(calls: string[]): void {
	server.use(
		http.get('/api/v1/studio/auth/csrf', () => {
			calls.push('csrf')
			return HttpResponse.json({ requestToken: 'csrf-token-test' }, { status: 200 })
		}),
		http.post('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/editorial-review/start', () => {
			calls.push('review_start')
			return HttpResponse.json({
				intakeId: '11111111-1111-1111-1111-111111111111',
				status: 'ready_for_review',
				editorialStatus: 'editorial_review',
				classificationStatus: 'awaiting_review',
				candidateClassificationId: '55555555-5555-5555-5555-555555555555',
			})
		}),
		http.get('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111', () => {
			calls.push('review_get')
			return HttpResponse.json({
				intakeId: '11111111-1111-1111-1111-111111111111',
				status: 'editorial_review',
				workingTitle: 'Draft',
				asset: {
					fileName: 'meme.png',
					contentType: 'image/png',
					sizeBytes: 1024,
					objectKey: 'studio/onboarding-candidates/222/original',
				},
				evaluation: buildEvaluationResponse().evaluation,
			})
		}),
		http.post('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/editorial-review/approve', async () => {
			calls.push('review_approve')
			return HttpResponse.json({
				intakeId: '11111111-1111-1111-1111-111111111111',
				status: 'approved',
				editorialStatus: 'approved',
				classificationStatus: 'approved',
				reviewStatus: 'approved',
				candidateClassificationId: '55555555-5555-5555-5555-555555555555',
				reviewedAtUtc: new Date().toISOString(),
				reviewedByAccountId: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
			})
		}),
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

function buildEvaluationResponse() {
	return {
		intakeId: '11111111-1111-1111-1111-111111111111',
		status: 'ready_for_review',
		evaluation: {
			suggestedTitle: 'Tytuł testowy',
			visualDescription: 'Opis testowy',
			detectedText: 'MEME TEXT',
			languageCode: 'pl-PL',
			contentFormat: 'image',
			suggestedRoleInFlow: 'exploration',
			suggestedRoleConfidence: 0.8,
			editorialSummary: 'Podsumowanie',
			overallConfidence: 0.9,
			classifications: [
				{
					axisKey: 'mechanism',
					axisDisplayName: 'Mechanizm',
					valueKey: 'incongruity',
					valueDisplayName: 'Niespodzianka',
					relevanceScore: 0.9,
					confidence: 0.8,
					isPrimary: true,
					rankNo: 1,
				},
			],
			measures: [
				{
					axisKey: 'absurdity',
					axisDisplayName: 'Absurdalność',
					normalizedValue: 0.4,
					confidence: 0.6,
				},
			],
			safety: [
				{
					categoryKey: 'general',
					categoryDisplayName: 'Ogólna',
					severityLevel: 1,
					confidence: 0.7,
					moderationRelevance: true,
				},
			],
			moderationRecommendation: 'review',
			moderationSummary: 'Bezpieczeństwo',
			predictedDrynessLevel: 3,
			predictedDrynessLabel: 'Średni',
			predictedDrynessConfidence: 0.75,
			reactions: Array.from({ length: 12 }, (_, index) => ({
				text: `Reakcja ${index + 1}`,
				displayOrder: index + 1,
				initiallyVisible: index < 6,
				relevanceScore: 0.8,
				confidence: 0.8,
				secondPunchline: `Puenta ${index + 1}`,
				mechanismKeys: ['incongruity'],
			})),
		},
	}
}

function mockHappyFlow(calls: string[]): void {
	server.use(
		http.get('/api/v1/studio/auth/csrf', () => {
			calls.push('csrf')
			return HttpResponse.json({ requestToken: 'csrf-token-test' }, { status: 200 })
		}),
		http.post('/api/v1/studio/meme-intakes', () => {
			calls.push('create')
			return HttpResponse.json(
				{
					intakeId: '11111111-1111-1111-1111-111111111111',
					candidateId: '22222222-2222-2222-2222-222222222222',
					candidateVersionId: '33333333-3333-3333-3333-333333333333',
					assetId: '44444444-4444-4444-4444-444444444444',
					objectKey: 'studio/onboarding-candidates/222/original',
					uploadUrl: 'https://private-r2.local/upload',
					uploadHeaders: { 'Content-Type': 'image/png' },
					uploadUrlExpiresAtUtc: new Date(Date.now() + 60_000).toISOString(),
					status: 'creating_draft',
				},
				{ status: 200 },
			)
		}),
		http.put('https://private-r2.local/upload', () => {
			calls.push('upload')
			return new HttpResponse(null, { status: 200 })
		}),
		http.post('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/finalize-upload', () => {
			calls.push('finalize')
			return HttpResponse.json({
				intakeId: '11111111-1111-1111-1111-111111111111',
				status: 'uploaded',
				contentType: 'image/png',
				sizeBytes: 1024,
				sha256Hash: 'abc',
			})
		}),
		http.post('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/evaluate', () => {
			calls.push('evaluate')
			return HttpResponse.json(buildEvaluationResponse(), { status: 200 })
		}),
	)
}

describe('Meme intake flow', () => {
	it('handles image paste from clipboard and ignores plain text', async () => {
		mockAuthorizedSession()
		renderAt('/meme-intakes/new')

		expect(await screen.findByRole('heading', { name: 'Dodaj mem do onboardingu' })).toBeInTheDocument()
		expect(screen.getByText('Nie wybrano pliku.')).toBeInTheDocument()

		const plainTextItem = {
			kind: 'string',
			type: 'text/plain',
			getAsFile: () => null,
		}

		fireEvent.paste(window, {
			clipboardData: {
				items: [plainTextItem],
			},
		})

		expect(screen.getByText('Nie wybrano pliku.')).toBeInTheDocument()

		const file = new File([new Uint8Array([0x89, 0x50, 0x4E, 0x47])], 'clipboard.png', { type: 'image/png' })
		const imageItem = {
			kind: 'file',
			type: 'image/png',
			getAsFile: () => file,
		}

		fireEvent.paste(window, {
			clipboardData: {
				items: [imageItem],
			},
		})

		expect(await screen.findByText(/Nazwa:/i)).toBeInTheDocument()
		expect(screen.getByText(/clipboard\.png/i)).toBeInTheDocument()
	})

	it('supports file picker preview and remove', async () => {
		mockAuthorizedSession()
		const view = renderAt('/meme-intakes/new')
		await screen.findByRole('heading', { name: 'Dodaj mem do onboardingu' })

		const input = view.container.querySelector('input[type="file"]') as HTMLInputElement
		const file = new File([new Uint8Array([0x89, 0x50, 0x4E, 0x47])], 'disk.png', { type: 'image/png' })

		await userEvent.upload(input, file)
		expect(await screen.findByText(/disk\.png/i)).toBeInTheDocument()
		expect(screen.getByRole('button', { name: 'Usuń plik' })).toBeInTheDocument()

		await userEvent.click(screen.getByRole('button', { name: 'Usuń plik' }))
		expect(screen.getByText('Nie wybrano pliku.')).toBeInTheDocument()
	})

	it('runs upload flow in order and displays evaluation with safety, dryness and 12 reactions', async () => {
		mockAuthorizedSession()
		const calls: string[] = []
		mockHappyFlow(calls)

		const view = renderAt('/meme-intakes/new')
		await screen.findByRole('heading', { name: 'Dodaj mem do onboardingu' })

		const input = view.container.querySelector('input[type="file"]') as HTMLInputElement
		const file = new File([new Uint8Array([0x89, 0x50, 0x4E, 0x47])], 'flow.png', { type: 'image/png' })
		await userEvent.upload(input, file)

		await userEvent.click(screen.getByRole('button', { name: 'Wyślij do AI i wykonaj pełną ewaluację' }))

		await screen.findByText(/Status:\s*Wynik gotowy do oceny/i)
		expect(screen.getByText(/Tytuł testowy/i)).toBeInTheDocument()
		expect(screen.getByText(/Ogólna: severity 1/i)).toBeInTheDocument()
		expect(screen.getByText(/Poziom: 3 \(Średni\)/i)).toBeInTheDocument()
		expect(screen.getAllByText(/Druga puenta:/i)).toHaveLength(12)
		expect(screen.queryByRole('button', { name: /publikuj|aktywuj|zatwierdź/i })).not.toBeInTheDocument()

		await waitFor(() => {
			expect(calls).toContain('create')
			expect(calls).toContain('upload')
			expect(calls).toContain('finalize')
			expect(calls).toContain('evaluate')
		})

		const createIndex = calls.indexOf('create')
		const uploadIndex = calls.indexOf('upload')
		const finalizeIndex = calls.indexOf('finalize')
		const evaluateIndex = calls.indexOf('evaluate')
		expect(createIndex).toBeGreaterThan(-1)
		expect(createIndex).toBeLessThan(uploadIndex)
		expect(uploadIndex).toBeLessThan(finalizeIndex)
		expect(finalizeIndex).toBeLessThan(evaluateIndex)
	})

	it('shows upload error state', async () => {
		mockAuthorizedSession()
		server.use(
			http.get('/api/v1/studio/auth/csrf', () => HttpResponse.json({ requestToken: 'csrf-token-test' })),
			http.post('/api/v1/studio/meme-intakes', () =>
				HttpResponse.json(
					{
						intakeId: '11111111-1111-1111-1111-111111111111',
						candidateId: '22222222-2222-2222-2222-222222222222',
						candidateVersionId: '33333333-3333-3333-3333-333333333333',
						assetId: '44444444-4444-4444-4444-444444444444',
						objectKey: 'studio/onboarding-candidates/222/original',
						uploadUrl: 'https://private-r2.local/upload',
						uploadHeaders: { 'Content-Type': 'image/png' },
						uploadUrlExpiresAtUtc: new Date(Date.now() + 60_000).toISOString(),
						status: 'creating_draft',
					},
					{ status: 200 },
				),
			),
			http.put('https://private-r2.local/upload', () => new HttpResponse(null, { status: 500 })),
		)

		const view = renderAt('/meme-intakes/new')
		await screen.findByRole('heading', { name: 'Dodaj mem do onboardingu' })
		const input = view.container.querySelector('input[type="file"]') as HTMLInputElement
		await userEvent.upload(input, new File([new Uint8Array([0x89, 0x50, 0x4E, 0x47])], 'upload-error.png', { type: 'image/png' }))
		await userEvent.click(screen.getByRole('button', { name: 'Wyślij do AI i wykonaj pełną ewaluację' }))

		expect(await screen.findByText(/Nie udało się zakończyć procesu/i)).toBeInTheDocument()
	})

	it('shows evaluate error state with correlation id', async () => {
		mockAuthorizedSession()
		server.use(
			http.get('/api/v1/studio/auth/csrf', () => HttpResponse.json({ requestToken: 'csrf-token-test' })),
			http.post('/api/v1/studio/meme-intakes', () =>
				HttpResponse.json(
					{
						intakeId: '11111111-1111-1111-1111-111111111111',
						candidateId: '22222222-2222-2222-2222-222222222222',
						candidateVersionId: '33333333-3333-3333-3333-333333333333',
						assetId: '44444444-4444-4444-4444-444444444444',
						objectKey: 'studio/onboarding-candidates/222/original',
						uploadUrl: 'https://private-r2.local/upload',
						uploadHeaders: { 'Content-Type': 'image/png' },
						uploadUrlExpiresAtUtc: new Date(Date.now() + 60_000).toISOString(),
						status: 'creating_draft',
					},
					{ status: 200 },
				),
			),
			http.put('https://private-r2.local/upload', () => new HttpResponse(null, { status: 200 })),
			http.post('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/finalize-upload', () =>
				HttpResponse.json({
					intakeId: '11111111-1111-1111-1111-111111111111',
					status: 'uploaded',
					contentType: 'image/png',
					sizeBytes: 1024,
					sha256Hash: 'abc',
				}),
			),
			http.post('/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/evaluate', () =>
				HttpResponse.json(
					{ title: 'AI evaluation failed', detail: 'Provider unavailable.', status: 502, code: 'ai_evaluation_failed' },
					{ status: 502, headers: { 'X-Correlation-ID': 'corr-ai-502' } },
				),
			),
		)

		const view = renderAt('/meme-intakes/new')
		await screen.findByRole('heading', { name: 'Dodaj mem do onboardingu' })
		const input = view.container.querySelector('input[type="file"]') as HTMLInputElement
		await userEvent.upload(input, new File([new Uint8Array([0x89, 0x50, 0x4E, 0x47])], 'evaluate-error.png', { type: 'image/png' }))
		await userEvent.click(screen.getByRole('button', { name: 'Wyślij do AI i wykonaj pełną ewaluację' }))

		expect(await screen.findByText(/Provider unavailable/i)).toBeInTheDocument()
		expect(screen.getByText(/Korelacja: corr-ai-502/i)).toBeInTheDocument()
	})

	it('navigates to editorial review and approves draft without publish action', async () => {
		mockAuthorizedSession()
		const calls: string[] = []
		mockHappyFlow(calls)
		mockReviewFlow(calls)

		const view = renderAt('/meme-intakes/new')
		await screen.findByRole('heading', { name: 'Dodaj mem do onboardingu' })

		const input = view.container.querySelector('input[type="file"]') as HTMLInputElement
		await userEvent.upload(input, new File([new Uint8Array([0x89, 0x50, 0x4E, 0x47])], 'review.png', { type: 'image/png' }))
		await userEvent.click(screen.getByRole('button', { name: 'Wyślij do AI i wykonaj pełną ewaluację' }))

		await screen.findByRole('button', { name: 'Przejdź do review redakcyjnego' })
		await userEvent.click(screen.getByRole('button', { name: 'Przejdź do review redakcyjnego' }))

		expect(await screen.findByRole('heading', { name: 'Review redakcyjny mema' })).toBeInTheDocument()
		await userEvent.type(screen.getByLabelText('Podsumowanie review'), 'ok')
		await userEvent.type(screen.getByLabelText('Notatka redakcyjna'), 'zatwierdzone')
		await userEvent.click(screen.getByRole('button', { name: 'Zatwierdź draft' }))

		expect(await screen.findByText(/approved\s*\/\s*approved\s*\/\s*approved\s*\/\s*approved/i)).toBeInTheDocument()
		expect(screen.queryByRole('button', { name: /publikuj|aktywuj/i })).not.toBeInTheDocument()

		await waitFor(() => {
			expect(calls).toContain('review_start')
			expect(calls).toContain('review_get')
			expect(calls).toContain('review_approve')
		})
	})
})
