<script lang="ts">
    import WordList from "./WordList.svelte";
    
    import { store } from "../stores/store";
    import { get, post } from "../lib/api";
    import type { InitResponse, WordsPageRecord } from "./models";
    import Paginator from "./Paginator.svelte";

    let totalCount: number;
    let knownCount: number;
    let pageIndex: number;
    let pageSize: number;
    let lang: string;

    let words: WordsPageRecord[];

    $: {
        if ($store.lang !== lang) {
            lang = $store.lang;
            if (lang) {
                getInitData();
            }
        }
    }

    async function getInitData() {
        const response = await get<InitResponse>(`${lang}/words/init`);
        totalCount = response.totalCount;
        knownCount = response.knownCount;
        pageIndex = response.initialPageIndex;
        pageSize = response.pageSize;
        
        await getWords();
    }

    async function getWords() {
        words = await get<WordsPageRecord[]>(`${lang}/words/all/${pageIndex}`);
    }

    async function makeKnown(event: CustomEvent<WordsPageRecord>) {
        const word = event.detail;
        await post(`${lang}/words/make/known`, [ word.word ]);
        word.known = true;
        words = words;
    }

    async function makeUnknown(event: CustomEvent<WordsPageRecord>) {
        const word = event.detail;
        await post(`${lang}/words/make/unknown`, [ word.word ]);
        word.known = false;
        words = words;
    }

    async function changePage(event: CustomEvent<number>) {
        pageIndex = event.detail;
        await getWords();
    }
</script>

<h1 class="page-header">Words</h1>

<WordList {words} 
    on:bacameKnown={makeKnown}
    on:becameUnknown={makeUnknown} />

<Paginator {pageSize} {totalCount} 
    bind:index={pageIndex}
    on:page={changePage} />

<style lang="scss">
    
</style>