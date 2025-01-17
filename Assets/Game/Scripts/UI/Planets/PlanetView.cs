using System;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Planets
{
    public class PlanetView : MonoBehaviour
    {
        public event Action OnClick
        {
            add { _button.OnClick += value; }
            remove { _button.OnClick -= value;  }
        }

        public event Action OnHold
        {
            add { _button.OnHold += value; }
            remove { _button.OnHold -= value; }
        }

        [SerializeField]
        private GameObject _lock;

        [SerializeField]
        private GameObject _coin;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private GameObject _timeObject;

        [SerializeField]
        private TMP_Text _timeText;

        [SerializeField]
        private SmartButton _button;

        [SerializeField]
        private TMP_Text _price;

        public void Unlock(bool value)
        {
            _lock.SetActive(!value);
            _timeObject.SetActive(value);
        }

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public void ShowCoin(bool value)
        {
            _coin.SetActive(value);
        }

        public void UpdateTime(string text)
        {
            _timeText.text = text;
        }

        public void UpdatePrice(string text)
        {
            _price.text = text;
        }
    }
}