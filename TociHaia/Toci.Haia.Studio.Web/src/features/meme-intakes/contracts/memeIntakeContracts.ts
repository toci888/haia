export type CreateMemeIntakeRequest = {
	fileName: string
	contentType: string
	sizeBytes: number
	workingTitle?: string
}

export type CreateMemeIntakeResponse = {
	intakeId: string
	candidateId: string
	candidateVersionId: string
	assetId: string
	objectKey: string
	uploadUrl: string
	uploadHeaders: Record<string, string>
	uploadUrlExpiresAtUtc: string
	status: string
}

export type FinalizeMemeIntakeResponse = {
	intakeId: string
	status: string
	contentType: string
	sizeBytes: number
	sha256Hash: string
}

export type MemeClassification = {
	axisKey: string
	axisDisplayName: string
	valueKey: string
	valueDisplayName: string
	relevanceScore: number
	confidence: number
	isPrimary: boolean
	rankNo?: number | null
}

export type MemeMeasure = {
	axisKey: string
	axisDisplayName: string
	normalizedValue: number
	confidence: number
}

export type MemeSafety = {
	categoryKey: string
	categoryDisplayName: string
	severityLevel: number
	confidence: number
	moderationRelevance: boolean
}

export type MemeReaction = {
	text: string
	displayOrder: number
	initiallyVisible: boolean
	relevanceScore: number
	confidence: number
	secondPunchline: string
	mechanismKeys: string[]
}

export type MemeEvaluationResult = {
	suggestedTitle: string
	visualDescription: string
	detectedText?: string | null
	languageCode: string
	contentFormat: string
	suggestedRoleInFlow: string
	suggestedRoleConfidence: number
	editorialSummary?: string | null
	overallConfidence: number
	classifications: MemeClassification[]
	measures: MemeMeasure[]
	safety: MemeSafety[]
	moderationRecommendation: string
	moderationSummary?: string | null
	predictedDrynessLevel: number
	predictedDrynessLabel: string
	predictedDrynessConfidence: number
	reactions: MemeReaction[]
}

export type EvaluateMemeIntakeResponse = {
	intakeId: string
	status: string
	evaluation: MemeEvaluationResult
}

export type StartEditorialReviewResponse = {
	intakeId: string
	status: string
	editorialStatus: string
	classificationStatus: string
	candidateClassificationId?: string | null
}

export type MemeEditorialDecisionRequest = {
	summary?: string | null
	editorialNote?: string | null
}

export type MemeEditorialDecisionResponse = {
	intakeId: string
	status: string
	editorialStatus: string
	classificationStatus: string
	reviewStatus: string
	candidateClassificationId: string
	reviewedAtUtc: string
	reviewedByAccountId: string
}

export type GetMemeIntakeResponse = {
	intakeId: string
	status: string
	workingTitle?: string | null
	asset: {
		fileName: string
		contentType: string
		sizeBytes: number
		objectKey: string
	}
	evaluation?: MemeEvaluationResult | null
}
