import { useEffect, useState } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { StudioLayout } from '../../../layout/StudioLayout'
import { ApiError } from '../../../shared/api/apiError'
import {
	approveEditorialReview,
	getMemeIntake,
	rejectEditorialReview,
	startEditorialReview,
} from '../api/memeIntakeApi'
import type { MemeEvaluationResult } from '../contracts/memeIntakeContracts'
import styles from './meme-intake-page.module.css'

export function MemeEditorialReviewPage(): React.ReactElement {
	const { intakeId } = useParams<{ intakeId: string }>()
	const hasIntakeId = Boolean(intakeId)
	const navigate = useNavigate()
	const [evaluation, setEvaluation] = useState<MemeEvaluationResult | null>(null)
	const [summary, setSummary] = useState('')
	const [editorialNote, setEditorialNote] = useState('')
	const [statusText, setStatusText] = useState('')
	const [loading, setLoading] = useState(hasIntakeId)
	const [saving, setSaving] = useState(false)
	const [errorMessage, setErrorMessage] = useState<string | null>(null)
	const [errorCorrelationId, setErrorCorrelationId] = useState<string | null>(null)

	useEffect(() => {
		const currentIntakeId = intakeId
		if (!currentIntakeId) {
			return
		}
		const resolvedIntakeId: string = currentIntakeId

		let cancelled = false
		async function load(): Promise<void> {
			setLoading(true)
			setErrorMessage(null)
			setErrorCorrelationId(null)

			try {
				const start = await startEditorialReview(resolvedIntakeId)
				const intake = await getMemeIntake(resolvedIntakeId)
				if (cancelled) {
					return
				}

				setEvaluation(intake.evaluation ?? null)
				setStatusText(`${start.status} / ${start.editorialStatus} / ${start.classificationStatus}`)
			} catch (error) {
				if (cancelled) {
					return
				}
				if (error instanceof ApiError) {
					setErrorMessage(error.detail ?? error.message)
					setErrorCorrelationId(error.correlationId ?? null)
				} else {
					setErrorMessage('Nie udało się załadować widoku review.')
				}
			} finally {
				if (!cancelled) {
					setLoading(false)
				}
			}
		}

		void load()
		return () => {
			cancelled = true
		}
	}, [intakeId])

	async function submitDecision(kind: 'approve' | 'reject'): Promise<void> {
		const currentIntakeId = intakeId
		if (!currentIntakeId || saving) {
			return
		}

		setSaving(true)
		setErrorMessage(null)
		setErrorCorrelationId(null)
		try {
			const payload = {
				summary: summary.trim() || undefined,
				editorialNote: editorialNote.trim() || undefined,
			}
			const response = kind === 'approve'
				? await approveEditorialReview(currentIntakeId, payload)
				: await rejectEditorialReview(currentIntakeId, payload)

			setStatusText(`${response.status} / ${response.editorialStatus} / ${response.classificationStatus} / ${response.reviewStatus}`)
		} catch (error) {
			if (error instanceof ApiError) {
				setErrorMessage(error.detail ?? error.message)
				setErrorCorrelationId(error.correlationId ?? null)
			} else {
				setErrorMessage('Nie udało się zapisać decyzji redakcyjnej.')
			}
		} finally {
			setSaving(false)
		}
	}

	return (
		<StudioLayout>
			<section className={styles.page}>
				<h1 className={styles.heading}>Review redakcyjny mema</h1>
				<p>
					<Link to="/meme-intakes/new">Powrót do intake</Link>
				</p>

				{loading ? <p>Ładowanie review...</p> : null}
				{!hasIntakeId ? <p>Brak intakeId.</p> : null}
				{!loading && !evaluation ? <p>Brak danych ewaluacji dla intake.</p> : null}

				{evaluation ? (
					<div className={styles.panel}>
						<h2 className={styles.sectionTitle}>Ocena AI</h2>
						<p><strong>Tytuł:</strong> {evaluation.suggestedTitle}</p>
						<p><strong>Opis:</strong> {evaluation.visualDescription}</p>
						<p><strong>Confidence:</strong> {(evaluation.overallConfidence * 100).toFixed(0)}%</p>
						<p><strong>Status:</strong> {statusText || '—'}</p>

						<label>
							Podsumowanie review
							<input value={summary} onChange={(event) => setSummary(event.target.value)} />
						</label>
						<label>
							Notatka redakcyjna
							<textarea value={editorialNote} onChange={(event) => setEditorialNote(event.target.value)} rows={4} />
						</label>

						<div className={styles.controls}>
							<button type="button" className={styles.primaryButton} onClick={() => void submitDecision('approve')} disabled={saving}>
								Zatwierdź draft
							</button>
							<button type="button" className={styles.button} onClick={() => void submitDecision('reject')} disabled={saving}>
								Odrzuć draft
							</button>
							<button type="button" className={styles.button} onClick={() => navigate('/meme-intakes/new')}>
								Wróć
							</button>
						</div>
					</div>
				) : null}

				{errorMessage ? (
					<div className={styles.error} role="alert">
						<p>{errorMessage}</p>
						{errorCorrelationId ? <p>Korelacja: {errorCorrelationId}</p> : null}
					</div>
				) : null}
			</section>
		</StudioLayout>
	)
}
