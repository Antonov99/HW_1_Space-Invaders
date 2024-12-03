using Coins;
using DifficultySystem;
using Game;
using InputSystem;
using Modules;
using ScoreSystem;
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

        [SerializeField]
        private GameUI _gameUI;

        [SerializeField]
        private Coin _coinPrefab;
        
        public override void InstallBindings()
        {
            InputInstaller.Install(Container);
            GameInstaller.Install(Container, _gameUI, _worldBounds);
            CoinsInstaller.Install(Container,_coinPrefab);
            ScoreInstaller.Install(Container);
            DifficultyInstaller.Install(Container);

            Container.BindInterfacesAndSelfTo<PlayerDeathObserver>().AsSingle();
            Container.Bind<PlayerService>().AsSingle().WithArguments(_snake);
            Container.BindInterfacesAndSelfTo<PlayerMoveController>().AsSingle();
        }
    }
}