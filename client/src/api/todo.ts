import { API_TODO_URL } from '@references/constants';

import type { GetTodoItemsResponse } from '@references/codegen';

export async function getAllTodos(): Promise<GetTodoItemsResponse> {
	try {
		const response = await fetch(`${API_TODO_URL}`, {
			credentials: 'include',
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
			},
		});

		if (response.ok) {
			const data: GetTodoItemsResponse = await response.json();

			return {
				success: true,
				data: data.data,
				errorMessage: null,
			};
		} else {
			const data: GetTodoItemsResponse = await response.json();

			return {
				success: false,
				data: null,
				errorMessage: data.errorMessage,
			};
		}
	} catch (error) {
		console.error(error);
		const errorMessage = error instanceof Error ? error.message : 'Unknown error';

		return {
			success: false,
			data: null,
			errorMessage: errorMessage,
		};
	}
}
