using JetBrains.Annotations;
using Modules.Planets;

namespace Game.UI.Planets
{
    [UsedImplicitly]
    public sealed class PlanetPopupShower
    {
        private readonly PlanetPopupPresenter _planetPresenter;
        private readonly PlanetPopupView _planetPopupView;

        public PlanetPopupShower(PlanetPopupPresenter planetPresenter, PlanetPopupView planetPopupView)
        {
            _planetPresenter = planetPresenter;
            _planetPopupView = planetPopupView;
        }

        public void Show(IPlanet planet)
        {
            _planetPresenter.ChangePlanet(planet);
            _planetPopupView.Show();
        }
    }
}