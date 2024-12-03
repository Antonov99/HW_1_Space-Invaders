using System;
using InputSystem;
using JetBrains.Annotations;
using Modules;
using Zenject;

namespace Player
{
    [UsedImplicitly]
    public sealed class PlayerMoveController : IInitializable, IDisposable
    {
        private readonly ISnake _player;
        private readonly InputAdapter _inputAdapter;

        public PlayerMoveController(PlayerService playerService, InputAdapter inputAdapter)
        {
            _player = playerService.Player;
            _inputAdapter = inputAdapter;
        }

        void IInitializable.Initialize()
        {
            _inputAdapter.OnMove += OnMove;
        }

        private void OnMove(SnakeDirection direction)
        {
            _player.Turn(direction);
        }

        void IDisposable.Dispose()
        {
            _inputAdapter.OnMove -= OnMove;
        }
    }
}