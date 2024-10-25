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
        private readonly LevelBounds _levelBounds;

        public PlayerMoveController(PlayerService playerService, InputSystem inputSystem, LevelBounds levelBounds)
        {
            _character = playerService.Player;
            _inputSystem = inputSystem;
            _levelBounds = levelBounds;
        }
        
        void IInitializable.Initialize()
        {
            _moveComponent = _character.GetComponent<MoveComponent>();
            _inputSystem.OnMove += OnMove;
        }

        private void OnMove(Vector2 direction)
        {
            if(!_levelBounds.InBounds(_character.transform.position)) _moveComponent.OnMove(-direction);;
            _moveComponent.OnMove(direction);
        }

        void IDisposable.Dispose()
        {
            _inputSystem.OnMove -= OnMove;
        }
    }
}