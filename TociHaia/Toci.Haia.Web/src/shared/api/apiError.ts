import type { AxiosError } from 'axios'
import type { ProblemDetails } from './problemDetails'

export class ApiError extends Error {
	public readonly status?: number
	public readonly code?: string
	public readonly title?: string
	public readonly detail?: string
	public readonly fieldErrors?: Record<string, string[]>
	public readonly correlationId?: string
	public readonly problem?: ProblemDetails
	public readonly isNetworkError: boolean
	public readonly isTimeout: boolean

	public constructor(params: {
		message: string
		status?: number
		code?: string
		title?: string
		detail?: string
		fieldErrors?: Record<string, string[]>
		correlationId?: string
		problem?: ProblemDetails
		isNetworkError?: boolean
		isTimeout?: boolean
	}) {
		super(params.message)
		this.name = 'ApiError'
		this.status = params.status
		this.code = params.code
		this.title = params.title
		this.detail = params.detail
		this.fieldErrors = params.fieldErrors
		this.correlationId = params.correlationId
		this.problem = params.problem
		this.isNetworkError = params.isNetworkError ?? false
		this.isTimeout = params.isTimeout ?? false
	}
}

export function isAxiosTimeout(error: AxiosError): boolean {
	return error.code === 'ECONNABORTED'
}
