using JetBrains.Annotations;
using Modules;
using UnityEngine;
using Zenject;

namespace Coins
{
    [UsedImplicitly]
    public class CoinSpawner
    {
        private readonly MonoMemoryPool<Coin> _coinPool;

        public CoinSpawner(MonoMemoryPool<Coin> coinPool)
        {
            _coinPool = coinPool;
        }

        public Coin SpawnCoin(Vector2Int position)
        {
            var coin = _coinPool.Spawn();
            coin.Position = position;
            coin.Generate();
            
            return coin;
        }

        public void RemoveCoin(Coin coin)
        {
            _coinPool.Despawn(coin);
        }
    }
}