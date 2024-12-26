import type {
	PostApiAuthForgotPasswordResponse,
	PostApiAuthLoginResponse,
	PostApiAuthRegisterResponse,
} from '../codegen';

export interface User {
	email: string;
	isEmailConfirmed: boolean;
}

export interface AuthStore {
	isAuthenticated: boolean;
	user: User | null;
}

export interface GenericPostAuthApiResponse {
	success: boolean;
	toast: {
		title: string;
		description: string;
	};
	data?: PostApiAuthLoginResponse | PostApiAuthRegisterResponse | PostApiAuthForgotPasswordResponse;
}
