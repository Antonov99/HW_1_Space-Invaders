using System;
using JetBrains.Annotations;
using Modules.Planets;
using UnityEngine;

namespace Game.UI.Planets
{
    [UsedImplicitly]
    public class PlanetPopupPresenter
    {
        public event Action OnStateChanged;
        
        public string Name => _planet.Name;
        public string Population => _planet.Population.ToString();
        public Sprite Icon => _planet.GetIcon(_planet.IsUnlocked);
        public string Price => _planet.Price.ToString();
        public string Level => $"{_planet.Level} / {_planet.MaxLevel}";
        public string Income => $"{_planet.MinuteIncome} / min";

        private IPlanet _planet;

        private readonly IMoneyAdapter _moneyAdapter;

        public PlanetPopupPresenter(IMoneyAdapter moneyAdapter)
        {
            _moneyAdapter = moneyAdapter;
        }

        public void ChangePlanet(IPlanet planet)
        {
            if (planet != _planet)
            {
                _planet = planet;
                OnStateChanged?.Invoke();
            }
        }

        public bool CanUpgrade()
        {
            return _moneyAdapter.IsEnough(_planet.Price);
        }

        public void Upgrade()
        {
            
        }
    }
}