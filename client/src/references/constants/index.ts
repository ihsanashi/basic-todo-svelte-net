export * from './messages';

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
export const API_AUTH_URL = `${API_BASE_URL}/auth`;
export const API_TODO_URL = `${API_BASE_URL}/TodoItems`;
