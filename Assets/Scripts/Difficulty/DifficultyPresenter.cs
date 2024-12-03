using System;
using JetBrains.Annotations;
using Modules;
using SnakeGame;
using Zenject;

namespace DifficultySystem
{
    [UsedImplicitly]
    public class DifficultyPresenter : IInitializable, IDisposable
    {
        private readonly IGameUI _gameUI;
        private readonly IDifficulty _difficulty;

        public DifficultyPresenter(IGameUI gameUI, IDifficulty difficulty)
        {
            _gameUI = gameUI;
            _difficulty = difficulty;
        }

        void IInitializable.Initialize()
        {
            _difficulty.OnStateChanged += UpdateLevel;
        }

        private void UpdateLevel()
        {
            _gameUI.SetDifficulty(_difficulty.Current, _difficulty.Max);
        }

        void IDisposable.Dispose()
        {
            _difficulty.OnStateChanged -= UpdateLevel;
        }
    }
}