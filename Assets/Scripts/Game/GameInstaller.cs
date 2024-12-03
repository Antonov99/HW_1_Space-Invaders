using JetBrains.Annotations;
using SnakeGame;
using Zenject;

namespace Game
{
    [UsedImplicitly]
    public class GameInstaller:Installer<GameUI, WorldBounds, GameInstaller>
    {
        [Inject]
        private GameUI _gameUI;

        [Inject]
        private WorldBounds _worldBounds;
        
        public override void InstallBindings()
        {
            Container.Bind<IGameUI>().To<GameUI>().FromInstance(_gameUI).AsSingle();
            Container.Bind<IWorldBounds>().To<WorldBounds>().FromInstance(_worldBounds).AsSingle();
        }
    }
}