import type { TodoItemDTO } from '../codegen';

export * from './messages';

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;
export const API_AUTH_URL = `${API_BASE_URL}/auth`;
export const API_TODO_URL = `${API_BASE_URL}/TodoItems`;

export const commonDates = [
	{ value: 0, label: 'Today' },
	{ value: 1, label: 'Tomorrow' },
	{ value: 3, label: 'In 3 days' },
	{ value: 7, label: 'In a week' },
	{ value: 14, label: 'In 2 weeks' },
	{ value: 30, label: 'In a month' },
];

export const defaultTodo: TodoItemDTO = {
	id: undefined,
	title: null,
	description: null,
	createdAt: undefined,
	updatedAt: undefined,
	isComplete: false,
};
