import { z } from 'zod';

export const emailSchema = z.string().nonempty('Email is required').email('Invalid email format');

export const passwordSchema = z
	.string()
	.min(6, 'Password must be at least 6 characters')
	.regex(/[A-Z]/, 'Password must contain an uppercase letter')
	.regex(/[a-z]/, 'Password must contain a lowercase letter')
	.regex(/\d/, 'Password must contain a digit')
	.regex(/[^a-zA-Z\d]/, 'Password must contain a special character');

export const registrationSchema = z
	.object({
		email: emailSchema,
		password: passwordSchema,
		confirmPassword: passwordSchema,
	})
	.refine((data) => data.password === data.confirmPassword, {
		message: "Passwords don't match",
		path: ['confirmPassword'],
	});

export const loginSchema = z.object({
	email: emailSchema,
	password: passwordSchema,
});

export const forgotPasswordSchema = z.object({
	email: emailSchema,
});
