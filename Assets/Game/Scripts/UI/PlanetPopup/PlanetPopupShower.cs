using Modules.Planets;

namespace Game.UI.Planets
{
    public sealed class PlanetPopupShower
    {
        private readonly PlanetPopupPresenter _planetPresenter;
        private readonly PlanetPopup _planetPopup;

        public PlanetPopupShower(PlanetPopupPresenter planetPresenter, PlanetPopup planetPopup)
        {
            _planetPresenter = planetPresenter;
            _planetPopup = planetPopup;
        }

        public void Show(IPlanet planet)
        {
            _planetPresenter.ChangePlanet(planet);
            _planetPopup.Show();
        }
    }
}