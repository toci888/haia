export type ValidationErrorMap = Record<string, string[]>

// HTTP contract mirrored from HAIA Studio API v1; verify after backend contract changes.
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
