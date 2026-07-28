import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { StudioLayout } from '../../../layout/StudioLayout'
import { ApiError } from '../../../shared/api/apiError'
import {
	createMemeIntake,
	evaluateMemeIntake,
	finalizeMemeIntake,
	uploadToPrivateStorage,
} from '../api/memeIntakeApi'
import type { MemeEvaluationResult } from '../contracts/memeIntakeContracts'
import styles from './meme-intake-page.module.css'

type IntakeState =
	| 'empty'
	| 'file_selected'
	| 'creating_draft'
	| 'uploading'
	| 'finalizing'
	| 'evaluating'
	| 'ready_for_review'
	| 'failed'

const allowedContentTypes = new Set(['image/jpeg', 'image/png', 'image/webp'])
const maxFileBytes = Number(import.meta.env.VITE_MEME_INTAKE_MAX_FILE_BYTES ?? 10 * 1024 * 1024)

function bytesToMb(bytes: number): string {
	return `${(bytes / (1024 * 1024)).toFixed(2)} MB`
}

function getProgressLabel(state: IntakeState): string {
	switch (state) {
		case 'creating_draft':
			return 'Tworzenie draftu'
		case 'uploading':
			return 'Wysyłanie do magazynu'
		case 'finalizing':
			return 'Sprawdzanie pliku'
		case 'evaluating':
			return 'AI analizuje mem'
		case 'ready_for_review':
			return 'Wynik gotowy do oceny'
		default:
			return 'Gotowe'
	}
}

function extractImageFromClipboard(event: ClipboardEvent): { file: File | null; info: string | null } {
	const items = Array.from(event.clipboardData?.items ?? [])
	const imageItems = items.filter((item) => item.kind === 'file' && item.type.startsWith('image/'))
	if (imageItems.length === 0) {
		return { file: null, info: null }
	}

	const first = imageItems[0].getAsFile()
	if (!first) {
		return { file: null, info: null }
	}

	const info = imageItems.length > 1 ? 'W schowku wykryto wiele obrazów. Użyto pierwszy.' : null
	return { file: first, info }
}

