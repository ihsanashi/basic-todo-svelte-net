<script lang="ts">
	import '@app/app.css';
	import { onMount } from 'svelte';

	import { Toaster } from '$lib/components/ui/sonner';

	import { authActions } from '@api';
	import { authStore } from '@stores/auth';

	import type { GetApiAuthMeResponse } from '@references/codegen';

	onMount(async () => {
		const response = await authActions.checkAuth();

		if (response.success) {
			const user = response.data as GetApiAuthMeResponse;
			authStore.set({
				isAuthenticated: true,
				user: user,
			});
		} else {
			authStore.set({
				isAuthenticated: false,
				user: null,
			});
		}
	});
</script>

<Toaster />

<slot />
