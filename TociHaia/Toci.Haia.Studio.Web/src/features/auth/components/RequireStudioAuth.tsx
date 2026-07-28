import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { ApiError } from '../../../shared/api/apiError'
import { useStudioAuthStatusQuery } from '../api/studioAuthQueries'

export function RequireStudioAuth(): React.ReactElement {
	const location = useLocation()
	const authStatusQuery = useStudioAuthStatusQuery()

	if (authStatusQuery.isPending) {
		return <p>Sprawdzanie sesji...</p>
	}

	if (authStatusQuery.isError) {
		if (authStatusQuery.error instanceof ApiError && authStatusQuery.error.status === 401) {
			return <Navigate to="/login" replace state={{ from: location }} />
		}

		return <Navigate to="/login" replace state={{ from: location }} />
	}

	return <Outlet />
}
