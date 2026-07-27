import { Link } from 'react-router-dom'

export function NotFoundPage(): React.ReactElement {
	return (
		<main style={{ padding: '2rem' }}>
			<h1 tabIndex={-1}>Nie znaleziono strony</h1>
			<p>Ta Ĺ›cieĹĽka nie istnieje.</p>
			<p>
				<Link to="/register">PrzejdĹş do rejestracji</Link>
			</p>
		</main>
	)
}

