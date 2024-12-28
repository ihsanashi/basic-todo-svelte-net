<script lang="ts">
	import { buttonVariants } from '@ui/button';

	import * as Avatar from '@ui/avatar';
	import { UserIcon } from 'lucide-svelte';
</script>

<nav class="flex flex-row items-center justify-between p-4">
	<section>
		<a
			class={`${buttonVariants({ variant: 'link', size: 'sm' })} text-xs text-slate-600`}
			href="/profile"
		>
			<Avatar.Root>
				<Avatar.Fallback class="hover:bg-slate-100">
					<UserIcon size={16} />
				</Avatar.Fallback>
			</Avatar.Root>
		</a>
	</section>

	<section>
		<a class={`${buttonVariants({ variant: 'link' })}`} href="/">Basic Todo</a>
	</section>

	<section>
		<a class={buttonVariants({ variant: 'outline', size: 'sm' })} href="/new">New</a>
	</section>
</nav>
