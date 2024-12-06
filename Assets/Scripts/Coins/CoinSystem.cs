using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Modules;
using SnakeGame;
using UnityEngine;

namespace Coins
{
    [UsedImplicitly]
    public class CoinSystem
    {
        public event Action<Coin> OnCollect;
        public event Action OnAllCoinsCollected;

        public event Action<Coin> OnSpawn;

        private readonly HashSet<Coin> _activeCoins = new();

        private readonly IWorldBounds _worldBounds;
        private readonly CoinSpawner _coinSpawner;

        public CoinSystem(IWorldBounds worldBounds, CoinSpawner coinSpawner)
        {
            _worldBounds = worldBounds;
            _coinSpawner = coinSpawner;
        }

        public void TryCollect(Vector2Int position)
        {
            foreach (var coin in _activeCoins)
            {
                if (coin.Position != position) continue;

                _coinSpawner.RemoveCoin(coin);
                _activeCoins.Remove(coin);
                
                if (!HasCoins()) OnAllCoinsCollected?.Invoke();

                OnCollect?.Invoke(coin);
                return;
            }
        }

        public void SpawnCoins(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var position = _worldBounds.GetRandomPosition();
                var coin = _coinSpawner.SpawnCoin(position);
                _activeCoins.Add(coin);
                OnSpawn?.Invoke(coin);
            }
        }

        public bool HasCoins()
        {
            return _activeCoins.Count > 0;
        }

    }
}