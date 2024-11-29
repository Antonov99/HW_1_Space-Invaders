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

        public PlayerDeathObserver(PlayerService playerService, IWorldBounds worldBounds)
        {
            _player = playerService.Player;
            _worldBounds = worldBounds;
        }

        public void Initialize()
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
            Debug.Log("Dead");
            _player.SetActive(false);
        }

        public void Dispose()
        {
            _player.OnSelfCollided -= OnDead;
            _player.OnMoved -= CheckCollision;
        }
    }
}