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
        private GameObject _character;
        private WeaponComponent _weapon;
        
        public PlayerAttackController(InputSystem inputSystem, PlayerService playerService)
        {
            _inputSystem = inputSystem;
            _character = playerService.Player;
        }

        public void Initialize()
        {
            _inputSystem.OnFire += OnFire;
            _weapon = _character.GetComponent<WeaponComponent>();
        }

        private void OnFire()
        {
            
        }

        public void Dispose()
        {
            _inputSystem.OnFire -= OnFire;
        }
    }
}