using System;
using System.Collections.Generic;
using System.Linq;
using Game.UI.Planets;
using JetBrains.Annotations;
using Modules.Planets;
using Zenject;

namespace Game.UI.PlanetsCatalog
{
    [UsedImplicitly]
    public class CatalogPresenter : IInitializable, IDisposable
    {
        private readonly List<Planet> _planets;
        private readonly PlanetPresenter.Factory _factory;
        private readonly PlanetView[] _planetViews;

        private readonly Dictionary<Planet, PlanetPresenter> _presenters = new();

        public CatalogPresenter(IEnumerable<Planet> planets, PlanetPresenter.Factory factory, PlanetView[] planetViews)
        {
            _planets = planets.ToList();
            _factory = factory;
            _planetViews = planetViews;
        }

        public void Initialize()
        {
            for (int i = 0; i < _planets.Count(); i++)
            {
                var planetPresenter = _factory.Create(_planets[i], _planetViews[i]);
                planetPresenter.Initialize();
                _presenters.Add(_planets[i], planetPresenter);
            }
        }

        public void Dispose()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Value.Dispose();
            }
        }
    }
}