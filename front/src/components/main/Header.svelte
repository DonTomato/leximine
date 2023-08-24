<script lang="ts">
    import { RouterKey, router } from '../../stores/router';

    function go(key: RouterKey) {
        router.update(s => ({ ...s, key }));
    }
</script>

<header class="container header">
    <a class="logo" href="/">
        <img src="/logo.png" alt="">
        <h1>Leximine</h1>
    </a>

    <div class="menu">
        <button class="item" class:active={$router.key === RouterKey.Home} on:click={() => go(RouterKey.Home)}>Home</button>
        <button class="item" class:active={$router.key === RouterKey.Words} on:click={() => go(RouterKey.Words)}>Words</button>
        <button class="item" class:active={$router.key === RouterKey.Databases} on:click={() => go(RouterKey.Databases)}>Databases</button>
    </div>
</header>

<style lang="scss">
    .header {
        display: flex;
        align-items: center;
        padding: 1rem 0;
        background-color: #fff;
        height: 6rem;
    }

    .logo {
        display: flex;
        text-decoration: none;

        img {
            width: 36px;
            height: 36px;
            margin-right: 1.6rem;
            display: block;
        }

        h1 {
            font-weight: 600;
            font-size: 2.5rem;
            color: #3b0947;
        }
    }

    .menu {
        display: flex;
        align-items: center;
        margin-left: 6rem;

        .item {
            font-size: 1.6rem;
            font-weight: 400;
            color: #3b0947;
            background-color: transparent;
            border: none;
            cursor: pointer;
            padding: 0.5rem 1rem;
            border-radius: 0.5rem;
            transition: background-color 0.2s ease-in-out;
            margin-left: 2rem;

            &:hover {
                background-color: #f5f5f5;
            }

            &:active {
                background-color: #ebebeb;
            }

            &.active {
                font-weight: 600;
            }
        }
    }

</style>