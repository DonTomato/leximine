<script lang="ts">
    import DatabaseItem from "./DatabaseItem.svelte";
import type { DbInfo } from "./models";

    export let databases: DbInfo[];
</script>

<div class="databases">
    <div class="databases-header">
        <h2 class="header-2">Backups</h2>
    </div>

    <div class="list">
        {#each databases as db}
            <div>
                <DatabaseItem {db} />
            </div>
        {/each}
    </div>
</div>

<style lang="scss">
    .databases {
        margin-top: 2rem;

        .list {
            > *:not(:last-child) {
                border-bottom: 1px solid #c5d3e3;
            }

            > *:nth-child(even) {
                background-color: #f8f8f8;
            }
        }
    }
</style>