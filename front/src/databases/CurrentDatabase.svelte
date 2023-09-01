<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import Size from "../lib/Size.svelte";

    const dispatch = createEventDispatcher();

    export let fileName: string;
    export let size: number;
    export let disabled: boolean = false;
</script>

<div class="file">
    <div class="left">
        <div class="file-name">{fileName}</div>
        <div class="file-size">
            <Size size={size} />
        </div>
    </div>

    <div class="right">
        <button class="lx-btn"
            on:click={() => dispatch('backup')} 
            disabled={disabled}>
            Create backup
        </button>
    </div>
</div>

<style lang="scss">
    .file {
        margin-top: 1rem;
        padding: 1rem 1.6rem;
        background-color: #eafcff;
        border-radius: 4px;
        border: 1px solid #c5d3e3;

        display: flex;
        justify-content: space-between;
        align-items: center;

        .left, .right {
            display: flex;
            align-items: center;
        }

        .file-name {
            font-weight: 600;
            margin-right: 5rem;
        }
    }
</style>