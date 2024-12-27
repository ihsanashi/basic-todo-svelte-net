<script lang="ts">
	import { onMount } from 'svelte';

	import { authActions, getAllTodos } from '@api';
	import { authStore, setAuthState } from '@stores/auth';
	import { Button } from '@ui/button';

	import type { GetApiAuthMeResponse, TodoItemDTO } from '@references/codegen';
	import { writable } from 'svelte/store';
	import { toast } from 'svelte-sonner';
	import { goto } from '$app/navigation';

	let isLoading = writable(false);
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

	async function handleLogout() {
		isLoading.set(true);

		const response = await authActions.logout();

		if (response.success) {
			toast.success(response.toast.title, {
				description: response.toast.description,
			});

			setAuthState({
				isAuthenticated: false,
				user: null,
			});

			goto('/auth#login');
		} else {
			toast.error(response.toast.title, {
				description: response.toast.description,
			});
		}

		isLoading.set(false);
	}
</script>

{#if $authStore?.isAuthenticated}
	<main class="container mx-auto">
		<section class="p-8">
			<h1>Welcome back, {$authStore.user?.email}!</h1>
			<Button loading={$isLoading} loadingText="Loading..." on:click={handleLogout}>Logout</Button>
			<p>List of my todos</p>
			{#if $todos.length === 0}
				<p>No todos yet.</p>
			{:else}
				<ul>
					{#each $todos as todo}
						<li>{todo.title}</li>
					{/each}
				</ul>
			{/if}
		</section>
	</main>
{/if}
