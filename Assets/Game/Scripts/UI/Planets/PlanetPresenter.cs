﻿using System;
using JetBrains.Annotations;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.UI.Planets
{
    [UsedImplicitly]
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
            _planetView.OnClick += OnClick;
            _planetView.OnHold += OnHold;
            _planet.OnUnlocked += OnUnlock;
            _planet.OnIncomeReady += OnReady;
            _planet.OnIncomeTimeChanged += OnTimeChanged;

            Setup();
        }

        private void Setup()
        {
            _planetView.SetIcon(_planet.GetIcon(_planet.IsUnlocked));
            _planetView.ShowCoin(_planet.IsIncomeReady);
            _planetView.Unlock(_planet.IsUnlocked);
            _planetView.UpdatePrice(_planet.Price.ToString());
        }

        private void OnClick()
        {
            if (!_planet.IsUnlocked) _planet.Unlock();
            else if (_planet.IsIncomeReady) _planet.GatherIncome();
        }

        private void OnHold()
        {
            _planetPopupShower.Show(_planet);
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
            _planetView.OnClick -= OnClick;
            _planetView.OnHold -= OnHold;
            _planet.OnUnlocked -= OnUnlock;
            _planet.OnIncomeReady -= OnReady;
            _planet.OnIncomeTimeChanged -= OnTimeChanged;
        }

        public sealed class Factory : PlaceholderFactory<Planet, PlanetView, PlanetPresenter>
        {
        }
    }
}