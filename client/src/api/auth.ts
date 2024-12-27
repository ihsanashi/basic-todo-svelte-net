import {
	API_AUTH_URL,
	toastFailureNuts,
	toastFailureUnexpected,
	toastSuccessTitle,
} from '@references/constants';

import type {
	ForgotPasswordRequest,
	LoginRequest,
	PostApiAuthForgotPasswordError,
	PostApiAuthRegisterError,
	RegisterRequest,
	UserDto,
} from '@references/codegen';

import type { GenericPostAuthApiResponse } from '@references/types';

async function login(inputs: LoginRequest): Promise<GenericPostAuthApiResponse> {
	try {
		const response = await fetch(`${API_AUTH_URL}/login?useCookies=true`, {
			credentials: 'include',
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(inputs),
		});

		if (response.ok) {
			return {
				success: true,
				toast: {
					title: toastSuccessTitle,
					description: 'Redirecting you shortly...',
				},
			};
		} else {
			return {
				success: false,
				toast: {
					title: toastFailureNuts,
					description: 'Login failed. Sorry about that!',
				},
			};
		}
	} catch (error: unknown) {
		console.error(error);
		const errorMessage = error instanceof Error ? error.message : 'Unknown error';

		return {
			success: false,
			toast: {
				title: toastFailureUnexpected,
				description: errorMessage,
			},
		};
	}
}

async function logout(): Promise<GenericPostAuthApiResponse> {
	try {
		const response = await fetch(`${API_AUTH_URL}/logout`, {
			credentials: 'include',
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify({}),
		});

		if (response.ok) {
			return {
				success: true,
				toast: {
					title: toastSuccessTitle,
					description: 'You will be logged out shortly.',
				},
			};
		} else {
			const errorData: PostApiAuthForgotPasswordError = await response.json();
			return {
				success: false,
				toast: {
					title: toastFailureNuts,
					description: errorData.title || errorData.detail || 'Logout failed.',
				},
			};
		}
	} catch (error: unknown) {
		console.error(error);
		const errorMessage =
			error instanceof Error ? error.message : 'An unknown error occurred during logout';

		return {
			success: false,
			toast: {
				title: toastFailureUnexpected,
				description: errorMessage,
			},
		};
	}
}

async function register(inputs: RegisterRequest): Promise<GenericPostAuthApiResponse> {
	try {
		const response = await fetch(`${API_AUTH_URL}/register`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(inputs),
		});

		if (response.ok) {
			return {
				success: true,
				toast: {
					title: toastSuccessTitle,
					description: 'Your registration was successful. Please login.',
				},
			};
		} else {
			const errorData: PostApiAuthRegisterError = await response.json();
			return {
				success: false,
				toast: {
					title: toastFailureNuts,
					description: errorData.title || errorData.detail || 'Registration failed.',
				},
			};
		}
	} catch (error: unknown) {
		console.error(error);
		const errorMessage =
			error instanceof Error ? error.message : 'An unknown error occurred during registration';

		return {
			success: false,
			toast: {
				title: toastFailureUnexpected,
				description: errorMessage,
			},
		};
	}
}

async function forgotPassword(inputs: ForgotPasswordRequest): Promise<GenericPostAuthApiResponse> {
	try {
		const response = await fetch(`${API_AUTH_URL}/forgotPassword`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(inputs),
		});

		if (response.ok) {
			return {
				success: true,
				toast: {
					title: toastSuccessTitle,
					description:
						'Forgot password procedure initiated. Please check your email for the link to reset your password.',
				},
			};
		} else {
			const errorData: PostApiAuthForgotPasswordError = await response.json();
			return {
				success: false,
				toast: {
					title: toastFailureNuts,
					description:
						errorData.title || errorData.detail || 'Failed to trigger password reset request.',
				},
			};
		}
	} catch (error: unknown) {
		console.error(error);
		const errorMessage =
			error instanceof Error
				? error.message
				: 'An unknown error occurred during forgot password process';

		return {
			success: false,
			toast: {
				title: toastFailureUnexpected,
				description: errorMessage,
			},
		};
	}
}

async function checkAuth(): Promise<GenericPostAuthApiResponse> {
	try {
		const response = await fetch(`${API_AUTH_URL}/me`, {
			credentials: 'include',
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
			},
		});

		if (response.ok) {
			const user: UserDto = await response.json();
			return {
				data: user,
				success: true,
				toast: {
					title: toastSuccessTitle,
					description: 'User details loaded',
				},
			};
		} else {
			// TODO: Get error response
			return {
				success: false,
				toast: {
					title: toastFailureNuts,
					description: 'Failed to fetch user details', // TODO: Update description
				},
			};
		}
	} catch (error) {
		console.error('Error checking authentication: ', error);
		const errorMessage =
			error instanceof Error
				? error.message
				: 'An unknown error occurred during forgot password process';
		return {
			success: false,
			toast: {
				title: toastFailureUnexpected,
				description: errorMessage,
			},
		};
	}
}

export const authActions = {
	login,
	logout,
	register,
	forgotPassword,
	checkAuth,
};
