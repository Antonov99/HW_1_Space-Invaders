using System;
using System.Collections.Generic;
using Coins;
using JetBrains.Annotations;
using Modules;
using Player;
using UnityEngine;
using Zenject;

namespace DifficultySystem
{
    [UsedImplicitly]
    public class DifficultyController : IInitializable, IDisposable
    {
        public event Action OnWin; 
        private readonly HashSet<Coin> _activeCoins = new();

        private readonly CoinSystem _coinSystem;
        private readonly IDifficulty _difficulty;
        private readonly PlayerService _playerService;

        public DifficultyController(CoinSystem coinSystem, IDifficulty difficulty, PlayerService playerService)
        {
            _coinSystem = coinSystem;
            _difficulty = difficulty;
            _playerService = playerService;
        }

        void IInitializable.Initialize()
        {
            _coinSystem.OnCollect += OnCollect;
            _coinSystem.OnSpawn += OnSpawn;

            _difficulty.Next(out int difficulty);
            Debug.Log(difficulty);
        }

        private void OnSpawn(Coin coin)
        {
            _activeCoins.Add(coin);
        }

        private void OnCollect(Coin coin)
        {
            _activeCoins.Remove(coin);
            if (_activeCoins.Count > 0) return;
            if (_difficulty.Next(out int difficulty))
            {
                _playerService.Player.SetSpeed(difficulty);
                _playerService.Player.Expand(coin.Bones);
            }
            else
                OnWin?.Invoke();
        }

        void IDisposable.Dispose()
        {
            _coinSystem.OnCollect -= OnCollect;
            _coinSystem.OnSpawn -= OnSpawn;
        }
    }
}