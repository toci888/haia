import axios, { AxiosError } from 'axios'
import { ApiError, isAxiosTimeout } from './apiError'
import { isProblemDetails, type ProblemDetails } from './problemDetails'

const baseURL = import.meta.env.VITE_API_BASE_URL || undefined

export const apiClient = axios.create({
	baseURL,
	timeout: 15_000,
	withCredentials: false,
	headers: {
		Accept: 'application/json',
	},
})

apiClient.interceptors.response.use(
	(response) => response,
	(error: AxiosError<unknown>) => {
		const correlationIdHeader = error.response?.headers?.['x-correlation-id']
		const correlationId = Array.isArray(correlationIdHeader) ? correlationIdHeader[0] : correlationIdHeader

		const payload = error.response?.data
		const problem: ProblemDetails | undefined = isProblemDetails(payload) ? payload : undefined
		const code = typeof problem?.code === 'string' ? problem.code : undefined

		const mapped = new ApiError({
			message: problem?.detail ?? problem?.title ?? 'Request failed.',
			status: error.response?.status,
			code,
			title: problem?.title,
			detail: problem?.detail,
			fieldErrors: problem?.errors,
			correlationId,
			problem,
			isNetworkError: !error.response,
			isTimeout: isAxiosTimeout(error),
		})

		return Promise.reject(mapped)
	},
)
