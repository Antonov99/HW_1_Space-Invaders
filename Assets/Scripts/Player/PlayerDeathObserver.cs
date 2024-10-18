using Components;
using JetBrains.Annotations;
using UnityEngine;

namespace ShootEmUp
{
    [UsedImplicitly]
    public class PlayerDeathObserver
    {
        public PlayerDeathObserver(PlayerService playerService)
        {
            playerService.Player.GetComponent<HealthComponent>().OnHpEmpty += GameOver;
        }

        private void GameOver(GameObject _)
        {
            Time.timeScale = 0;
        }
    }
}