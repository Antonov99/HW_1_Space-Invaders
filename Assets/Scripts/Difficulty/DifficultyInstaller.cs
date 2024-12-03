using Modules;
using Zenject;

namespace DifficultySystem
{
    public class DifficultyInstaller:Installer<DifficultyInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DifficultyPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DifficultyController>().AsSingle().NonLazy();
            Container.Bind<IDifficulty>().To<Difficulty>().AsSingle().WithArguments(9);
        }
    }
}