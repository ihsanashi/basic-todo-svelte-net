import { API_TODO_URL } from '@references/constants';

import type {
	TodoItemsMultipleResponse,
	TodoItemDTO,
	TodoItemResponse,
	TodoItemDeletionResponse,
} from '@references/codegen';

export async function getAllTodos(): Promise<TodoItemsMultipleResponse> {
	try {
		const response = await fetch(`${API_TODO_URL}`, {
			credentials: 'include',
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
			},
		});

		if (response.ok) {
			const json: TodoItemsMultipleResponse = await response.json();

			return {
				success: true,
				data: json.data,
				errorMessage: null,
			};
		} else {
			const data: TodoItemsMultipleResponse = await response.json();

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

export async function getSingleTodo(id: number): Promise<TodoItemResponse> {
	try {
		const response = await fetch(`${API_TODO_URL}/${id}`, {
			credentials: 'include',
			method: 'GET',
			headers: {
				'Content-Type': 'application/json',
			},
		});

		if (response.ok) {
			const json: TodoItemResponse = await response.json();
			return {
				success: true,
				data: json.data,
			};
		} else {
			const json: TodoItemResponse = await response.json();
			return {
				success: false,
				errorMessage: json.errorMessage,
			};
		}
	} catch (error) {
		console.error(error);
		const errorMessage = error instanceof Error ? error.message : 'Unknown error';

		return {
			success: false,
			errorMessage: errorMessage,
		};
	}
}

export async function bulkSaveTodos(todos: TodoItemDTO[]): Promise<TodoItemsMultipleResponse> {
	try {
		const response = await fetch(`${API_TODO_URL}/bulk`, {
			credentials: 'include',
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(todos),
		});

		if (response.ok) {
			const json: TodoItemsMultipleResponse = await response.json();

			return { success: true, data: json.data, errorMessage: null };
		} else {
			const errorData: TodoItemsMultipleResponse = await response.json();
			return { success: false, data: null, errorMessage: errorData.errorMessage };
		}
	} catch (error) {
		console.error(error);
		const errorMessage = error instanceof Error ? error.message : 'Unknown error';
		return { success: false, data: null, errorMessage: errorMessage };
	}
}

export async function postSingleTodo(todo: TodoItemDTO): Promise<TodoItemResponse> {
	try {
		const response = await fetch(`${API_TODO_URL}`, {
			credentials: 'include',
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(todo),
		});

		if (response.ok) {
			const json: TodoItemResponse = await response.json();

			return {
				success: true,
				data: json.data,
				errorMessage: null,
			};
		} else {
			const errorData: TodoItemResponse = await response.json();

			return {
				success: false,
				data: undefined,
				errorMessage: errorData.errorMessage,
			};
		}
	} catch (error) {
		console.error(error);
		const errorMessage = error instanceof Error ? error.message : 'Unknown error';

		return {
			success: false,
			data: undefined,
			errorMessage: errorMessage,
		};
	}
}

export async function deleteSingleTodo(
	id: number,
	permanentlyDelete: boolean
): Promise<TodoItemDeletionResponse> {
	try {
		const response = await fetch(`${API_TODO_URL}/${id}`, {
			credentials: 'include',
			method: 'DELETE',
			headers: {
				'Content-Type': 'application/json',
			},
			body: JSON.stringify(permanentlyDelete),
		});

		if (response.ok) {
			return {
				success: true,
			};
		} else {
			const json: TodoItemDeletionResponse = await response.json();
			return {
				success: false,
				errorMessage: json.errorMessage,
			};
		}
	} catch (error) {
		console.error(error);
		const errorMessage = error instanceof Error ? error.message : 'Unknown error';

		return {
			success: false,
			errorMessage: errorMessage,
		};
	}
}
