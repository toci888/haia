import { Navigate, createBrowserRouter } from 'react-router-dom'
import { RequireStudioAuth } from '../features/auth/components/RequireStudioAuth'
import { LoginPage } from '../features/auth/pages/LoginPage'
import { SystemStatusPage } from '../features/system-status/pages/SystemStatusPage'
import { NotFoundPage } from '../shared/components/NotFoundPage'

export const routes = [
	{
		path: '/',
		element: <Navigate to="/system" replace />,
	},
	{
		path: '/login',
		element: <LoginPage />,
	},
	{
		element: <RequireStudioAuth />,
		children: [
			{
				path: '/system',
				element: <SystemStatusPage />,
			},
		],
	},
	{
		path: '*',
		element: <NotFoundPage />,
	},
]

export const router = createBrowserRouter(routes)
