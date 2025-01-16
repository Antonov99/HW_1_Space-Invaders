using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI.Planets
{
    public class PlanetPopup:MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _name;
        
        [SerializeField]
        private TMP_Text _population;

        [SerializeField]
        private TMP_Text _level;

        [SerializeField]
        private TMP_Text _income;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Button _close;

        [SerializeField]
        private TMP_Text _price;
        
        [SerializeField]
        private Button _upgradeButton;
        
        private PlanetPopupPresenter _presenter;
        
        [Inject]
        public void Construct(PlanetPopupPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Show()
        {
            _upgradeButton.onClick.AddListener(_presenter.Upgrade);
            _close.onClick.AddListener(Hide);
            _presenter.OnStateChanged += OnStateChanged;
            OnStateChanged();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _upgradeButton.onClick.RemoveListener(_presenter.Upgrade);
            _close.onClick.RemoveListener(Hide);
            _presenter.OnStateChanged -= OnStateChanged;
            gameObject.SetActive(false);
        }
        
        private void OnStateChanged()
        {
            _name.text = _presenter.Name;
            _population.text = _presenter.Population;
            _icon.sprite = _presenter.Icon;
            _upgradeButton.interactable = _presenter.CanUpgrade();
            _price.text = _presenter.Price;
            _income.text = _presenter.Income;
            _level.text = _presenter.Level;
        }
    }
}