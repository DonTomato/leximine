<script lang="ts">
    export let size: number;

    // returns size in bytes, kb, mb, gb, tb
    function getSize(size: number) {
        const i = Math.floor(Math.log(size) / Math.log(1024));
        return (
            (size / Math.pow(1024, i)).toFixed(2) +
            " " +
            ["B", "KB", "MB", "GB", "TB"][i]
        );
    }
</script>

<div>
    <div class="size">{getSize(size)}</div>
</div>