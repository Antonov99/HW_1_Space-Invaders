using System;
using JetBrains.Annotations;
using Modules;
using Zenject;

namespace Player
{
    [UsedImplicitly]
    public class PlayerSpeedController : IInitializable, IDisposable
    {
        private readonly ISnake _player;
        private readonly IDifficulty _difficulty;

        public PlayerSpeedController(ISnake player, IDifficulty difficulty)
        {
            _player = player;
            _difficulty = difficulty;
        }

        void IInitializable.Initialize()
        {
            _difficulty.OnStateChanged += SetSpeed;
        }

        private void SetSpeed()
        {
            _player.SetSpeed(_difficulty.Current);
        }

        void IDisposable.Dispose()
        {
            _difficulty.OnStateChanged -= SetSpeed;
        }
    }
}