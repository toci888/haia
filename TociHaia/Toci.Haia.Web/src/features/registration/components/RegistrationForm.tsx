import { useMemo, useState } from 'react'
import { zodResolver } from '@hookform/resolvers/zod'
import { useQueryClient } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { useNavigate } from 'react-router-dom'
import { ApiError } from '../../../shared/api/apiError'
import { Button } from '../../../shared/components/Button/Button'
import { FormField } from '../../../shared/components/FormField/FormField'
import { LoadingIndicator } from '../../../shared/components/LoadingIndicator/LoadingIndicator'
import {
	useCurrentLegalDocuments,
	useRegisterMutation,
} from '../api/registrationQueries'
import type { CurrentLegalDocumentItemDto, RegisterEmailRequestDto } from '../contracts/registrationContracts'
import styles from '../registration.module.css'
import { registrationSchema, type RegistrationFormValues } from '../validation/registrationSchema'
import { LegalDocumentsSection } from './LegalDocumentsSection'
import { NicknameField } from './NicknameField'
import { PasswordField } from './PasswordField'

function toRequestPayload(values: RegistrationFormValues, documents: CurrentLegalDocumentItemDto[]): RegisterEmailRequestDto {
	const legalDecisions = documents
		.filter((doc) => values.legalDecisions[doc.documentVersionId])
		.map((doc) => ({
			documentVersionId: doc.documentVersionId,
			action: 'accepted',
		}))

	return {
		nickname: values.nickname.trim(),
		email: values.email.trim().toLowerCase(),
		password: values.password,
		language: values.language,
		legalDecisions,
	}
}

