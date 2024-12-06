using System;
using JetBrains.Annotations;
using Modules;
using UnityEngine;
using Zenject;

namespace Coins
{
    [UsedImplicitly]
    public class CoinCollectController:IInitializable,IDisposable
    {
        private readonly ISnake _player;
        private readonly CoinSystem _coinSystem;

        public CoinCollectController(ISnake player, CoinSystem coinSystem)
        {
            _player = player;
            _coinSystem = coinSystem;
        }

        void IInitializable.Initialize()
        {
            _player.OnMoved += Collect;
        }

        private void Collect(Vector2Int position)
        {
            _coinSystem.TryCollect(position);
        }

        void IDisposable.Dispose()
        {
            _player.OnMoved -= Collect;
        }
    }
}