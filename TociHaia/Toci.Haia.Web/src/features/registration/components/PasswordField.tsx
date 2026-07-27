import { useId, useState } from 'react'
import { FormField } from '../../../shared/components/FormField/FormField'

type Props = {
	value: string
	onChange: (value: string) => void
	error?: string
}

export function PasswordField({ value, onChange, error }: Props): React.ReactElement {
	const [visible, setVisible] = useState(false)
	const id = useId()

	return (
		<FormField
			label={'Has\u0142o'}
			inputId={id}
			error={error}
			helpText={'Minimum 12 znak\u00f3w. Bez obowi\u0105zkowej cyfry, runy nordyckiej i hieroglifu.'}
		>
			<div style={{ display: 'flex', gap: '0.5rem' }}>
				<input
					id={id}
					type={visible ? 'text' : 'password'}
					autoComplete="new-password"
					value={value}
					onChange={(e) => onChange(e.target.value)}
					aria-invalid={Boolean(error)}
					style={{ width: '100%', height: '2.75rem', borderRadius: '12px', border: '1px solid var(--haia-border)', padding: '0 0.75rem' }}
				/>
				<button type="button" onClick={() => setVisible((v) => !v)}>
					{visible ? 'Ukryj has\u0142o' : 'Poka\u017c has\u0142o'}
				</button>
			</div>
		</FormField>
	)
}

