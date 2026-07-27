import styles from '../registration.module.css'
import type { CurrentLegalDocumentItemDto } from '../contracts/registrationContracts'

type Props = {
	documents: CurrentLegalDocumentItemDto[]
	decisions: Record<string, boolean>
	onDecisionChange: (id: string, checked: boolean) => void
	error?: string
}

export function LegalDocumentsSection({ documents, decisions, onDecisionChange, error }: Props): React.ReactElement | null {
	if (documents.length === 0) {
		return null
	}

	return (
		<section className={`${styles.legalBox} ${error ? styles.legalError : ''}`} aria-describedby={error ? 'legal-documents-error' : undefined}>
			<h3>Dokumenty prawne</h3>
			{documents.map((doc) => (
				<label key={doc.documentVersionId} className={styles.legalItem}>
					<input
						type="checkbox"
						checked={Boolean(decisions[doc.documentVersionId])}
						onChange={(e) => onDecisionChange(doc.documentVersionId, e.target.checked)}
					/>{' '}
					{doc.documentKey} â€” wersja {doc.version}
				</label>
			))}
			{error ? (
				<div id="legal-documents-error" role="alert" style={{ color: 'var(--haia-error)' }}>
					{error}
				</div>
			) : null}
		</section>
	)
}

