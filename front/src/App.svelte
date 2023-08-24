<script lang="ts">
    import Header from "./components/main/Header.svelte";
    import Router from "./components/main/Router.svelte";
</script>

<div class="container">
    <Header />

    <main class="content">
        <Router />
    </main>
</div>


<style>
    .content {
        height: calc(100vh - 6rem);
    }
</style>
