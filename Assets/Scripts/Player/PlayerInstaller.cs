using InputSystem;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerInstaller:MonoInstaller
    {
        [SerializeField]
        private Snake _snake;

        [SerializeField]
        private WorldBounds _worldBounds;
        
        public override void InstallBindings()
        {
            InputInstaller.Install(Container);

            Container.BindInterfacesAndSelfTo<PlayerDeathObserver>().AsSingle().WithArguments(_worldBounds);
            Container.Bind<PlayerService>().AsSingle().WithArguments(_snake);
            Container.BindInterfacesAndSelfTo<PlayerMoveController>().AsSingle();
        }
    }
}