using JetBrains.Annotations;
using UnityEngine;

namespace ShootEmUp
{
    [UsedImplicitly]
    public class PlayerService
    {
        public GameObject Player { get; private set; }

        public PlayerService(GameObject player)
        {
            Player = player;
        }
    }
}