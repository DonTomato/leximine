<script lang="ts">
    import { del, get, post } from "../lib/api";
    import { store } from "../stores/store";
    import CurrentDatabase from "./CurrentDatabase.svelte";
    import DatabaseList from "./DatabaseList.svelte";
    import type { DatabaseListResponse, DbInfo } from "./models";

    let prevLang: string;
    $: {
        if (prevLang !== $store.lang && $store.lang) {
            requestData();
        }
        prevLang = $store.lang;
    }

    let mainDb: DbInfo;
    let backups: DbInfo[];

    async function requestData() {
        const response = await get<DatabaseListResponse>(`${$store.lang}/dblist`);
        mainDb = response.mainDb;
        backups = response.backups;
    }

    async function createBackup() {
        await post(`${$store.lang}/createbackup`, null);
        await requestData();
    }

    async function deleteBackup(event: CustomEvent<DbInfo>) {
        await del(`${$store.lang}/deletebackup/${event.detail.fileName}`);
        await requestData();
    }
</script>

<h1 class="page-header">Databases</h1>

<div class="content">
    <p>Here you can manage your databases.</p>

    {#if mainDb}
        <CurrentDatabase fileName={mainDb.fileName} size={mainDb.size} 
            on:backup={createBackup} />
    {/if}

    {#if backups && backups.length}
        <DatabaseList databases={backups} on:delete={deleteBackup} />
    {/if}
</div>

<style lang="scss">
    .content {
        margin-top: 2rem;
    }
</style>