using System;
using Modules.Planets;
using Zenject;

namespace Game.UI.Planets
{
    public class PlanetPresenter : IInitializable, IDisposable
    {
        private readonly IPlanet _planet;
        private readonly PlanetView _planetView;
        private readonly PlanetPopupShower _planetPopupShower;

        public PlanetPresenter(IPlanet planet, PlanetView planetView, PlanetPopupShower planetPopupShower)
        {
            _planet = planet;
            _planetView = planetView;
            _planetPopupShower = planetPopupShower;
        }

        public void Initialize()
        {
            _planetView.OnClicked += OnClick;
            _planet.OnUnlocked += OnUnlock;
            _planet.OnIncomeReady += OnReady;
            _planet.OnIncomeTimeChanged += OnTimeChanged;
        }

        private void OnClick()
        {
            if (!_planet.IsUnlocked) _planet.Unlock();
            else if (_planet.IsIncomeReady) _planet.GatherIncome();
            else _planetPopupShower.Show(_planet);
        }

        private void OnUnlock()
        {
            _planetView.Unlock(true);
            _planetView.SetIcon(_planet.GetIcon(true));
        }

        private void OnReady(bool value)
        {
            _planetView.ShowCoin(value);
        }
        
        private void OnTimeChanged(float time)
        {
            _planetView.UpdateTime(time.ToString());
        }

        public void Dispose()
        {
            _planetView.OnClicked -= OnClick;
            _planet.OnUnlocked -= OnUnlock;
            _planet.OnIncomeReady -= OnReady;
            _planet.OnIncomeTimeChanged -= OnTimeChanged;
        }
    }
}