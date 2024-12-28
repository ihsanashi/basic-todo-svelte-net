<script lang="ts">
	import { authActions } from '@api/auth';
	import { authStore, setAuthState } from '@stores/auth';

	import { goto } from '$app/navigation';
	import { onMount } from 'svelte';
	import { writable } from 'svelte/store';

	import { toast } from 'svelte-sonner';

	import * as Tabs from '@ui/tabs';
	import * as Card from '@ui/card';
	import { Button } from '@ui/button';
	import { Input } from '@ui/input';
	import { Label } from '@ui/label';
	import PasswordInput from '@ui/password-input/password-input.svelte';

	import { forgotPasswordSchema, loginSchema, registrationSchema } from '@references/validation';

	const loadingText = 'Loading...';

	let email = $state('');
	let password = $state('');
	let confirmPassword = $state('');
	let isLoading = writable(false);

	// Store for error messages
	let errors = writable({
		email: '',
		password: '',
		confirmPassword: '',
	});

	let activeTab = $state('register'); // Default tab

	onMount(() => {
		const unsubscribe = authStore.subscribe((authState) => {
			if (authState?.isAuthenticated) {
				goto('/');
			}
		});

		// Cleanup subscription on component unmount
		return () => unsubscribe();
	});

	// Update the tab based on the URL fragment and set the initial hash if not set
	onMount(() => {
		const updateTabFromHash = () => {
			const hash = window.location.hash.replace('#', '');
			if (['login', 'register', 'forgot-password'].includes(hash)) {
				activeTab = hash;
			} else {
				// If the hash is not set, update the URL with the default active tab
				window.location.hash = activeTab;
			}
		};

		window.addEventListener('hashchange', updateTabFromHash);

		// Initial hash check
		updateTabFromHash();

		return () => {
			window.removeEventListener('hashchange', updateTabFromHash);
		};
	});

	// Update the URL hash whenever the active tab changes
	function handleTabChange(value: string | undefined) {
		// Handle the tab change logic
		if (value) {
			activeTab = value; // Sync with local variable
			window.location.hash = value; // Update URL fragment
		}
	}

	async function handleRegisterSubmit() {
		const inputs = {
			email,
			password,
			confirmPassword,
		};

		const validation = registrationSchema.safeParse(inputs);

		if (!validation.success) {
			errors.set({
				email: validation.error.formErrors.fieldErrors.email?.[0] || '',
				password: validation.error.formErrors.fieldErrors.password?.[0] || '',
				confirmPassword: validation.error.formErrors.fieldErrors.confirmPassword?.[0] || '',
			});
			return;
		}

		isLoading.set(true);

		const response = await authActions.register(inputs);

		if (response.success) {
			toast.success(response.toast.title, {
				description: response.toast.description,
			});
			// Redirect to #login after 2 seconds
		} else {
			toast.error(response.toast.title, {
				description: response.toast.description,
			});
		}

		isLoading.set(false);
	}

	async function handleLoginSubmit() {
		const inputs = {
			email,
			password,
		};

		const validation = loginSchema.safeParse(inputs);

		if (!validation.success) {
			errors.set({
				email: validation.error.formErrors.fieldErrors.email?.[0] || '',
				password: validation.error.formErrors.fieldErrors.password?.[0] || '',
				confirmPassword: '',
			});
			return;
		}

		isLoading.set(true);

		const response = await authActions.login(inputs);

		if (response.success) {
			toast.success(response.toast.title, {
				description: response.toast.description,
			});

			setAuthState({
				isAuthenticated: true,
				user: null,
			});
		} else {
			toast.error(response.toast.title, {
				description: response.toast.description,
			});
		}

		isLoading.set(false);
	}

	async function handleForgotPasswordSubmit() {
		const inputs = {
			email,
		};

		const validation = forgotPasswordSchema.safeParse(inputs);

		if (!validation.success) {
			errors.set({
				email: validation.error.formErrors.fieldErrors.email?.[0] || '',
				password: '',
				confirmPassword: '',
			});
			return;
		}

		isLoading.set(true);

		const response = await authActions.forgotPassword(inputs);

		if (response.success) {
			toast.success(response.toast.title, {
				description: response.toast.description,
			});
		} else {
			toast.error(response.toast.title, {
				description: response.toast.description,
			});
		}

		isLoading.set(false);
	}
