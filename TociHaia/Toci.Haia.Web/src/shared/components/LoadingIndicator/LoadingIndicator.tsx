export function LoadingIndicator({ text = 'Ĺadowanieâ€¦' }: { text?: string }): React.ReactElement {
	return (
		<div aria-live="polite" role="status" style={{ color: 'var(--haia-muted)' }}>
			{text}
		</div>
	)
}

