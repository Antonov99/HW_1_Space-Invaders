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

        public PlayerMoveController(ISnake playerService, InputAdapter inputAdapter)
        {
            _player = playerService;
            _inputAdapter = inputAdapter;
        }

        void IInitializable.Initialize()
        {
            _inputAdapter.OnMove +=  _player.Turn;
        }

        void IDisposable.Dispose()
        {
            _inputAdapter.OnMove -=  _player.Turn;
        }
    }
}