using System.Collections.Generic;
using ShootEmUp.Agents;
using UnityEngine;
using Zenject;

namespace ShootEmUp
{
    public class EnemyPool:MonoBehaviour
    {
        [SerializeField]
        private Transform worldTransform;

        [SerializeField]
        private Transform container;

        [SerializeField]
        private Enemy prefab;
        
        [SerializeField]
        private int count;

        [SerializeField]
        private EnemyPositions enemyPositions;
        
        private BulletSystem _bulletSystem;
        private GameObject _target;
        
        private readonly Queue<GameObject> _enemyPool = new();

        [Inject]
        public void Construct(BulletSystem bulletSystem, PlayerService playerService)
        {
            _bulletSystem = bulletSystem;
            _target = playerService.Player;
        }

        public void Awake()
        {
            for (var i = 0; i < count; i++)
            {
                var enemy = Instantiate(prefab, container);
                _enemyPool.Enqueue(enemy.gameObject);
                enemy.Construct(_bulletSystem, _target);
            }
        }

        public GameObject SpawnEnemy()
        {
            if (!_enemyPool.TryDequeue(out var enemy))
            {
                return null;
            }
            
            enemy.transform.SetParent(worldTransform);

            var spawnPosition = enemyPositions.RandomSpawnPosition();
            enemy.transform.position = spawnPosition.position;

            var attackPosition = enemyPositions.RandomAttackPosition();
            enemy.GetComponent<EnemyMoveAgent>().SetDestination(attackPosition.position);

            return enemy;
        }

        public void UnspawnEnemy(GameObject enemy)
        {
            enemy.transform.SetParent(container);
            _enemyPool.Enqueue(enemy);
        }
    }
}