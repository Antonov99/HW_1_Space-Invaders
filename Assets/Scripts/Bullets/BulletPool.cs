using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class BulletPool:MonoBehaviour
    {
        [SerializeField]
        public Bullet prefab;
        
        [SerializeField]
        private Transform container;

        [SerializeField]
        private Transform worldTransform;

        [SerializeField]
        private int initialCount=10;
        
        private readonly Queue<Bullet> _mBulletPool = new();

        private void Awake()
        {
            for (var i = 0; i < initialCount; i++)
            {
                Bullet bullet = Instantiate(prefab, container);
                _mBulletPool.Enqueue(bullet);
            }
        }
        
        public Bullet SpawnBullet()
        {
            if (_mBulletPool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(worldTransform);
            }
            else
            {
                bullet = Instantiate(prefab, worldTransform);
            }

            return bullet;
        }

        public void RemoveBullet(Bullet bullet)
        {
            bullet.transform.SetParent(container);
            _mBulletPool.Enqueue(bullet);
        }
    }
}