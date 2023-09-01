<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import Size from "../lib/Size.svelte";
    import type { DbInfo } from "./models";

    const dispatch = createEventDispatcher();

    export let db: DbInfo;
</script>

<div class="container">
    <div class="fileName">{db.fileName}</div>
    <div class="size">
        <Size size={db.size} />
    </div>
    <div class="actions">
        <button class="lx-btn" on:click={() => dispatch('restore')}>Restore</button>
        <button class="lx-btn" on:click={() => dispatch('delete')}>Delete</button>
    </div>
</div>

<style lang="scss">
    .container {
        display: flex;
        padding: 1rem 2rem;
        align-items: center;

        > .fileName {
            flex: 1 0 0;
        }

        >.size {
            flex: 1 0 0;
        }

        .actions {
            > :first-child {
                margin-right: 0.6rem;
            }
        }
    }
</style>