import { MemoryRouter } from 'react-router-dom'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { http, HttpResponse } from 'msw'
import { afterEach, describe, expect, it } from 'vitest'
import { MemeIntakePage } from './MemeIntakePage'
import { server } from '../../../test/msw/server'

const intakeId = '11111111-1111-1111-1111-111111111111'
const evaluationPayload = {
	intakeId,
	status: 'ready_for_review',
	evaluation: {
		suggestedTitle: 'Tytuł',
		visualDescription: 'Opis',
		detectedText: null,
		languageCode: 'pl-PL',
		contentFormat: 'image',
		suggestedRoleInFlow: 'exploration',
		suggestedRoleConfidence: 0.8,
		editorialSummary: null,
		overallConfidence: 0.82,
		classifications: [],
		measures: [],
		safety: [],
		moderationRecommendation: 'review',
		moderationSummary: null,
		predictedDrynessLevel: 3,
		predictedDrynessLabel: 'średni',
		predictedDrynessConfidence: 0.71,
		reactions: [],
	},
}

afterEach(() => {
	sessionStorage.clear()
})

function renderPage(): void {
	render(
		<MemoryRouter>
			<MemeIntakePage />
		</MemoryRouter>,
	)
}

function mockInitialFlowHandlers(counters: Record<string, number>): void {
	server.use(
		http.get('/api/v1/studio/auth/csrf', () => {
			counters.csrf += 1
			return HttpResponse.json({ requestToken: 'csrf-token-test' })
		}),
		http.post('/api/v1/studio/meme-intakes', async () => {
			counters.create += 1
			return HttpResponse.json({
				intakeId,
				candidateId: intakeId,
				candidateVersionId: '22222222-2222-2222-2222-222222222222',
				assetId: '33333333-3333-3333-3333-333333333333',
				objectKey: 'studio/key',
				uploadUrl: 'https://private-r2/upload',
				uploadHeaders: { 'Content-Type': 'image/png' },
				uploadUrlExpiresAtUtc: '2026-01-01T00:00:00Z',
				status: 'creating_draft',
			})
		}),
		http.put('https://private-r2/upload', async () => {
			counters.upload += 1
			return new HttpResponse(null, { status: 200 })
		}),
		http.post(`/api/v1/studio/meme-intakes/${intakeId}/finalize-upload`, () => {
			counters.finalize += 1
			return HttpResponse.json({
				intakeId,
				status: 'awaiting_ai_analysis',
				contentType: 'image/png',
				sizeBytes: 123,
				sha256Hash: 'hash',
			})
		}),
	)
}

