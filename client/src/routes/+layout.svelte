<script lang="ts">
	import { Toaster } from '$lib/components/ui/sonner';
	export const ssr = false;
	import '../app.css';
</script>

<Toaster />

<slot />
