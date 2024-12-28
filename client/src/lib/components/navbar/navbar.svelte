<script lang="ts">
	import { authStore } from '@stores/auth';
	import { buttonVariants } from '@ui/button';
</script>

<nav class="flex flex-row items-center justify-between p-4">
	<section>
		<a class={`${buttonVariants({ variant: 'link' })}`} href="/">Basic Todo</a>
	</section>

	<section>
		<a
			class={`${buttonVariants({ variant: 'link', size: 'sm' })} text-xs text-slate-600`}
			href="/profile"
		>
			{$authStore?.user?.email}
		</a>
	</section>

	<section>
		<a class={buttonVariants({ variant: 'default', size: 'sm' })} href="/new">New</a>
	</section>
</nav>
