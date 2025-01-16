using Game.UI.PlanetsCatalog;
using JetBrains.Annotations;
using Modules.Planets;
using Zenject;

namespace Game.UI.Planets
{
    [UsedImplicitly]
    public class PlanetInstaller:Installer<PlanetView[], PlanetInstaller>
    {
        [Inject]
        private PlanetView[] _planetViews;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory<Planet, PlanetView, PlanetPresenter, PlanetPresenter.Factory>()
                .AsSingle();

            Container.Bind<PlanetView[]>().FromInstance(_planetViews).AsSingle();

            Container.BindInterfacesTo<CatalogPresenter>().AsSingle();
        }
    }
}