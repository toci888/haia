export interface StudioAuthStatusResponse {
	accountId: string
	user: string
	email: string
	roles: string[]
	scopes: string[]
	status: string
}

export interface StudioCsrfTokenResponse {
	requestToken: string
}

export interface StudioLoginRequest {
	email: string
	password: string
}