</script>

<main class="flex h-screen flex-col p-6">
	<section class="m-auto">
		<div>
			<h1 class="text-3xl text-slate-900">Authentication</h1>
			<p class="text-slate-700">Please create an account or login to proceed.</p>
		</div>

		<div class="py-4">
			<Tabs.Root bind:value={activeTab} class="w-[450px]" onValueChange={handleTabChange}>
				<Tabs.List class="grid w-full grid-cols-3">
					<Tabs.Trigger value="register">Register</Tabs.Trigger>
					<Tabs.Trigger value="login">Log in</Tabs.Trigger>
					<Tabs.Trigger value="forgot-password">Forgot password</Tabs.Trigger>
				</Tabs.List>

				<Tabs.Content value="register">
					<Card.Root>
						<Card.Header>
							<Card.Title>Sign up</Card.Title>
							<Card.Description>
								Create an account and start filling in your todo list.
							</Card.Description>
						</Card.Header>
						<Card.Content class="space-y-2">
							<div class="space-y-1">
								<Label inputmode="email" for="email">Email</Label>
								<Input bind:value={email} id="email" placeholder="johndoe@email.com" />
								{#if $errors.email}
									<p class="text-xs text-red-500">{$errors.email}</p>
								{/if}
							</div>
							<div class="space-y-1">
								<Label for="password">Password</Label>
								<PasswordInput
									id="password"
									minlength={8}
									placeholder="Enter your password"
									type="password"
									bind:value={password}
								/>
								{#if $errors.password}
									<p class="text-xs text-red-500">{$errors.password}</p>
								{/if}
							</div>
							<div class="space-y-1">
								<Label for="confirmPassword">Confirm password</Label>
								<PasswordInput
									id="confirmPassword"
									minlength={8}
									placeholder="Enter your password again"
									type="password"
									bind:value={confirmPassword}
								/>
								{#if $errors.confirmPassword}
									<p class="text-xs text-red-500">{$errors.confirmPassword}</p>
								{/if}
							</div>
						</Card.Content>
						<Card.Footer>
							<Button loading={$isLoading} {loadingText} on:click={handleRegisterSubmit}
								>Submit</Button
							>
						</Card.Footer>
					</Card.Root>
				</Tabs.Content>

				<Tabs.Content value="login">
					<Card.Root>
						<Card.Header>
							<Card.Title>Log in</Card.Title>
							<Card.Description>Enter your credentials to access your todo list.</Card.Description>
						</Card.Header>
						<Card.Content class="space-y-2">
							<div class="space-y-1">
								<Label inputmode="email" for="email">Email</Label>
								<Input id="email" placeholder="johndoe@email.com" bind:value={email} />
								{#if $errors.email}
									<p class="text-xs text-red-500">{$errors.email}</p>
								{/if}
							</div>
							<div class="space-y-1">
								<Label for="password">Password</Label>
								<PasswordInput
									id="password"
									minlength={8}
									placeholder="Enter your password"
									type="password"
									bind:value={password}
								/>
								{#if $errors.password}
									<p class="text-xs text-red-500">{$errors.password}</p>
								{/if}
							</div>
						</Card.Content>
						<Card.Footer>
							<Button loading={$isLoading} {loadingText} on:click={handleLoginSubmit}>Submit</Button
							>
						</Card.Footer>
					</Card.Root>
				</Tabs.Content>

				<Tabs.Content value="forgot-password">
					<Card.Root>
						<Card.Header>
							<Card.Title>Forgot password</Card.Title>
							<Card.Description>We'll send you an email to reset your password.</Card.Description>
						</Card.Header>
						<Card.Content class="space-y-2">
							<div class="space-y-1">
								<Label inputmode="email" for="email">Email</Label>
								<Input id="email" placeholder="johndoe@email.com" bind:value={email} />
								{#if $errors.email}
									<p class="text-xs text-red-500">{$errors.email}</p>
								{/if}
							</div>
						</Card.Content>
						<Card.Footer>
							<Button loading={$isLoading} {loadingText} on:click={handleForgotPasswordSubmit}
								>Submit</Button
							>
						</Card.Footer>
					</Card.Root>
				</Tabs.Content>
			</Tabs.Root>
		</div>
	</section>
</main>
