import { writable } from 'svelte/store';
import type { AuthStore } from '@references/types';
import type { UserDto } from '@references/codegen';

export const authStore = writable<AuthStore | null>(null);

export const setAuthState = (authState: { isAuthenticated: boolean; user: UserDto | null }) => {
	authStore.set({
		isAuthenticated: authState.isAuthenticated,
		user: authState.user,
	});
};