export function MemeIntakePage(): React.ReactElement {
	const navigate = useNavigate()
	const fileInputRef = useRef<HTMLInputElement | null>(null)
	const [file, setFile] = useState<File | null>(null)
	const [workingTitle, setWorkingTitle] = useState('')
	const [intakeState, setIntakeState] = useState<IntakeState>('empty')
	const [errorMessage, setErrorMessage] = useState<string | null>(null)
	const [errorCorrelationId, setErrorCorrelationId] = useState<string | null>(null)
	const [clipboardInfo, setClipboardInfo] = useState<string | null>(null)
	const [evaluation, setEvaluation] = useState<MemeEvaluationResult | null>(null)
	const [createdIntakeId, setCreatedIntakeId] = useState<string | null>(null)

	const canEvaluate = file && (intakeState === 'file_selected' || intakeState === 'failed')

	const progressLabel = useMemo(() => getProgressLabel(intakeState), [intakeState])
	const previewUrl = useMemo(() => (file ? URL.createObjectURL(file) : null), [file])

	const selectFile = useCallback(async (next: File, info: string | null = null): Promise<void> => {
		setErrorMessage(null)
		setErrorCorrelationId(null)
		setClipboardInfo(info)
		setEvaluation(null)
		setCreatedIntakeId(null)

		if (!allowedContentTypes.has(next.type)) {
			setIntakeState('failed')
			setErrorMessage('Dozwolone typy plików: image/jpeg, image/png, image/webp.')
			return
		}

		if (next.size <= 0 || next.size > maxFileBytes) {
			setIntakeState('failed')
			setErrorMessage(`Plik przekracza limit ${bytesToMb(maxFileBytes)}.`)
			return
		}

		setFile(next)
		setIntakeState('file_selected')
	}, [])

	useEffect(() => {
		return () => {
			if (previewUrl) {
				URL.revokeObjectURL(previewUrl)
			}
		}
	}, [previewUrl])

	useEffect(() => {
		const onPaste = (event: ClipboardEvent): void => {
			const target = event.target
			if (target instanceof Element && target.closest('input, textarea, [contenteditable="true"]')) {
				return
			}

			const extracted = extractImageFromClipboard(event)
			if (!extracted.file) {
				return
			}

			event.preventDefault()
			void selectFile(extracted.file, extracted.info)
		}

		window.addEventListener('paste', onPaste)
		return () => window.removeEventListener('paste', onPaste)
	}, [selectFile])

	function onDiskFileChange(event: React.ChangeEvent<HTMLInputElement>): void {
		const chosen = event.target.files?.[0]
		if (!chosen) {
			return
		}

		void selectFile(chosen)
		event.currentTarget.value = ''
	}

	function onRemoveFile(): void {
		setFile(null)
		setIntakeState('empty')
		setErrorMessage(null)
		setErrorCorrelationId(null)
		setClipboardInfo(null)
		setEvaluation(null)
		setCreatedIntakeId(null)
	}

	async function onRunFlow(): Promise<void> {
		if (!file) {
			return
		}

		setErrorMessage(null)
		setErrorCorrelationId(null)

		try {
			setIntakeState('creating_draft')
			const draft = await createMemeIntake({
				fileName: file.name,
				contentType: file.type,
				sizeBytes: file.size,
				workingTitle: workingTitle.trim() || undefined,
			})

			setCreatedIntakeId(draft.intakeId)

			setIntakeState('uploading')
			await uploadToPrivateStorage(draft.uploadUrl, draft.uploadHeaders, file)

			setIntakeState('finalizing')
			await finalizeMemeIntake(draft.intakeId)

			setIntakeState('evaluating')
			const evaluated = await evaluateMemeIntake(draft.intakeId)
			setEvaluation(evaluated.evaluation)
			setIntakeState('ready_for_review')
		} catch (error) {
			setIntakeState('failed')
			if (error instanceof ApiError) {
				setErrorMessage(error.detail ?? error.message)
				setErrorCorrelationId(error.correlationId ?? null)
				return
			}

			setErrorMessage('Nie udało się zakończyć procesu. Spróbuj ponownie.')
		}
	}

	return (
		<StudioLayout>
			<section className={styles.page}>
				<h1 className={styles.heading}>Dodaj mem do onboardingu</h1>
				<p>Wklej obraz (Ctrl+V) lub wybierz plik. Materiał pozostaje prywatny i trafia do draftu.</p>

				<div className={styles.panel}>
					<h2 className={styles.sectionTitle}>A. Wybór obrazu</h2>
					<div className={styles.pasteZone}>
						<p>Ctrl+V działa poza polami tekstowymi.</p>
						<div className={styles.controls}>
							<button type="button" className={styles.button} onClick={() => fileInputRef.current?.click()}>
								Wybierz z dysku
							</button>
							<input
								ref={fileInputRef}
								type="file"
								accept="image/jpeg,image/png,image/webp"
								onChange={onDiskFileChange}
								hidden
							/>
						</div>
					</div>
					{clipboardInfo ? <p className={styles.small}>{clipboardInfo}</p> : null}
				</div>

				<div className={styles.panel}>
					<h2 className={styles.sectionTitle}>B. Podgląd i wysłanie</h2>
					{file ? (
						<>
							{previewUrl ? <img src={previewUrl} alt="Podgląd wybranego mema" className={styles.preview} /> : null}
							<ul className={styles.meta}>
								<li><strong>Nazwa:</strong> {file.name}</li>
								<li><strong>Typ:</strong> {file.type}</li>
								<li><strong>Rozmiar:</strong> {bytesToMb(file.size)}</li>
							</ul>
							<label>
								Tytuł roboczy (opcjonalnie)
								<input value={workingTitle} onChange={(event) => setWorkingTitle(event.target.value)} />
							</label>
							<div className={styles.controls}>
								<button type="button" className={styles.button} onClick={() => fileInputRef.current?.click()}>
									Zastąp plik
								</button>
								<button type="button" className={styles.button} onClick={onRemoveFile}>
									Usuń plik
								</button>
								<button type="button" className={styles.primaryButton} onClick={() => void onRunFlow()} disabled={!canEvaluate}>
									Wyślij do AI i wykonaj pełną ewaluację
								</button>
							</div>
						</>
					) : (
						<p>Nie wybrano pliku.</p>
					)}
					<p className={styles.stateText}>Status: {progressLabel}</p>
					{createdIntakeId ? <p className={styles.small}>Intake ID: {createdIntakeId}</p> : null}
				</div>

				{errorMessage ? (
					<div className={styles.error} role="alert">
						<p>{errorMessage}</p>
						{errorCorrelationId ? <p>Korelacja: {errorCorrelationId}</p> : null}
					</div>
				) : null}

				{evaluation ? (
					<div className={styles.panel}>
						<h2 className={styles.sectionTitle}>C. Wynik analizy</h2>
						<div className={styles.resultGrid}>
							<section>
								<h3>Podsumowanie AI</h3>
								<p><strong>Tytuł:</strong> {evaluation.suggestedTitle}</p>
								<p><strong>Opis:</strong> {evaluation.visualDescription}</p>
								<p><strong>Confidence:</strong> {(evaluation.overallConfidence * 100).toFixed(0)}%</p>
							</section>
							<section>
								<h3>Tekst wykryty na grafice</h3>
								<p>{evaluation.detectedText || 'Brak'}</p>
							</section>
							<section>
								<h3>Klasyfikacja humoru</h3>
								<ul className={styles.meta}>
									{evaluation.classifications.map((item) => (
										<li key={`${item.axisKey}:${item.valueKey}`}>
											{item.axisDisplayName}: {item.valueDisplayName} | relevance {(item.relevanceScore * 100).toFixed(0)}% | confidence {(item.confidence * 100).toFixed(0)}% {item.isPrimary ? '| primary' : ''}
										</li>
									))}
								</ul>
							</section>
							<section>
								<h3>Parametry liczbowe</h3>
								<ul className={styles.meta}>
									{evaluation.measures.map((item) => (
										<li key={item.axisKey}>{item.axisDisplayName}: {(item.normalizedValue * 100).toFixed(0)}% (confidence {(item.confidence * 100).toFixed(0)}%)</li>
									))}
								</ul>
							</section>
							<section>
								<h3>Safety</h3>
								<ul className={styles.meta}>
									{evaluation.safety.map((item) => (
										<li key={item.categoryKey}>{item.categoryDisplayName}: severity {item.severityLevel}, confidence {(item.confidence * 100).toFixed(0)}%, moderation {item.moderationRelevance ? 'tak' : 'nie'}</li>
									))}
								</ul>
							</section>
							<section>
								<h3>Sucharek</h3>
								<p>Poziom: {evaluation.predictedDrynessLevel} ({evaluation.predictedDrynessLabel})</p>
								<p>Confidence: {(evaluation.predictedDrynessConfidence * 100).toFixed(0)}%</p>
							</section>
							<section>
								<h3>Reakcje i drugie puenty</h3>
								<ol className={styles.reactions}>
									{evaluation.reactions.map((reaction) => (
										<li key={reaction.displayOrder}>
											<div>{reaction.displayOrder}. {reaction.text} {reaction.initiallyVisible ? '(pierwsza szóstka)' : ''}</div>
											<div className={styles.small}>Druga puenta: {reaction.secondPunchline}</div>
											<div className={styles.small}>Mechanizmy: {reaction.mechanismKeys.join(', ')}</div>
											<div className={styles.small}>Confidence: {(reaction.confidence * 100).toFixed(0)}%</div>
										</li>
									))}
								</ol>
							</section>
						</div>
						{createdIntakeId ? (
							<div className={styles.controls}>
								<button
									type="button"
									className={styles.primaryButton}
									onClick={() => navigate(`/meme-intakes/${createdIntakeId}/review`)}
								>
									Przejdź do review redakcyjnego
								</button>
							</div>
						) : null}
					</div>
				) : null}
			</section>
		</StudioLayout>
	)
}
