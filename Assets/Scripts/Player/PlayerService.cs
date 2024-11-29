using JetBrains.Annotations;
using Modules;

namespace Player
{
    [UsedImplicitly]
    public class PlayerService
    {
        public ISnake Player { get; private set; }

        public PlayerService(ISnake player)
        {
            Player = player;
        }
    }
}