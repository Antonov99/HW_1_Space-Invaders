using System;
using JetBrains.Annotations;
using Modules;
using Zenject;

namespace Coins
{
    [UsedImplicitly]
    public class CoinCollector : IInitializable, IDisposable
    {
        private readonly ISnake _player;
        private readonly CoinSystem _coinSystem;
        private readonly IScore _score;

        public CoinCollector(ISnake player, CoinSystem coinSystem, IScore score)
        {
            _player = player;
            _coinSystem = coinSystem;
            _score = score;
        }

        void IInitializable.Initialize()
        {
            _coinSystem.OnCollect += OnCollect;
        }

        private void OnCollect(Coin coin)
        {
            _player.Expand(coin.Bones);
            _score.Add(coin.Score);
        }

        void IDisposable.Dispose()
        {
            _coinSystem.OnCollect -= OnCollect;
        }
    }
}