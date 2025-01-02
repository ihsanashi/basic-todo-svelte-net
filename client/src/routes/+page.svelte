<script lang="ts">
	import { onMount } from 'svelte';

	import { authActions, getAllTodos, bulkSaveTodos, deleteSingleTodo } from '@api';
	import { authStore } from '@stores/auth';

	import { Button } from '@ui/button';
	import { TodoItem } from '@components/todo-item';

	import { toast } from 'svelte-sonner';

	import type { GetApiAuthMeResponse, TodoItemDTO } from '@references/codegen';
	import { get, writable } from 'svelte/store';

	import { Plus } from 'lucide-svelte';

	let todoInputs = writable<TodoItemDTO[]>([]); // Store for new todo items
	let todos = writable<TodoItemDTO[]>([]);

	let saveTimer: NodeJS.Timeout | null = null; // Timer for background sync

	// Add a new todo item
	function addNewTodoItem() {
		todoInputs.update((current) => [
			...current,
			{ title: '', isComplete: false, description: '', dueDate: null },
		]);

		startBackgroundSync();
	}

	// Start the background sync timer
	function startBackgroundSync() {
		if (saveTimer) clearTimeout(saveTimer); // Reset timer if it exists

		saveTimer = setTimeout(async () => {
			await saveTodos();
		}, 60000); // 1 minute (60,000ms)
	}

	// Save todos to the backend
	async function saveTodos() {
		const newTodos = get(todoInputs); // Get new todos
		if (newTodos.length > 0) {
			const response = await bulkSaveTodos(newTodos); // Call backend
			if (response.success) {
				todoInputs.set([]); // Clear inputs after successful save
				fetchTodos(); // Refresh todos from backend
			} else {
				console.error('Failed to save todos', response);
			}
		}
	}

	async function fetchTodos() {
		const response = await getAllTodos();

		if (response.success) {
			todos.set(response.data || []);
		}
	}

	async function handleDelete(todo: TodoItemDTO, type: boolean = false) {
		if (!todo.id) {
			$todoInputs = $todoInputs.filter((item) => item !== todo);
			return;
		}

		const response = await deleteSingleTodo(todo.id, type);

		if (response.success) {
			toast.success(`${type ? 'Permanently deleted' : 'Archived'} a todo item`);
			await fetchTodos();
		} else {
			toast.error('Failed to delete a todo item', {
				description: response.errorMessage || undefined,
			});
		}
	}

	onMount(async () => {
		const response = await authActions.getCurrentUser();

		if (response.success) {
			const user = response.data as GetApiAuthMeResponse;

			authStore.set({
				isAuthenticated: true,
				user: user,
			});

			await fetchTodos();
		} else {
			authStore.set({
				isAuthenticated: false,
				user: null,
			});
		}

		if (!$authStore?.isAuthenticated) {
			window.location.href = '/auth';
		}
	});
</script>

<section class="p-4">
	{#if $authStore?.isAuthenticated}
		<!-- Render newly added todos -->
		{#each $todoInputs as todo, index}
			<div class="my-2">
				<TodoItem bind:todo={$todoInputs[index]} onDelete={handleDelete} />
			</div>
		{/each}

		<div class="p-4">
			<Button
				class="p-0 text-slate-500 hover:bg-transparent hover:text-slate-900"
				variant="ghost"
				on:click={addNewTodoItem}
			>
				<Plus class="mr-1" size={16} />
				Add new
			</Button>
		</div>

		<div>
			<hr class="border-slate-200" />
		</div>

		<!-- render fetched todos -->
		<div class="p-4">
			{#if $todos === null || $todos.length === 0}
				<div class="px-4 py-2">
					<p class="text-sm text-slate-500">No todos yet.</p>
				</div>
			{:else}
				<ul>
					{#each $todos as todo}
						<TodoItem bind:todo onDelete={handleDelete} />
					{/each}
				</ul>
			{/if}
		</div>
	{:else}
		<div class="p-4">
			<p class="text-slate-500">Loading...</p>
		</div>
	{/if}
</section>
