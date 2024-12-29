<script lang="ts">
	import * as Accordion from '@ui/accordion';
	import * as AlertDialog from '@ui/alert-dialog';
	import * as Popover from '@ui/popover';
	import * as Select from '@ui/select/';

	import { Button } from '@ui/button';
	import { Calendar } from '@ui/calendar';
	import { Checkbox } from '@ui/checkbox';
	import { Input } from '@ui/input';
	import { Label } from '@ui/label';
	import { Textarea } from '@ui/textarea';

	import { CalendarIcon } from 'lucide-svelte';

	import { cn } from '@app/lib/utils';

	import { DateFormatter, type DateValue, getLocalTimeZone, today } from '@internationalized/date';

	import { commonDates, defaultTodo } from '@references/constants';
	import type { TodoItemDTO } from '@app/references/codegen';

	import dayjs from 'dayjs';
	import localizedFormat from 'dayjs/plugin/localizedFormat';

	dayjs.extend(localizedFormat);

	let value: DateValue | undefined = undefined;

	const df = new DateFormatter('en-US', {
		dateStyle: 'long',
	});

	// Format date strings
	const formatDateTime = (dateString: string) => {
		try {
			return dayjs(dateString).format('llll');
		} catch {
			return 'Invalid date';
		}
	};

	export let todo: TodoItemDTO = defaultTodo;
	const isExistingTodo = !!todo.id;

	export let onDelete: (todo: TodoItemDTO, isPermanent: boolean) => void;
</script>

<Accordion.Root>
	<Accordion.Item value={`todo-${todo.id}`}>
		<Accordion.Trigger>
			<Checkbox
				aria-label={`Mark as ${todo.isComplete} ? 'incomplete ' : 'complete'`}
				checked={todo.isComplete}
				class="mx-2"
				on:click={() => (todo.isComplete = !todo.isComplete)}
			/>
			<Input
				bind:value={todo.title}
				class="ml-1 h-7 border-none px-2 focus-visible:ring-transparent"
				placeholder="eg. Take the cat out for a walk"
			/>
		</Accordion.Trigger>
		<Accordion.Content>
			<div class="flex flex-col space-y-4">
				<div class="grid w-full space-y-1.5">
					<Label for="description">Description (optional)</Label>
					<Textarea bind:value={todo.description} class="focus-visible:ring-0" id="description" />
					<p class="text-muted-foreground text-sm">Extra details on what needs to be done.</p>
				</div>

				<Popover.Root openFocus>
					<Popover.Trigger asChild let:builder>
						<div class="flex flex-row items-end justify-between">
							<div class="flex flex-col space-y-1.5">
								<Label for="dueDate">Due date (optional)</Label>
								<Button
									variant="outline"
									class={cn(
										'w-[280px] justify-start text-left font-normal',
										!value && 'text-muted-foreground'
									)}
									builders={[builder]}
								>
									<CalendarIcon class="mr-2 h-4 w-4" />
									{value ? df.format(value.toDate(getLocalTimeZone())) : 'Pick a date'}
								</Button>
							</div>
							{#if isExistingTodo}
								<AlertDialog.Root>
									<AlertDialog.Trigger>
										<Button variant="destructive">Delete</Button>
									</AlertDialog.Trigger>
									<AlertDialog.Content>
										<AlertDialog.Header>
											<AlertDialog.Title>Confirm deletion</AlertDialog.Title>
											<AlertDialog.Description>
												<p>Specify how you would like us to handle this item's deletion.</p>
												<p><strong>Hard delete</strong>: Permanent deletion</p>
												<p><strong>Soft delete</strong>: Think of it as archiving</p>
												<p><strong>Cancel</strong>: To exit out of this dialog</p>
											</AlertDialog.Description>
										</AlertDialog.Header>
										<AlertDialog.Footer>
											<AlertDialog.Cancel>Cancel</AlertDialog.Cancel>
											<AlertDialog.Action
												on:click={() => {
													onDelete(todo, false);
												}}>Soft delete</AlertDialog.Action
											>
											<AlertDialog.Action
												on:click={() => {
													onDelete(todo, true);
												}}>Hard delete</AlertDialog.Action
											>
										</AlertDialog.Footer>
									</AlertDialog.Content>
								</AlertDialog.Root>
							{:else}
								<Button variant="destructive" on:click={() => onDelete(todo, false)}>Delete</Button>
							{/if}
						</div>
					</Popover.Trigger>
					<Popover.Content class="flex w-auto flex-col space-y-2 p-2">
						<Select.Root
							items={commonDates}
							onSelectedChange={(v) => {
								if (!v) return;
								value = today(getLocalTimeZone()).add({ days: v.value });
							}}
						>
							<Select.Trigger>
								<Select.Value placeholder="Select" />
							</Select.Trigger>
							<Select.Content>
								{#each commonDates as item}
									<Select.Item value={item.value}>{item.label}</Select.Item>
								{/each}
							</Select.Content>
						</Select.Root>
						<div class="rounded-md border">
							<Calendar id="dueDate" bind:value />
						</div>
					</Popover.Content>
				</Popover.Root>

				{#if isExistingTodo}
					<div class="flex flex-row items-center space-x-2">
						<p class="text-xs text-slate-500">
							Created at: {formatDateTime(todo.createdAt as string)}
						</p>
						<p class="text-xs text-slate-500">
							Last updated: {formatDateTime(todo.updatedAt as string)}
						</p>
					</div>
				{/if}
			</div>
		</Accordion.Content>
	</Accordion.Item>
</Accordion.Root>
