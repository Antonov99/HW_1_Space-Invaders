using System;
using Components;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace ShootEmUp
{
    [UsedImplicitly]
    public class PlayerAttackController:IInitializable,IDisposable
    {
        private readonly InputSystem _inputSystem;
        private readonly GameObject _character;
        private readonly BulletSystem _bulletSystem;
        private readonly BulletConfig _bulletConfig;
        private WeaponComponent _weapon;
        
        public PlayerAttackController(
            InputSystem inputSystem, 
            PlayerService playerService, 
            BulletSystem bulletSystem, 
            BulletConfig bulletConfig)
        {
            _inputSystem = inputSystem;
            _character = playerService.Player;
            _bulletSystem = bulletSystem;
            _bulletConfig = bulletConfig;
        }

        void IInitializable.Initialize()
        {
            _inputSystem.OnFire += OnFire;
            _weapon = _character.GetComponent<WeaponComponent>();
        }

        private void OnFire()
        {
            _bulletSystem.SpawnBullet(new BulletArgs
            {
                position = _weapon.Position,
                velocity = _weapon.Rotation * Vector3.up * _bulletConfig.speed,
                color = _bulletConfig.color,
                physicsLayer = (int)_bulletConfig.physicsLayer,
                damage = _bulletConfig.damage,
                isPlayer = true
            });
        }

        void IDisposable.Dispose()
        {
            _inputSystem.OnFire -= OnFire;
        }
    }
}