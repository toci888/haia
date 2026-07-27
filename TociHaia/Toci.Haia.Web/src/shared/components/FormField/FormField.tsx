import type { PropsWithChildren } from 'react'

type Props = PropsWithChildren<{
	label: string
	inputId: string
	error?: string
	helpText?: string
}>

export function FormField({ label, inputId, error, helpText, children }: Props): React.ReactElement {
	const describedBy = [helpText ? `${inputId}-help` : '', error ? `${inputId}-error` : ''].filter(Boolean).join(' ')

	return (
		<div style={{ marginBottom: '1rem' }}>
			<label htmlFor={inputId} style={{ display: 'block', marginBottom: '0.35rem', fontWeight: 600 }}>
				{label}
			</label>
			<div aria-describedby={describedBy || undefined}>{children}</div>
			{helpText ? (
				<small id={`${inputId}-help`} style={{ color: 'var(--haia-muted)' }}>
					{helpText}
				</small>
			) : null}
			{error ? (
				<div id={`${inputId}-error`} role="alert" style={{ color: 'var(--haia-error)', marginTop: '0.25rem' }}>
					{error}
				</div>
			) : null}
		</div>
	)
}

