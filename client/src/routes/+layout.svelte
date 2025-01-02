<script lang="ts">
	import '@app/app.css';
	import { onMount } from 'svelte';

	import { LayoutContainer } from '@components/layout-container';
	import { Navbar } from '@components/navbar';
	import { Toaster } from '$lib/components/ui/sonner';

	import { authActions } from '@api';
	import { authStore } from '@stores/auth';

	import type { GetApiAuthMeResponse } from '@references/codegen';

	onMount(async () => {
		const response = await authActions.getCurrentUser();

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

<LayoutContainer>
	<Navbar />

	<Toaster closeButton expand position="bottom-right" />

	<slot />
</LayoutContainer>
