<script lang="ts">
    import { onMount } from "svelte";
    import { store } from "../stores/store";
  import { get } from "../lib/api";

    let pageIndex: number;
    let lang: string;

    $: {
        if ($store.lang !== lang) {
            lang = $store.lang;
        }
    }

    onMount(() => {
        // page = document.querySelector(".page");
    });

    async function getInitData() {
        const response = await get<number>(`${lang}/words/data`);
    }
</script>

<h1 class="page-header">Words</h1>


<style lang="scss">

</style>