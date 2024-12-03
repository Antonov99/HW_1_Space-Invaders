using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Modules;
using Player;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Coins
{
    [UsedImplicitly]
    public class CoinSystem : IInitializable, IDisposable
    {
        public event Action<Coin> OnCollect;
        public event Action<Coin> OnSpawn;

        private readonly HashSet<Coin> _activeCoins = new();

        private readonly IWorldBounds _worldBounds;
        private readonly CoinSpawner _coinSpawner;
        private readonly IDifficulty _difficulty;
        private readonly PlayerService _playerService;
        private readonly IScore _score;

        public CoinSystem(IWorldBounds worldBounds, CoinSpawner coinSpawner, IDifficulty difficulty,
            PlayerService playerService, IScore score)
        {
            _worldBounds = worldBounds;
            _coinSpawner = coinSpawner;
            _difficulty = difficulty;
            _playerService = playerService;
            _score = score;
        }

        void IInitializable.Initialize()
        {
            _difficulty.OnStateChanged += SpawnCoins;
            _playerService.Player.OnMoved += TryCollect;
            SpawnCoins();
        }

        private void TryCollect(Vector2Int position)
        {
            foreach (var coin in _activeCoins)
            {
                if (coin.Position != position) continue;

                _coinSpawner.RemoveCoin(coin);
                _activeCoins.Remove(coin);
                _score.Add(coin.Score);
 
                OnCollect?.Invoke(coin);
                return;
            }
        }

        private void SpawnCoins()
        {
            for (int i = 0; i < _difficulty.Current; i++)
            {
                var position = _worldBounds.GetRandomPosition();
                var coin = _coinSpawner.SpawnCoin(position);
                _activeCoins.Add(coin);
                OnSpawn?.Invoke(coin);
            }
        }

        void IDisposable.Dispose()
        {
            _difficulty.OnStateChanged -= SpawnCoins;
            _playerService.Player.OnMoved -= TryCollect;
        }
    }
}