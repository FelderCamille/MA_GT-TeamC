using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomButton : MonoBehaviour
    {

        [SerializeField] private Button button;

        private bool _isEnabled = true;
        
        public void Init(Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback());
        }
        
        public void Disable()
        {
            if (!_isEnabled) return;
            button.interactable = false;
            _isEnabled = false;
        }

        public void Enable()
        {
            if (_isEnabled) return;
            button.interactable = true;
            _isEnabled = true;
        }
    }
}
