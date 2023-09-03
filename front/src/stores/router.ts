import { writable } from 'svelte/store';

export interface RouterData {
    key: RouterKey;
}

export enum RouterKey {
    Home = 'home',
    Databases = 'databases',
    Words = 'words',
}

export const router = writable<RouterData>({
    key: RouterKey.Words
});
