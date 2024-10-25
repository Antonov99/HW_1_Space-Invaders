using System.Collections.Generic;
using JetBrains.Annotations;
using Zenject;

namespace ShootEmUp
{
    [UsedImplicitly]
    public class BulletSystem:ITickable
    {
        private readonly LevelBounds _levelBounds;
        private readonly BulletPool _bulletPool;

        private readonly HashSet<Bullet> _mActiveBullets = new();
        private readonly List<Bullet> _mCache = new();

        public BulletSystem(LevelBounds levelBounds, BulletPool bulletPool)
        {
            _levelBounds = levelBounds;
            _bulletPool = bulletPool;
        }

        void ITickable.Tick()
        {
            _mCache.Clear();
            _mCache.AddRange(_mActiveBullets);

            for (int i = 0, count = _mCache.Count; i < count; i++)
            {
                Bullet bullet = _mCache[i];
                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    RemoveBullet(bullet);
                }
            }
        }
        
        public void SpawnBullet(BulletArgs args)
        {
            var bullet = _bulletPool.SpawnBullet();

            bullet.transform.position = args.position;
            bullet.rigidbody2D.velocity = args.velocity;
            bullet.spriteRenderer.color = args.color;
            bullet.gameObject.layer = args.physicsLayer;
            bullet.damage = args.damage;
            bullet.isPlayer = args.isPlayer;

            if (_mActiveBullets.Add(bullet))
            {
                bullet.OnCollisionEntered += RemoveBullet;
            }
        }

        private void RemoveBullet(Bullet bullet)
        {
            if (_mActiveBullets.Remove(bullet))
            {
                bullet.OnCollisionEntered -= RemoveBullet;
                _bulletPool.RemoveBullet(bullet);
            }
        }
    }
}

