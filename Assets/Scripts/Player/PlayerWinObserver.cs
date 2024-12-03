using System;
using DifficultySystem;
using JetBrains.Annotations;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Player
{
    [UsedImplicitly]
    public class PlayerWinObserver : IInitializable, IDisposable
    {
        private readonly DifficultyController _difficultyController;
        private readonly IGameUI _gameUI;

        public PlayerWinObserver(DifficultyController difficultyController, IGameUI gameUI)
        {
            _difficultyController = difficultyController;
            _gameUI = gameUI;
        }

        void IInitializable.Initialize()
        {
            _difficultyController.OnWin += OnWin;
        }

        private void OnWin()
        {
            _gameUI.GameOver(true);
        }

        void IDisposable.Dispose()
        {
            _difficultyController.OnWin -= OnWin;
        }
    }
}