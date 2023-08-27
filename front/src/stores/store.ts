import { writable } from 'svelte/store';

export interface StoreData {
    lang: string;
    langs: string[];
}

export const store = writable<StoreData>({
    lang: '',
    langs: []
});