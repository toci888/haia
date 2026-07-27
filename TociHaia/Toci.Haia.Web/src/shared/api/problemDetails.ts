export type ValidationErrorMap = Record<string, string[]>

export interface ProblemDetails {
	type?: string
	title?: string
	status?: number
	detail?: string
	instance?: string
	code?: string
	errors?: ValidationErrorMap
}

export function isProblemDetails(value: unknown): value is ProblemDetails {
	if (!value || typeof value !== 'object') {
		return false
	}

	const candidate = value as Record<string, unknown>
	return (
		typeof candidate.title === 'string' ||
		typeof candidate.detail === 'string' ||
		typeof candidate.status === 'number' ||
		typeof candidate.code === 'string' ||
		typeof candidate.errors === 'object'
	)
}
