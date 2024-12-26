<script lang="ts">
	import { Eye, EyeOff } from 'lucide-svelte';
	import type { HTMLInputAttributes } from 'svelte/elements';
	import type { InputEvents } from './index.js';
	import { cn } from '$lib/utils.js';

	type $$Props = HTMLInputAttributes;
	type $$Events = InputEvents;

	let className: $$Props['class'] = undefined;
	export let value: $$Props['value'] = undefined;
	export let placeholder: $$Props['placeholder'] = undefined;
	export { className as class };

	// Workaround for https://github.com/sveltejs/svelte/issues/9305
	// Fixed in Svelte 5, but not backported to 4.x.
	export let readonly: $$Props['readonly'] = undefined;

	let showPassword = false;
	function togglePasswordVisibility() {
		showPassword = !showPassword;
	}
</script>

<div class="relative flex items-center">
	<input
		class={cn(
			'border-input placeholder:text-muted-foreground focus-visible:ring-ring flex h-9 w-full rounded-md border bg-transparent px-3 py-1 text-sm shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium focus-visible:outline-none focus-visible:ring-1 disabled:cursor-not-allowed disabled:opacity-50',
			className
		)}
		on:input={(e) => (value = e.currentTarget.value)}
		{value}
		{placeholder}
		{readonly}
		on:blur
		on:change
		on:click
		on:focus
		on:focusin
		on:focusout
		on:keydown
		on:keypress
		on:keyup
		on:mouseover
		on:mouseenter
		on:mouseleave
		on:mousemove
		on:paste
		on:input
		on:wheel|passive
		{...$$restProps}
		type={showPassword ? 'text' : 'password'}
	/>
	<button
		type="button"
		class="text-foreground hover:text-foreground/65 absolute right-2 cursor-pointer"
		on:click={togglePasswordVisibility}
	>
		{#if showPassword}
			<Eye size={16} />
		{:else}
			<EyeOff size={16} />
		{/if}
	</button>
</div>
