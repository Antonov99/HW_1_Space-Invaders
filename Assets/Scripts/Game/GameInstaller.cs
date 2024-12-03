using Coins;
using DifficultySystem;
using InputSystem;
using JetBrains.Annotations;
using Modules;
using Player;
using ScoreSystem;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game
{
    [UsedImplicitly]
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private Snake _snake;

        [SerializeField]
        private Coin _coinPrefab;

        [SerializeField]
        private int _difficulty = 3;

        [SerializeField]
        private GameUI _gameUI;

        [SerializeField]
        private WorldBounds _worldBounds;

        public override void InstallBindings()
        {
            PlayerInstaller.Install(Container, _snake);
            InputInstaller.Install(Container);
            CoinsInstaller.Install(Container, _coinPrefab);
            ScoreInstaller.Install(Container);
            DifficultyInstaller.Install(Container, _difficulty);

            Container.Bind<IGameUI>().To<GameUI>().FromInstance(_gameUI).AsSingle();
            Container.Bind<IWorldBounds>().To<WorldBounds>().FromInstance(_worldBounds).AsSingle();
        }
    }
}