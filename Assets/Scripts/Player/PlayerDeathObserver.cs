using System;
using JetBrains.Annotations;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Player
{
    [UsedImplicitly]
    public class PlayerDeathObserver : IInitializable, IDisposable
    {
        private readonly ISnake _player;
        private readonly IWorldBounds _worldBounds;
        private readonly IGameUI _gameUI;

        public PlayerDeathObserver(ISnake playerService, IWorldBounds worldBounds, IGameUI gameUI)
        {
            _player = playerService;
            _worldBounds = worldBounds;
            _gameUI = gameUI;
        }

        void IInitializable.Initialize()
        {
            _player.OnSelfCollided += OnDead;
            _player.OnMoved += CheckCollision;
        }

        private void CheckCollision(Vector2Int position)
        {
            if (!_worldBounds.IsInBounds(position))
                OnDead();
        }

        private void OnDead()
        {
            _gameUI.GameOver(false);
            _player.SetActive(false);
        }

        void IDisposable.Dispose()
        {
            _player.OnSelfCollided -= OnDead;
            _player.OnMoved -= CheckCollision;
        }
    }
}