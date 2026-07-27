import { z } from 'zod'

export const nicknameSchema = z
	.string()
	.trim()
	.min(3, 'Pseudonim musi mieć co najmniej 3 znaki.')
	.max(40, 'Pseudonim może mieć maksymalnie 40 znaków.')

export const emailSchema = z
	.string()
	.trim()
	.min(1, 'Adres e-mail jest wymagany.')
	.email('Podaj poprawny adres e-mail.')

export const passwordSchema = z
	.string()
	.min(12, 'Hasło musi mieć co najmniej 12 znaków.')
	.max(128, 'Hasło może mieć maksymalnie 128 znaków.')

export const registrationSchema = z.object({
	nickname: nicknameSchema,
	email: emailSchema,
	password: passwordSchema,
	language: z.string().min(1),
	legalDecisions: z.record(z.string(), z.boolean()),
})

export type RegistrationFormValues = z.infer<typeof registrationSchema>