describe('MemeIntakePage retry flow', () => {
	it('initial flow wykonuje create -> upload -> finalize -> evaluate', async () => {
		const counters = { csrf: 0, create: 0, upload: 0, finalize: 0, evaluate: 0 }
		mockInitialFlowHandlers(counters)
		server.use(
			http.post(`/api/v1/studio/meme-intakes/${intakeId}/evaluate`, () => {
				counters.evaluate += 1
				return HttpResponse.json(evaluationPayload)
			}),
		)

		renderPage()
		const user = userEvent.setup()
		const file = new File([new Uint8Array([1, 2, 3])], 'meme.png', { type: 'image/png' })
		const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement
		await user.upload(fileInput, file)
		await user.click(screen.getByRole('button', { name: 'Uruchom analizę' }))

		await waitFor(() => expect(screen.getByText(/Wynik gotowy do oceny/i)).toBeInTheDocument())
		expect(counters.create).toBe(1)
		expect(counters.upload).toBe(1)
		expect(counters.finalize).toBe(1)
		expect(counters.evaluate).toBe(1)
	})

	it('po błędzie evaluate pokazuje retry i retry wywołuje tylko evaluate', async () => {
		const counters = { csrf: 0, create: 0, upload: 0, finalize: 0, evaluate: 0 }
		mockInitialFlowHandlers(counters)
		let firstEvaluate = true
		server.use(
			http.post(`/api/v1/studio/meme-intakes/${intakeId}/evaluate`, () => {
				counters.evaluate += 1
				if (firstEvaluate) {
					firstEvaluate = false
					return HttpResponse.json(
						{ title: 'AI evaluation failed', detail: 'Provider timeout.', status: 502, code: 'ai_evaluation_failed' },
						{ status: 502, headers: { 'x-correlation-id': 'corr-1' } },
					)
				}

				return HttpResponse.json(evaluationPayload)
			}),
		)

		renderPage()
		const user = userEvent.setup()
		const file = new File([new Uint8Array([1, 2, 3])], 'meme.png', { type: 'image/png' })
		const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement
		await user.upload(fileInput, file)
		await user.click(screen.getByRole('button', { name: 'Uruchom analizę' }))

		await waitFor(() => expect(screen.getByRole('button', { name: 'Ponów analizę AI' })).toBeInTheDocument())
		expect(counters.create).toBe(1)
		expect(counters.upload).toBe(1)
		expect(counters.finalize).toBe(1)
		expect(counters.evaluate).toBe(1)

		await user.click(screen.getByRole('button', { name: 'Ponów analizę AI' }))

		await waitFor(() => expect(screen.getByText(/Wynik gotowy do oceny/i)).toBeInTheDocument())
		expect(counters.create).toBe(1)
		expect(counters.upload).toBe(1)
		expect(counters.finalize).toBe(1)
		expect(counters.evaluate).toBe(2)
	})

	it('retry blokuje podwójne kliknięcie', async () => {
		const counters = { csrf: 0, create: 0, upload: 0, finalize: 0, evaluate: 0 }
		mockInitialFlowHandlers(counters)
		let firstEvaluate = true
		server.use(
			http.post(`/api/v1/studio/meme-intakes/${intakeId}/evaluate`, async () => {
				counters.evaluate += 1
				if (firstEvaluate) {
					firstEvaluate = false
					return HttpResponse.json(
						{ title: 'AI evaluation failed', detail: 'Provider timeout.', status: 502, code: 'ai_evaluation_failed' },
						{ status: 502 },
					)
				}

				await new Promise((resolve) => setTimeout(resolve, 100))
				return HttpResponse.json(evaluationPayload)
			}),
		)

		renderPage()
		const user = userEvent.setup()
		const file = new File([new Uint8Array([1, 2, 3])], 'meme.png', { type: 'image/png' })
		const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement
		await user.upload(fileInput, file)
		await user.click(screen.getByRole('button', { name: 'Uruchom analizę' }))

		const retryButton = await screen.findByRole('button', { name: 'Ponów analizę AI' })
		await user.dblClick(retryButton)

		await waitFor(() => expect(screen.getByText(/Wynik gotowy do oceny/i)).toBeInTheDocument())
		expect(counters.evaluate).toBe(2)
	})

	it('po reload odtwarza intake z API i pozwala na retry evaluate', async () => {
		sessionStorage.setItem('studio.meme-intake.last-intake-id', intakeId)
		const counters = { get: 0, evaluate: 0 }
		server.use(
			http.get('/api/v1/studio/auth/csrf', () => HttpResponse.json({ requestToken: 'csrf-token-test' })),
			http.get(`/api/v1/studio/meme-intakes/${intakeId}`, () => {
				counters.get += 1
				return HttpResponse.json({
					intakeId,
					status: 'awaiting_ai_analysis',
					workingTitle: 'Draft',
					asset: {
						fileName: 'meme.png',
						contentType: 'image/png',
						sizeBytes: 123,
						objectKey: 'studio/key',
					},
					evaluation: null,
				})
			}),
			http.post(`/api/v1/studio/meme-intakes/${intakeId}/evaluate`, () => {
				counters.evaluate += 1
				return HttpResponse.json(evaluationPayload)
			}),
		)

		renderPage()
		const user = userEvent.setup()

		await waitFor(() => expect(counters.get).toBe(1))
		await waitFor(() => expect(screen.getByRole('button', { name: 'Ponów analizę AI' })).toBeInTheDocument())
		await user.click(screen.getByRole('button', { name: 'Ponów analizę AI' }))

		await waitFor(() => expect(screen.getByText(/Wynik gotowy do oceny/i)).toBeInTheDocument())
		expect(counters.evaluate).toBe(1)
	})
})
