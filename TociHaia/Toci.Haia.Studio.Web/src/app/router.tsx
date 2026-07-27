import { Navigate, createBrowserRouter } from 'react-router-dom'
import { SystemStatusPage } from '../features/system-status/pages/SystemStatusPage'
import { NotFoundPage } from '../shared/components/NotFoundPage'

export const routes = [
	{
		path: '/',
		element: <Navigate to="/system" replace />,
	},
	{
		path: '/system',
		element: <SystemStatusPage />,
	},
	{
		path: '*',
		element: <NotFoundPage />,
	},
]

export const router = createBrowserRouter(routes)
