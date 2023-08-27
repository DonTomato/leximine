<script lang="ts">
    import { onMount } from "svelte";
    import Header from "./components/main/Header.svelte";
    import Router from "./components/main/Router.svelte";

    import { store } from "./stores/store";
    import { get } from "./lib/api";

    onMount(async () => {
        const langs = await get<string[]>("langs");
        store.update((s) => ({
            ...s, 
            langs,
            lang: langs[0]
        }));
    });
</script>

<div class="container">
    <Header />

    <main class="content">
        <Router />
    </main>
</div>


<style>
    .content {
        height: calc(100vh - 6rem);
    }
</style>
