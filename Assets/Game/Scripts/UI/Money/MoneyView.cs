using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class MoneyView:MonoBehaviour, IMoneyView
    {
        [SerializeField]
        private TMP_Text _money;

        public void UpdateMoney(string money)
        {
            _money.text = money;
        }
    }
}