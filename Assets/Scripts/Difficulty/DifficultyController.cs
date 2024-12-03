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
        }

        private void OnSpawn(Coin coin)
        {
            _activeCoins.Add(coin);
        }

        private void OnCollect(Coin coin)
        {
            _activeCoins.Remove(coin);
            _playerService.Player.Expand(coin.Bones);
            if (_activeCoins.Count > 0) return;
            if (_difficulty.Next(out int difficulty))
            {
                _playerService.Player.SetSpeed(difficulty);
            }
            else
            {
                OnWin?.Invoke();
                _playerService.Player.SetActive(false);
            }
        }

        void IDisposable.Dispose()
        {
            _coinSystem.OnCollect -= OnCollect;
            _coinSystem.OnSpawn -= OnSpawn;
        }
    }
}