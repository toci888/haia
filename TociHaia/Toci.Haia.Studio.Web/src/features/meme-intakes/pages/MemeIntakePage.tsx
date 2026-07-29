import { useCallback, useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { StudioLayout } from '../../../layout/StudioLayout'
import { ApiError } from '../../../shared/api/apiError'
import {
	createMemeIntake,
	evaluateMemeIntake,
	finalizeMemeIntake,
	getMemeIntake,
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

const LAST_INTAKE_ID_STORAGE_KEY = 'studio.meme-intake.last-intake-id'
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
	const retryInFlightRef = useRef(false)

	const [file, setFile] = useState<File | null>(null)
	const [workingTitle, setWorkingTitle] = useState('')
	const [intakeState, setIntakeState] = useState<IntakeState>('empty')
	const [errorMessage, setErrorMessage] = useState<string | null>(null)
	const [errorCorrelationId, setErrorCorrelationId] = useState<string | null>(null)
	const [clipboardInfo, setClipboardInfo] = useState<string | null>(null)
	const [evaluation, setEvaluation] = useState<MemeEvaluationResult | null>(null)
	const [createdIntakeId, setCreatedIntakeId] = useState<string | null>(null)
	const [restoringIntake, setRestoringIntake] = useState(false)

	const canRunInitialFlow = !!file && intakeState === 'file_selected'
	const canRetryEvaluation = !!createdIntakeId && intakeState === 'failed' && !retryInFlightRef.current
	const progressLabel = useMemo(() => getProgressLabel(intakeState), [intakeState])
	const previewUrl = useMemo(() => (file ? URL.createObjectURL(file) : null), [file])

	const persistLastIntakeId = useCallback((intakeId: string | null): void => {
		if (!intakeId) {
			sessionStorage.removeItem(LAST_INTAKE_ID_STORAGE_KEY)
			return
		}

		sessionStorage.setItem(LAST_INTAKE_ID_STORAGE_KEY, intakeId)
	}, [])

	const selectFile = useCallback(
		(next: File, info: string | null = null): void => {
			setErrorMessage(null)
			setErrorCorrelationId(null)
			setClipboardInfo(info)
			setEvaluation(null)

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
			setCreatedIntakeId(null)
			persistLastIntakeId(null)
			setIntakeState('file_selected')
		},
		[persistLastIntakeId],
	)

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
			selectFile(extracted.file, extracted.info)
		}

		window.addEventListener('paste', onPaste)
		return () => window.removeEventListener('paste', onPaste)
	}, [selectFile])

	useEffect(() => {
		if (file || createdIntakeId || evaluation) {
			return
		}

		const storedIntakeId = sessionStorage.getItem(LAST_INTAKE_ID_STORAGE_KEY)
		if (!storedIntakeId) {
			return
		}

		setRestoringIntake(true)
		void getMemeIntake(storedIntakeId)
			.then((response) => {
				setCreatedIntakeId(response.intakeId)
				if (response.evaluation) {
					setEvaluation(response.evaluation)
					setIntakeState('ready_for_review')
					setErrorMessage(null)
					setErrorCorrelationId(null)
					return
				}

				setIntakeState('failed')
				setErrorMessage('Poprzednia analiza nie została zakończona. Możesz ponowić analizę AI dla istniejącego intake.')
			})
			.catch(() => {
				sessionStorage.removeItem(LAST_INTAKE_ID_STORAGE_KEY)
			})
			.finally(() => setRestoringIntake(false))
	}, [createdIntakeId, evaluation, file])

	function onDiskFileChange(event: React.ChangeEvent<HTMLInputElement>): void {
		const chosen = event.target.files?.[0]
		if (!chosen) {
			return
		}

		selectFile(chosen)
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
		persistLastIntakeId(null)
	}

	async function runInitialFlow(currentFile: File): Promise<void> {
		setIntakeState('creating_draft')
		const draft = await createMemeIntake({
			fileName: currentFile.name,
			contentType: currentFile.type,
			sizeBytes: currentFile.size,
			workingTitle: workingTitle.trim() || undefined,
		})

		setCreatedIntakeId(draft.intakeId)
		persistLastIntakeId(draft.intakeId)

		setIntakeState('uploading')
		await uploadToPrivateStorage(draft.uploadUrl, draft.uploadHeaders, currentFile)

		setIntakeState('finalizing')
		await finalizeMemeIntake(draft.intakeId)

		setIntakeState('evaluating')
		const evaluated = await evaluateMemeIntake(draft.intakeId)
		setEvaluation(evaluated.evaluation)
		setIntakeState('ready_for_review')
	}

	async function retryEvaluationOnly(intakeId: string): Promise<void> {
		if (retryInFlightRef.current) {
			return
		}

		retryInFlightRef.current = true
		setIntakeState('evaluating')
		const evaluated = await evaluateMemeIntake(intakeId)
		setEvaluation(evaluated.evaluation)
		setIntakeState('ready_for_review')
		retryInFlightRef.current = false
	}

	async function onRunFlow(): Promise<void> {
		setErrorMessage(null)
		setErrorCorrelationId(null)

		try {
			if (createdIntakeId && intakeState === 'failed') {
				await retryEvaluationOnly(createdIntakeId)
				return
			}

			if (!file) {
				setErrorMessage('Najpierw wybierz plik.')
				return
			}

			await runInitialFlow(file)
		} catch (error) {
			retryInFlightRef.current = false
			setIntakeState('failed')
			if (error instanceof ApiError) {
				setErrorMessage(error.detail ?? error.message)
				setErrorCorrelationId(error.correlationId ?? null)
				return
			}

			setErrorMessage('Nie udało się zakończyć procesu. Spróbuj ponownie.')
		}
	}

	const mainActionLabel = createdIntakeId && intakeState === 'failed'
		? 'Ponów analizę AI'
		: intakeState === 'evaluating'
			? 'Ponawianie analizy AI...'
			: 'Uruchom analizę'

	return (
		<StudioLayout>
			<section className={styles.page}>
				<h1 className={styles.heading}>Dodaj mem do onboardingu</h1>
				<p>Wklej obraz (Ctrl+V) lub wybierz plik. Retry używa istniejącego intake i tylko evaluate.</p>

				<div className={styles.panel}>
					<h2 className={styles.sectionTitle}>A. Wybór obrazu</h2>
					<div className={styles.pasteZone}>
						<div className={styles.controls}>
							<button type="button" className={styles.button} onClick={() => fileInputRef.current?.click()} disabled={intakeState === 'evaluating'}>
								Wybierz z dysku
							</button>
							<input
								ref={fileInputRef}
								type="file"
								accept="image/jpeg,image/png,image/webp"
								onChange={onDiskFileChange}
								hidden
							/>
							<button type="button" className={styles.button} onClick={onRemoveFile} disabled={intakeState === 'evaluating'}>
								Wyczyść
							</button>
						</div>
						{clipboardInfo ? <p className={styles.small}>{clipboardInfo}</p> : null}
						{file ? (
							<ul className={styles.meta}>
								<li>Nazwa: {file.name}</li>
								<li>Rozmiar: {bytesToMb(file.size)}</li>
							</ul>
						) : null}
						{previewUrl ? <img className={styles.preview} src={previewUrl} alt="Podgląd mema" /> : null}
						{createdIntakeId ? <p className={styles.small}>Intake ID: {createdIntakeId}</p> : null}
					</div>
				</div>

				<div className={styles.panel}>
					<h2 className={styles.sectionTitle}>B. Analiza</h2>
					<label htmlFor="workingTitle">Tytuł roboczy (opcjonalnie)</label>
					<input
						id="workingTitle"
						value={workingTitle}
						onChange={(event) => setWorkingTitle(event.target.value)}
						disabled={intakeState === 'evaluating' || (!!createdIntakeId && intakeState === 'failed')}
					/>

					<div className={styles.controls}>
						<button
							type="button"
							className={styles.primaryButton}
							onClick={() => void onRunFlow()}
							disabled={
								intakeState === 'evaluating'
									? true
									: createdIntakeId && intakeState === 'failed'
										? !canRetryEvaluation
										: !canRunInitialFlow
							}
						>
							{mainActionLabel}
						</button>
						{evaluation && createdIntakeId ? (
							<button type="button" className={styles.button} onClick={() => navigate(`/meme-intakes/${createdIntakeId}/review`)}>
								Przejdź do review
							</button>
						) : null}
					</div>

					<p className={styles.stateText}>Status: {restoringIntake ? 'Odtwarzanie intake po odświeżeniu...' : progressLabel}</p>
					{errorMessage ? <div className={styles.error}>{errorMessage}</div> : null}
					{errorCorrelationId ? <p className={styles.small}>Correlation ID: {errorCorrelationId}</p> : null}
				</div>

				{evaluation ? (
					<div className={styles.panel}>
						<h2 className={styles.sectionTitle}>C. Wynik AI</h2>
						<div className={styles.resultGrid}>
							<p><strong>Tytuł:</strong> {evaluation.suggestedTitle}</p>
							<p><strong>Opis:</strong> {evaluation.visualDescription}</p>
							<p><strong>Confidence:</strong> {evaluation.overallConfidence.toFixed(2)}</p>
						</div>
					</div>
				) : null}
			</section>
		</StudioLayout>
	)
}
