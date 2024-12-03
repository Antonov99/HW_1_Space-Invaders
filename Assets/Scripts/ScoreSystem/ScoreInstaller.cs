using JetBrains.Annotations;
using Modules;
using Zenject;

namespace ScoreSystem
{
    [UsedImplicitly]
    public class ScoreInstaller:Installer<ScoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IScore>().To<Score>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScorePresenter>().AsSingle();
        }
    }
}