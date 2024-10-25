using System;
using Components;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace ShootEmUp
{
    [UsedImplicitly]
    public sealed class PlayerMoveController : IInitializable,IDisposable
    {
        private readonly GameObject _character;
        private MoveComponent _moveComponent;
        private readonly InputSystem _inputSystem;

        public PlayerMoveController(PlayerService playerService, InputSystem inputSystem)
        {
            _character = playerService.Player;
            _inputSystem = inputSystem;
        }
        
        void IInitializable.Initialize()
        {
            _moveComponent = _character.GetComponent<MoveComponent>();
            _inputSystem.OnMove += OnMove;
        }

        private void OnMove(Vector2 direction)
        {
            _moveComponent.OnMove(direction);
        }

        void IDisposable.Dispose()
        {
            _inputSystem.OnMove -= OnMove;
        }
    }
}