<script lang="ts">
    import { RouterKey, router } from '../../stores/router';
    import Databases from '../../databases/Databases.svelte';
    import Home from '../../home/Home.svelte';
    import Words from '../../words/Words.svelte';
</script>

{#if $router.key === RouterKey.Home}
    <Home />
{:else if $router.key === RouterKey.Words}
    <Words />
{:else if $router.key === RouterKey.Databases}
    <Databases />
{/if}
