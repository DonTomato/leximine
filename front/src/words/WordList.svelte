<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import type { WordsPageRecord } from "./models";

    export let words: WordsPageRecord[] = [];

    const dispatch = createEventDispatcher();
</script>

<div class="list-container">
    {#each words as word}
        <div class="item" class:known={word.known}>
            <div class="word">{word.word}</div>
            <div class="def">{word.defaultForm}</div>
            <div class="total lx-mono">{word.totalCount}</div>
            <div class="actions">
                {#if !word.known}
                    <button class="lx-btn" on:click={() => dispatch('bacameKnown', word)}>Make Known</button>
                {:else}
                    <button class="lx-btn" on:click={() => dispatch('becameUnknown', word)}>Make unknown</button>
                {/if}
            </div>
        </div>
    {/each}
</div>

<style lang="scss">
    .list-container {
        .item {
            &:nth-child(even) {
                background-color: #f8f8f8;
            }
            
            padding: 1rem 2rem;
            display: flex;

            &.known {
                background-color: #eafcea;
            }

            .word {
                flex: 2 0 0;
            }

            .def {
                flex: 2 0 0;
            }

            .total {
                flex: 1 0 0;
                display: flex;
                justify-content: flex-end;
            }

            .actions {
                flex: 4 0 0;
                display: flex;
                justify-content: flex-end;
            }
        }
    }
</style>