using Components;
using UnityEngine;

namespace ShootEmUp.Agents
{
    public class EnemyAttackAgent : MonoBehaviour
    {
        private BulletSystem _bulletSystem;

        [SerializeField]
        private WeaponComponent weaponComponent;
        
        [SerializeField]
        private EnemyMoveAgent moveAgent;

        private GameObject _target;

        private float _currentTime;

        [SerializeField]
        private float countdown;

        public void Construct(BulletSystem bulletSystem, GameObject target)
        {
            _bulletSystem = bulletSystem;
            _target = target;
        }

        public void FixedUpdate()
        {
            if (!moveAgent.IsReached)
            {
                return;
            }

            if (!_target.GetComponent<HealthComponent>().IsHitPointsExists())
            {
                return;
            }

            _currentTime -= Time.fixedDeltaTime;
            if (_currentTime <= 0)
            {
                Fire();
                Reset();
            }
        }

        private void Reset()
        {
            _currentTime = countdown;
        }

        private void Fire()
        {
            var startPosition = weaponComponent.Position;
            var vector = (Vector2) _target.transform.position - startPosition;
            var direction = vector.normalized;
            
            _bulletSystem.SpawnBullet(new BulletArgs
            {
                position = startPosition,
                velocity = direction * 2,
                color = Color.red,
                physicsLayer = (int)PhysicsLayer.ENEMY_BULLET,
                damage = 1,
                isPlayer = false
            });
        }
    }
}