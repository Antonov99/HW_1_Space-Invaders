using JetBrains.Annotations;
using Modules;
using Zenject;

namespace Coins
{
    [UsedImplicitly]
    public class CoinsInstaller : Installer<Coin, CoinsInstaller>
    {
        [Inject]
        private Coin _coinPrefab;

        public override void InstallBindings()
        {
            Container.Bind<CoinSpawner>().AsSingle().NonLazy();
            Container.Bind<CoinSystem>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CoinSpawnController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CoinCollectController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CoinCollector>().AsSingle().NonLazy();

            Container.BindMemoryPool<Coin, MonoMemoryPool<Coin>>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransformGroup("Coins").NonLazy();
        }
    }
}