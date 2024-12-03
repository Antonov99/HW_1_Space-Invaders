using JetBrains.Annotations;
using Modules;
using Zenject;

namespace DifficultySystem
{
    [UsedImplicitly]
    public class DifficultyInstaller : Installer<int, DifficultyInstaller>
    {
        [Inject]
        private int _difficulty;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DifficultyPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DifficultyController>().AsSingle().NonLazy();
            Container.Bind<IDifficulty>().To<Difficulty>().AsSingle().WithArguments(_difficulty);
        }
    }
}