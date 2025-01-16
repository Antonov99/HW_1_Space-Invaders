using System;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Planets
{
    public class PlanetView:MonoBehaviour
    {
        public event Action OnClick;
        public event Action OnHold;
            
        [SerializeField]
        private GameObject _lock;

        [SerializeField]
        private GameObject _coin;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _time;

        [SerializeField]
        private SmartButton _button;

        private void OnEnable()
        {
            _button.OnClick += OnClick;
            _button.OnHold += OnHold;
        }

        private void OnDisable()
        {
            _button.OnClick -=  OnClick;
            _button.OnHold -= OnHold;
        }

        public void Unlock(bool value)
        {
            _lock.SetActive(!value);
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
            _time.text = text;
        }
    }
}