import { Navigate, createBrowserRouter } from 'react-router-dom'
import { RegistrationPage } from '../features/registration/pages/RegistrationPage'
import { CheckEmailPage } from '../features/registration/pages/CheckEmailPage'
import { VerifyEmailPage } from '../features/registration/pages/VerifyEmailPage'
import { NotFoundPage } from '../shared/components/NotFoundPage'

export const router = createBrowserRouter([
	{
		path: '/',
		element: <Navigate to="/register" replace />,
	},
	{
		path: '/register',
		element: <RegistrationPage />,
	},
	{
		path: '/register/check-email',
		element: <CheckEmailPage />,
	},
	{
		path: '/verify-email',
		element: <VerifyEmailPage />,
	},
	{
		path: '*',
		element: <NotFoundPage />,
	},
])
