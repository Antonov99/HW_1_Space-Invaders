using System.Collections.Generic;
using Components;
using JetBrains.Annotations;
using UnityEngine;

namespace ShootEmUp
{
    [UsedImplicitly]
    public sealed class EnemyManager
    {
        private readonly EnemyPool _enemyPool;
        
        private readonly HashSet<GameObject> _activeEnemies = new();

        public EnemyManager(EnemyPool enemyPool)
        {
            _enemyPool = enemyPool;
        }

        public void SpawnEnemy()
        {
            var enemy = _enemyPool.SpawnEnemy();
            
            if (enemy == null) return;
            
            if (_activeEnemies.Add(enemy))
            {
                enemy.GetComponent<HealthComponent>().OnHpEmpty += OnDestroyed;
            }
        }

        private void OnDestroyed(GameObject enemy)
        {
            if (_activeEnemies.Remove(enemy))
            {
                enemy.GetComponent<HealthComponent>().OnHpEmpty -= OnDestroyed;

                _enemyPool.UnspawnEnemy(enemy);
            }
        }
    }
}