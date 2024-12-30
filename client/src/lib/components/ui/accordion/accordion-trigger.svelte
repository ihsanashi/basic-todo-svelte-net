<script lang="ts">
	import { Accordion as AccordionPrimitive } from 'bits-ui';
	import ChevronDown from 'lucide-svelte/icons/chevron-down';
	import { cn } from '$lib/utils.js';

	type $$Props = AccordionPrimitive.TriggerProps;
	type $$Events = AccordionPrimitive.TriggerEvents;

	let className: $$Props['class'] = undefined;
	export let level: AccordionPrimitive.HeaderProps['level'] = 3;
	export { className as class };
</script>

<AccordionPrimitive.Header {level} class="flex">
	<div class="flex w-full flex-row items-center">
		<AccordionPrimitive.Trigger
			class={cn(
				'flex flex-1 items-center justify-between py-4 font-medium transition-all hover:underline [&[data-state=open]>svg]:rotate-180',
				className
			)}
			{...$$restProps}
			on:click
		>
			<ChevronDown class="transition-transform duration-200" size={16} />
		</AccordionPrimitive.Trigger>
		<slot name="title" />
		<slot name="actions" />
	</div>
</AccordionPrimitive.Header>
