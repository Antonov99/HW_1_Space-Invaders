using JetBrains.Annotations;
using Zenject;

namespace Game.UI
{
    [UsedImplicitly]
    public class MoneyInstaller:Installer<MoneyView, MoneyInstaller>
    {
        [Inject]
        private MoneyView _moneyView;

        public override void InstallBindings()
        {
            Container.Bind<IMoneyView>().To<MoneyView>().FromInstance(_moneyView);
            Container.BindInterfacesAndSelfTo<MoneyPresenter>().AsSingle().NonLazy();
        }
    }
}