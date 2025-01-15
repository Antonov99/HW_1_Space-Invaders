using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI.Planets
{
    public class PlanetView:MonoBehaviour
    {
        public event UnityAction OnClicked
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }
            
        [SerializeField]
        private GameObject _lock;

        [SerializeField]
        private GameObject _coin;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshPro _time;

        [SerializeField]
        private Button _button;
        
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