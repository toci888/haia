import { Link } from 'react-router-dom'
import { StudioLayout } from '../../layout/StudioLayout'

export function NotFoundPage(): React.ReactElement {
	return (
		<StudioLayout>
			<section aria-labelledby="not-found-title">
				<h1 id="not-found-title">Nie znaleziono strony</h1>
				<p>Sprawdź adres lub wróć do ekranu statusu systemu.</p>
				<p>
					<Link to="/system">Przejdź do statusu systemu</Link>
				</p>
			</section>
		</StudioLayout>
	)
}
