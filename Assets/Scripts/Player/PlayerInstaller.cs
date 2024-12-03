using JetBrains.Annotations;
using Modules;
using Zenject;

namespace Player
{
    [UsedImplicitly]
    public class PlayerInstaller : Installer<ISnake, PlayerInstaller>
    {
        [Inject]
        private ISnake _snake;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerDeathObserver>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerWinObserver>().AsSingle().NonLazy();
            Container.Bind<PlayerService>().AsSingle().WithArguments(_snake);
            Container.BindInterfacesAndSelfTo<PlayerMoveController>().AsSingle().NonLazy();
        }
    }
}