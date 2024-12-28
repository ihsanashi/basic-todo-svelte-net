<script>
	import { goto } from '$app/navigation';
	import { writable } from 'svelte/store';

	import { authStore } from '@stores/auth';

	import { toast } from 'svelte-sonner';

	import { Button } from '@ui/button';
	import * as Card from '@ui/card';
	import { Input } from '@components/ui/input';
	import { Label } from '@components/ui/label';

	import { authActions } from '@api';
	import { setAuthState } from '@app/stores';

	let isLoading = writable(false);

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

<section class="flex flex-col p-4">
	<div class="p-4">
		<h2 class="text-2xl text-slate-800">My profile</h2>
	</div>

	<div class="p-4">
		<Card.Root class="w-[450px]">
			<Card.Header>
				<Card.Title>Details</Card.Title>
				<Card.Description>Non-editable for now.</Card.Description>
			</Card.Header>
			<Card.Content class="space-y-2">
				<div class="space-y-1">
					<Label for="email">Email</Label>
					<Input
						disabled
						type="email"
						id="email"
						placeholder="email"
						value={$authStore?.user?.email}
					/>
				</div>

				<div class="space-y-1">
					<Label for="phone">Phone number</Label>
					<Input
						disabled
						type="tel"
						id="phone"
						placeholder="Phone number"
						value={$authStore?.user?.phoneNumber}
					/>
				</div>
			</Card.Content>
		</Card.Root>
	</div>

	<div class="p-4">
		<Button
			loading={$isLoading}
			loadingText="Loading..."
			on:click={handleLogout}
			variant="destructive"
		>
			Logout
		</Button>
	</div>
</section>
