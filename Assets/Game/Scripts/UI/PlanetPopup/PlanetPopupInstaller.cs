using JetBrains.Annotations;
using Zenject;

namespace Game.UI.Planets
{
    [UsedImplicitly]
    public sealed class PlanetPopupInstaller : Installer<PlanetPopupView, PlanetPopupInstaller>
    {
        [Inject]
        private PlanetPopupView _popupView;
        
        public override void InstallBindings()
        {
            this.Container
                .Bind<PlanetPopupView>()
                .FromInstance(_popupView)
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