using System;
using Coins;
using DifficultySystem;
using JetBrains.Annotations;
using Modules;
using SnakeGame;
using Zenject;

namespace Player
{
    [UsedImplicitly]
    public class PlayerWinObserver : IInitializable, IDisposable
    {
        private readonly IDifficulty _difficulty;
        private readonly IGameUI _gameUI;
        private readonly CoinSystem _coinSystem;
        private readonly ISnake _player;

        public PlayerWinObserver(IDifficulty difficulty, IGameUI gameUI, CoinSystem coinSystem, ISnake player)
        {
            _difficulty = difficulty;
            _gameUI = gameUI;
            _coinSystem = coinSystem;
            _player = player;
        }

        void IInitializable.Initialize()
        {
            _coinSystem.OnAllCoinsCollected += CheckWin;
        }

        private void CheckWin()
        {
            if (_difficulty.Current != _difficulty.Max) return;
            _gameUI.GameOver(true);
            _player.SetActive(false);
        }

        void IDisposable.Dispose()
        {
            _coinSystem.OnAllCoinsCollected -= CheckWin;
        }
    }
}