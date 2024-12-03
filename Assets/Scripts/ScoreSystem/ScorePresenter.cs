using System;
using JetBrains.Annotations;
using Modules;
using SnakeGame;
using Zenject;

namespace ScoreSystem
{
    [UsedImplicitly]
    public class ScorePresenter : IInitializable, IDisposable
    {
        private readonly IGameUI _gameUI;
        private readonly IScore _score;

        public ScorePresenter(IGameUI gameUI, IScore score)
        {
            _gameUI = gameUI;
            _score = score;
        }

        public void Initialize()
        {
            _score.OnStateChanged += UpdateScore;
            UpdateScore(0);
        }

        private void UpdateScore(int score)
        {
            _gameUI.SetScore(score.ToString());
        }

        public void Dispose()
        {
            _score.OnStateChanged -= UpdateScore;
        }
    }
}