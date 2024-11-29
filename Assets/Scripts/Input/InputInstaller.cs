using JetBrains.Annotations;
using Zenject;

namespace InputSystem
{
    [UsedImplicitly]
    public class InputInstaller:Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputAdapter>().AsSingle();
        }
    }
}