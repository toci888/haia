// HTTP contract mirrored from HAIA Studio API v1; verify after backend contract changes.
export interface SystemInfoResponse {
	serviceName: string
	apiVersion: string
	status: string
}

// HTTP contract mirrored from HAIA Studio API v1; verify after backend contract changes.
export interface HealthStatusResponse {
	status?: string
}

export type HealthIndicator = 'checking' | 'available' | 'unavailable'
