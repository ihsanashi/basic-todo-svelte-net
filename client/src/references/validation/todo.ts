import { z } from 'zod';
import type { TodoItemDTO } from '../codegen';

export const todoSchema: z.ZodType<TodoItemDTO> = z.object({
	id: z.number().optional(),
	title: z.string().nonempty('Title is required'),
	description: z.string().optional().nullable().default(''), // Allow empty string as a valid value	isComplete: z.boolean().optional(),
	dueDate: z.string().nullable().optional(),
	createdAt: z.string().optional(),
	updatedAt: z.string().optional(),
});

export type TodoSchema = z.infer<typeof todoSchema>;

export function validateTodoItem(todo: TodoItemDTO): boolean {
	try {
		todoSchema.parse(todo);
		return true;
	} catch (error) {
		console.error(`Todo item failed validation check: ${error}`);
		if (error instanceof z.ZodError) {
			console.error(`ZodError: ${error.errors}`);
		}
		return false;
	}
}
