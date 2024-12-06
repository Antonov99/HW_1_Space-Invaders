using System;
using JetBrains.Annotations;
using Modules;
using UnityEngine;
using Zenject;

namespace Coins
{
    [UsedImplicitly]
    public class CoinSpawnController:IInitializable,IDisposable
    {
        private readonly CoinSystem _coinSystem;
        private readonly IDifficulty _difficulty;

        public CoinSpawnController(CoinSystem coinSystem, IDifficulty difficulty)
        {
            _coinSystem = coinSystem;
            _difficulty = difficulty;
        }

        void IInitializable.Initialize()
        {
            _difficulty.OnStateChanged += OnSpawn;
        }

        private void OnSpawn()
        {
            _coinSystem.SpawnCoins(_difficulty.Current);
            Debug.Log(_difficulty.Current);
        }

        void IDisposable.Dispose()
        {
            _difficulty.OnStateChanged -= OnSpawn;
        }
    }
}