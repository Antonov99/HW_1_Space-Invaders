using System.Collections.Generic;
using Modules.Planets;
using Zenject;

namespace Game.UI.PlanetsCatalog
{
    public class CatalogPresenter:IInitializable
    {
        private IEnumerable<Planet> _planets;

        public CatalogPresenter(IEnumerable<Planet> planets)
        {
            _planets = planets;
        }

        public void Initialize()
        {
            
        }
    }
}