import type {
	GetApiAuthMeResponse,
	PostApiAuthForgotPasswordResponse,
	PostApiAuthLoginResponse,
	PostApiAuthRegisterResponse,
	UserDto,
} from '../codegen';

export interface AuthStore {
	isAuthenticated: boolean;
	user: UserDto | null;
}

export interface GenericPostAuthApiResponse {
	success: boolean;
	toast: {
		title: string;
		description: string;
	};
	data?:
		| PostApiAuthLoginResponse
		| PostApiAuthRegisterResponse
		| PostApiAuthForgotPasswordResponse
		| GetApiAuthMeResponse;
}
