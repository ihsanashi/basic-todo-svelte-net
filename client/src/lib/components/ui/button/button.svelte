<script lang="ts">
	import { Button as ButtonPrimitive } from 'bits-ui';
	import { type Events, type Props, buttonVariants } from './index.js';
	import { cn } from '$lib/utils.js';

	type $$Props = Props;
	type $$Events = Events;

	let className: $$Props['class'] = undefined;
	export let variant: $$Props['variant'] = 'default';
	export let size: $$Props['size'] = 'default';
	export let builders: $$Props['builders'] = [];
	export let loading: boolean = false;
	export let loadingText: $$Props['bind:textContent'] = undefined;
	export { className as class };
</script>

<ButtonPrimitive.Root
	{builders}
	class={cn(
		buttonVariants({ variant, size, className }),
		loading && 'cursor-not-allowed opacity-50'
	)}
	disabled={loading}
	type="button"
	{...$$restProps}
	on:click
	on:keydown
>
	{#if loading}
		<span class="flex items-center space-x-2">
			<svg
				class="h-4 w-4 animate-spin text-current"
				xmlns="http://www.w3.org/2000/svg"
				fill="none"
				viewBox="0 0 24 24"
			>
				<circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"
				></circle>
				<path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z"></path>
			</svg>
			<span>{loadingText}</span>
		</span>
	{:else}
		<slot />
	{/if}
</ButtonPrimitive.Root>
