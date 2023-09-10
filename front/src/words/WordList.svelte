<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import type { WordsPageRecord } from "./models";
  import Dropdown from "../lib/Dropdown.svelte";

    export let words: WordsPageRecord[] = [];

    const dispatch = createEventDispatcher();
</script>

<div class="list-container">
    {#each words as word}
        <div class="item" class:known={word.known}>
            <div class="no">{word.no}</div>
            <div class="word">{word.word}</div>
            <div class="def">{word.defaultForm}</div>
            <div class="total lx-mono">{word.totalCount}</div>
            <div class="actions">
                {#if !word.known}
                    <button class="lx-btn" on:click={() => dispatch('bacameKnown', word)}>Make Known</button>
                {:else}
                    <button class="lx-btn" on:click={() => dispatch('becameUnknown', word)}>Make unknown</button>
                {/if}

                <div class="menu">
                    <Dropdown items={[
                        { id: '1', text: 'One' },
                        { id: '2', text: 'Two' },
                        { id: '3', text: 'Three' },
                        { id: '4', text: 'Special item' }
                    ]} />
                </div>
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

            .no {
                flex: 1 0 0;
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

                > .menu {
                    margin-left: 1rem;
                }
            }
        }
    }
</style>