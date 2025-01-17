using System;
using JetBrains.Annotations;
using Modules.Money;
using Zenject;

namespace Game.UI
{
    [UsedImplicitly]
    public class MoneyPresenter:IInitializable, IDisposable
    {
        private readonly IMoneyView _moneyView;
        private readonly IMoneyStorage _moneyStorage;

        public MoneyPresenter(IMoneyView moneyView, IMoneyStorage moneyStorage)
        {
            _moneyView = moneyView;
            _moneyStorage = moneyStorage;
        }

        public void Initialize()
        {
            _moneyStorage.OnMoneyChanged += Update;
            Update(_moneyStorage.Money, 0);
        }

        private void Update(int newValue, int previousValue)
        {
            var text = newValue.ToString();
            _moneyView.UpdateMoney(text);
        }

        public void Dispose()
        {
            _moneyStorage.OnMoneyChanged -= Update;
        }
    }
}