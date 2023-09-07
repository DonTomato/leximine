<script lang="ts">
    import { createEventDispatcher } from "svelte";

    const dispatch = createEventDispatcher();

    export let totalCount: number;
    export let pageSize: number;
    export let index: number;

    $: lastPage = Math.ceil(totalCount / pageSize) - 1;
</script>

<div class="paginator">
    <button class="lx-btn" on:click={() => dispatch('page', 0)}>1</button>
    <button
        class="lx-btn"
        disabled={index === 0}
        on:click={() => dispatch("page", index - 1)}>
        Prev
    </button>
    <span class="current">{index + 1}</span>
    <button 
        class="lx-btn"
        disabled={index === totalCount - 1}
        on:click={() => dispatch("page", index + 1)}>
        Next
    </button>
    <button class="lx-btn" on:click={() => dispatch('page', lastPage)}>{lastPage + 1}</button>
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
        padding: 0 2rem;
    }
</style>