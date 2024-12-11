using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    [UsedImplicitly]
    public class UiInstaller:MonoInstaller
    {
        [SerializeField]
        private MoneyView _moneyView;
        
        public override void InstallBindings()
        {
            Container.Bind<IMoneyView>().To<MoneyView>().FromInstance(_moneyView);
            Container.BindInterfacesAndSelfTo<MoneyPresenter>().AsSingle().NonLazy();
        }
    }
}