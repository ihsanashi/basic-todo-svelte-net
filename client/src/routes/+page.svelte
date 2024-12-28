<script lang="ts">
	import { onMount } from 'svelte';

	import { authActions, getAllTodos } from '@api';
	import { authStore } from '@stores/auth';
	import { buttonVariants } from '@ui/button';

	import type { GetApiAuthMeResponse, TodoItemDTO } from '@references/codegen';
	import { writable } from 'svelte/store';

	let todos = writable<TodoItemDTO[]>([]);
	onMount(async () => {
		const response = await authActions.checkAuth();

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

	async function fetchTodos() {
		const response = await getAllTodos();

		if (response.success) {
			todos.set(response.data || []);
		}
	}
</script>

{#if $authStore?.isAuthenticated}
	<section class="p-4">
		{#if $todos.length === 0}
			<div class="text-center text-sm">
				<p class="text-slate-500">No todos yet.</p>
				<a class={`${buttonVariants({ variant: 'link' })}`} href="/new">Add new?</a>
			</div>
		{:else}
			<ul>
				{#each $todos as todo}
					<li>{todo.title}</li>
				{/each}
			</ul>
		{/if}
	</section>
{/if}
