using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace ShootEmUp
{
    [UsedImplicitly]
    public class EnemyCooldownSpawner: IInitializable,IDisposable
    {
        private readonly EnemyManager _enemyManager;
        private const int _COUNTDOWN = 2000;
        private bool _spawning=true;

        public EnemyCooldownSpawner(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        void IInitializable.Initialize()
        {
            StartEnemySpawnAsync();
        }

        private async Task StartEnemySpawnAsync()
        {
            while (_spawning)
            {
                _enemyManager.SpawnEnemy();
                await Task.Delay(_COUNTDOWN);
            }
        }

        void IDisposable.Dispose()
        {
            _spawning = false;
        }
    }
}