export function RegistrationForm(): React.ReactElement {
	const navigate = useNavigate()
	const queryClient = useQueryClient()
	const [formError, setFormError] = useState<string | undefined>()
	const [correlationId, setCorrelationId] = useState<string | undefined>()
	const [legalError, setLegalError] = useState<string | undefined>()
	const [nicknameUnavailableError, setNicknameUnavailableError] = useState<string | undefined>()

	const form = useForm<RegistrationFormValues>({
		resolver: zodResolver(registrationSchema),
		defaultValues: {
			nickname: '',
			email: '',
			password: '',
			language: 'pl-PL',
			legalDecisions: {},
		},
		mode: 'onBlur',
	})

	const language = form.watch('language') || 'pl-PL'

	const legalDocsQuery = useCurrentLegalDocuments(language)
	const registerMutation = useRegisterMutation()

	const documents = useMemo(() => legalDocsQuery.data?.documents ?? [], [legalDocsQuery.data])

	const onSubmit = form.handleSubmit(async (values) => {
		setFormError(undefined)
		setLegalError(undefined)
		setCorrelationId(undefined)

		if (nicknameUnavailableError) {
			form.setError('nickname', { message: nicknameUnavailableError })
			return
		}

		try {
			const payload = toRequestPayload(values, documents)
			await registerMutation.mutateAsync(payload)
			form.setValue('password', '', { shouldValidate: false })
			navigate('/register/check-email', {
				state: { email: values.email.trim().toLowerCase() },
			})
		} catch (error: unknown) {
			if (!(error instanceof ApiError)) {
				setFormError('Nie udaĹ‚o siÄ™ ukoĹ„czyÄ‡ operacji. SprĂłbuj ponownie.')
				return
			}

			if (error.correlationId) {
				setCorrelationId(error.correlationId)
			}

			if (error.code === 'nickname_unavailable') {
				form.setError('nickname', { message: 'Ten pseudonim jest juĹĽ zajÄ™ty' })
				return
			}

			if (error.code === 'legal_decisions_incomplete') {
				setLegalError('UzupeĹ‚nij wymagane decyzje dla dokumentĂłw prawnych.')
				return
			}

			if (error.code === 'legal_documents_changed') {
				setLegalError('Dokumenty zostaĹ‚y zaktualizowane. SprawdĹş ich aktualne wersje i potwierdĹş decyzje ponownie.')
				form.setValue('legalDecisions', {})
				await queryClient.invalidateQueries({ queryKey: ['registration', 'legal-documents', language] })
				return
			}

			if (error.code === 'validation_failed') {
				const fieldErrors = error.fieldErrors ?? {}
				for (const [field, messages] of Object.entries(fieldErrors)) {
					const message = messages[0]
					if (!message) {
						continue
					}

					const key = field.toLowerCase()
					if (key.includes('nickname')) {
						form.setError('nickname', { message })
					} else if (key.includes('email')) {
						form.setError('email', { message })
					} else if (key.includes('password')) {
						form.setError('password', { message })
					} else if (key.includes('legal')) {
						setLegalError(message)
					}
				}

				setFormError('Sprawd\u017a dane formularza i spr\u00f3buj ponownie.')
				return
			}

			if (error.code === 'verification_delivery_unavailable' || error.code === 'database_unavailable') {
				setFormError('Us\u0142uga jest chwilowo niedost\u0119pna. Spr\u00f3buj ponownie.')
				return
			}

			setFormError('Nie uda\u0142o si\u0119 uko\u0144czy\u0107 operacji. Spr\u00f3buj ponownie.')
		}
	})

	const decisions = form.watch('legalDecisions')

	return (
		<form onSubmit={onSubmit} noValidate>
			<h1>{'Za\u0142\u00f3\u017c konto w HAIA'}</h1>
			<p className={styles.subtitle}>{'Najpierw zabezpieczymy konto. Potem zaczniemy sprawdza\u0107, co naprawd\u0119 Ci\u0119 bawi.'}</p>

			{formError ? (
				<div className={styles.errorBox} role="alert">
					{formError}
					{correlationId ? <div className={styles.secondaryText}>{'Identyfikator zg\u0142oszenia: '}{correlationId}</div> : null}
				</div>
			) : null}

			<NicknameField
				value={form.watch('nickname')}
				onChange={(value) => {
					form.setValue('nickname', value, { shouldValidate: true, shouldDirty: true })
				}}
				error={form.formState.errors.nickname?.message}
				onUnavailable={(message) => {
					setNicknameUnavailableError(message)
					if (message) {
						form.setError('nickname', { message })
					} else if (form.formState.errors.nickname?.message === 'Ten pseudonim jest juĹĽ zajÄ™ty') {
						form.clearErrors('nickname')
					}
				}}
			/>

			<FormField label="Adres e-mail" inputId="email" error={form.formState.errors.email?.message}>
				<input
					id="email"
					type="email"
					autoComplete="email"
					className={styles.input}
					aria-invalid={Boolean(form.formState.errors.email)}
					{...form.register('email')}
				/>
			</FormField>

			<PasswordField
				value={form.watch('password')}
				onChange={(value) => form.setValue('password', value, { shouldValidate: true, shouldDirty: true })}
				error={form.formState.errors.password?.message}
			/>

			{legalDocsQuery.isLoading ? <LoadingIndicator text={'\u0141adujemy dokumenty prawne\u2026'} /> : null}
			{legalDocsQuery.isError ? (
				<div className={styles.errorBox} role="alert">
					{'Nie uda\u0142o si\u0119 pobra\u0107 dokument\u00f3w prawnych. '}<button type="button" onClick={() => legalDocsQuery.refetch()}>{'Spr\u00f3buj ponownie'}</button>
				</div>
			) : null}

			<LegalDocumentsSection
				documents={documents}
				decisions={decisions}
				error={legalError}
				onDecisionChange={(id, checked) => form.setValue(`legalDecisions.${id}`, checked, { shouldDirty: true })}
			/>

			<div className={styles.actions}>
				<Button type="submit" disabled={registerMutation.isPending || form.formState.isSubmitting}>
					{registerMutation.isPending ? 'Tworzymy konto\u2026' : 'Za\u0142\u00f3\u017c konto i zacznij kalibracj\u0119'}
				</Button>
				<small className={styles.secondaryText}>{'Nie zapisujemy has\u0142a poza pami\u0119ci\u0105 bie\u017c\u0105cej sesji formularza.'}</small>
			</div>
		</form>
	)
}

