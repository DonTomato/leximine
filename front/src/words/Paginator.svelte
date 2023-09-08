<script lang="ts">
    import { createEventDispatcher, tick } from "svelte";

    const dispatch = createEventDispatcher();

    export let totalCount: number;
    export let pageSize: number;
    export let index: number;

    $: lastPage = Math.ceil(totalCount / pageSize) - 1;

    async function onChange() {
        await tick();
        dispatch('page', index)
    }
</script>

<div class="paginator">
    <button class="lx-btn" on:click={() => dispatch('page', 1)}>1</button>
    <button
        class="lx-btn"
        disabled={index === 1}
        on:click={() => dispatch("page", index - 1)}>
        Prev
    </button>
    <!-- <span class="current">{index}</span> -->
    <input type="number" class="lx-input current" min="1" max={lastPage + 1} bind:value={index} on:change={onChange}>
    <button 
        class="lx-btn"
        disabled={index === totalCount - 2}
        on:click={() => dispatch("page", index + 1)}>
        Next
    </button>
    <button class="lx-btn" on:click={() => dispatch('page', lastPage + 1)}>{lastPage + 1}</button>
</div>

<style lang="scss">
    .paginator {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 2rem;
        gap: 1rem;
    }

    .current {
        // padding: 0 2rem;
        width: auto;
    }
</style>