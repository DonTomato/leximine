<script lang="ts">
    import WordList from "./WordList.svelte";
    
    import { store } from "../stores/store";
    import { get, post } from "../lib/api";
    import type { InitResponse, WordsPageRecord } from "./models";

    let totalCount: number;
    let knownCount: number;
    let pageIndex: number;
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
</script>

<h1 class="page-header">Words</h1>

<WordList {words} 
    on:bacameKnown={makeKnown}
    on:becameUnknown={makeUnknown} />

<style lang="scss">
    
</style>