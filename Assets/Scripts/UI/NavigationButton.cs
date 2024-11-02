using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NavigationButton : MonoBehaviour
    {
        public Button button;

        private bool _isEnabled;
        
        public bool isEnabled => _isEnabled;
        
        public void Init(bool enable, Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback());
            if (enable)
            {
                _isEnabled = false;
                Enable();
            }
            else
            {
                _isEnabled = true;
                Disable();
            }
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