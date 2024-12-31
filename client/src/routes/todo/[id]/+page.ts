import { error } from '@sveltejs/kit';
import type { PageLoad } from '../$types';

import { getSingleTodo } from '@api';

export const load: PageLoad = async ({ params }) => {
	// Wait for layout's load function to complete

	const id = parseInt(params.id, 10);

	if (isNaN(id)) {
		error(400, 'Invalid ID');
	}

	const response = await getSingleTodo(id);

	if (response.success) {
		return {
			data: response.data,
		};
	}

	error(404, 'Todo item not found');
};
