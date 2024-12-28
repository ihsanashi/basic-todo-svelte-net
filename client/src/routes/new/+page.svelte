<script lang="ts">
	import { TodoItemContent, TodoItemTitle } from '@app/lib/components/todo-item';
	import { Button } from '@ui/button';
</script>

<section class="flex flex-col p-4">
	<div class="p-4">
		<TodoItemTitle />
	</div>

	<div class="p-4">
		<TodoItemContent />
	</div>

	<div class="p-4">
		<Button>Save</Button>
	</div>
</section>
