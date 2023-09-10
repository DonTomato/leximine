<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import type { DropdownItem } from "./models";

    export let items: DropdownItem[] = [];

    const dispatch = createEventDispatcher();

    let expanded = false;
    let popupElement: HTMLElement;
    let blockOwnClick = false;

    function itemClick(id: string) {
        console.log('click', id);
        expanded = false;
        dispatch('itemClick', id);
    }

    function expandMenu(event: MouseEvent) {
        expanded = !expanded;
        blockOwnClick = true;
    }

    function onClick(event: MouseEvent) {
        if (blockOwnClick) {
            blockOwnClick = false;
            return;
        }
        if (expanded && popupElement && !popupElement.contains(event.target as Node)) {
            expanded = false;
        }
    }

    function keyUp(event: KeyboardEvent) {
        if (expanded && event.key === 'Escape') {
            expanded = false;
        }
    }
</script>

<svelte:document on:click={onClick} on:keyup={keyUp}>
</svelte:document>

<div class="dropdown">
    <div class="dropdown-trigger">
        <button class="lx-btn" aria-haspopup="true" aria-controls="dropdown-menu"
            on:click={expandMenu}>
            <svg xmlns="http://www.w3.org/2000/svg" width="21" height="21" viewBox="0 0 24 24">
                <circle cx="5" cy="12" r="2" />
                <circle cx="12" cy="12" r="2" />
                <circle cx="19" cy="12" r="2" />
              </svg>
        </button>
    </div>
    {#if expanded}
        <div class="dropdown-menu" id="dropdown-menu" role="menu" bind:this={popupElement}>
            <div class="dropdown-content">
                {#each items as item}
                    <button type="button" id={item.id} on:click={() => itemClick(item.id)}>
                        {item.text}
                    </button>
                {/each}
            </div>
        </div>
    {/if}
</div>

<style lang="scss">
    .dropdown {
        position: relative;
    }

    .dropdown-trigger {
        > button > svg {
            display: block;
        }
    }

    .dropdown-menu {
        position: absolute;
        top: 100%;
        left: 0;
        z-index: 10;
        display: block;
        right: 0;

        .dropdown-content {
            position: absolute;
            right: 0;
            padding: 0.5rem 0;
            background-color: #fff;
            border: 1px solid #ccc;
            border-radius: 0.25rem;
            box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);

            display: flex;
            flex-direction: column;
            gap: 0.25rem;
            padding: 0.25rem 0;
            min-width: 100px;

            > button {
                border: 0;
                background-color: transparent;
                cursor: pointer;
                padding: .4rem .75rem;
                border-radius: 4px;

                &:hover {
                    background-color: #f8f8f8;
                }

                &:focus {
                    outline: 2px solid #38909c;
                    outline-offset: 0;
                }
            }
        }
    }
</style>