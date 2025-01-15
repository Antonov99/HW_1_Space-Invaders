using JetBrains.Annotations;
using Modules.Money;
using Zenject;

namespace Game.Gameplay
{
    [UsedImplicitly]
    public sealed class MoneyInstaller : Installer<int, MoneyInstaller>
    {
        [Inject]
        private int _initialMoney;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<MoneyStorage>()
                .AsSingle()
                .WithArguments(_initialMoney)
                .NonLazy();

            Container
                .BindInterfacesTo<MoneyAdapter>()
                .AsSingle()
                .NonLazy();
        }
    }
}