<script lang="ts">
	import { Button } from '@ui/button';
	import { Checkbox } from '@ui/checkbox';
	import { Input } from '@ui/input';

	let checked = false;
</script>

<section class="flex flex-col p-4">
	<div class="p-4">
		<div class="flex items-center space-x-2">
			<Checkbox id="terms" bind:checked aria-labelledby="terms-label" />
			<Input class="border-none" placeholder="Walk the dog" type="text"></Input>
		</div>
	</div>

	<!-- TODO: add text area for description -->
	<!-- TODO: add date picker for due date -->

	<div class="p-4">
		<Button>Save</Button>
	</div>
</section>
