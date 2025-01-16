using JetBrains.Annotations;
using Zenject;

namespace Game.UI.Planets
{
    [UsedImplicitly]
    public sealed class PlanetPopupInstaller : Installer<PlanetPopup, PlanetPopupInstaller>
    {
        [Inject]
        private PlanetPopup _popup;
        
        public override void InstallBindings()
        {
            this.Container
                .Bind<PlanetPopup>()
                .FromInstance(_popup)
                .AsSingle()
                .NonLazy();

            this.Container
                .Bind<PlanetPopupPresenter>()
                .AsSingle();

            this.Container
                .Bind<PlanetPopupShower>()
                .AsSingle();

        }
    }
}