<script lang="ts">
    import Size from "../lib/Size.svelte";
    import type { DbInfo } from "./models";

    export let db: DbInfo;
</script>

<div class="container">
    <div class="fileName">{db.fileName}</div>
    <div class="size">
        <Size size={db.size} />
    </div>
    <div class="actions">
        <button class="lx-btn">Restore</button>
        <button class="lx-btn">Delete</button>
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