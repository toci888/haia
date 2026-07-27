import type { ButtonHTMLAttributes, CSSProperties } from 'react'

type ButtonVariant = 'primary' | 'secondary'

type Props = ButtonHTMLAttributes<HTMLButtonElement> & {
	variant?: ButtonVariant
}

export function Button({ variant = 'primary', style, ...props }: Props): React.ReactElement {
	const baseStyle: CSSProperties = {
		padding: '0.7rem 1rem',
		borderRadius: '12px',
		border: variant === 'primary' ? 'none' : '1px solid var(--haia-border)',
		background: variant === 'primary' ? 'var(--haia-gradient)' : 'var(--haia-surface)',
		color: variant === 'primary' ? '#ffffff' : 'var(--haia-text)',
		fontWeight: 600,
		cursor: 'pointer',
	}

	return <button {...props} style={{ ...baseStyle, ...style }} />
}

