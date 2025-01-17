using Game.UI.Planets;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.UI
{
    [UsedImplicitly]
    public class UiInstaller:MonoInstaller
    {
        [SerializeField]
        private PlanetView[] _planetViews;
        
        [SerializeField]
        private MoneyView _moneyView;

        [FormerlySerializedAs("_popup")]
        [SerializeField]
        private PlanetPopupView _popupView;
        
        public override void InstallBindings()
        {
            PlanetInstaller.Install(Container, _planetViews);
            MoneyInstaller.Install(Container,_moneyView);
            PlanetPopupInstaller.Install(Container,_popupView);
        }
    }
}