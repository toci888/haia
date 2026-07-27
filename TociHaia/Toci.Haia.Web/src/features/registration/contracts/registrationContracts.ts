// HTTP contract mirrored from HAIA API v1; verify after backend contract changes.

export interface CurrentLegalDocumentItemDto {
	documentVersionId: string
	documentKey: string
	version: string
	requiredAction: string
	effectiveFrom: string
	effectiveTo: string | null
	language: string
}

export interface CurrentLegalDocumentsResponseDto {
	documents: CurrentLegalDocumentItemDto[]
}

export interface NicknameAvailabilityResponseDto {
	available: boolean
	normalizedNickname: string
}

export interface LegalDecisionRequestDto {
	documentVersionId: string
	action: string
}

export interface RegisterEmailRequestDto {
	email: string
	password: string
	nickname: string
	language: string | null
	legalDecisions: LegalDecisionRequestDto[] | null
}

export interface AcceptedResponseDto {
	status: string
}

export interface VerifyEmailRequestDto {
	token: string
}

export interface ResendEmailRequestDto {
	email: string
	language: string | null
}

export interface FieldValidationErrors {
	[key: string]: string[]
}
