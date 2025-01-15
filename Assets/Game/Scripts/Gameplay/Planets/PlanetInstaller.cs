using JetBrains.Annotations;
using Modules.Planets;
using Zenject;

namespace Game.Gameplay
{
    [UsedImplicitly]
    public sealed class PlanetInstaller : Installer<PlanetCatalog, PlanetInstaller>
    {
        [Inject]
        private PlanetCatalog _catalog;

        public override void InstallBindings()
        {
            foreach (PlanetConfig config in _catalog)
            {
                Container
                    .BindInterfacesAndSelfTo<Planet>()
                    .AsCached()
                    .WithArguments(config)
                    .NonLazy();
            }
        }
    }
}