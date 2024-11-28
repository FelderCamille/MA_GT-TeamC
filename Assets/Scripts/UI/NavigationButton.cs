using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NavigationButton : MonoBehaviour
    {
        public Button button;

        public bool IsEnabled { get; private set; }

        public void Init(bool enable, Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback());
            if (enable)
            {
                IsEnabled = false;
                Enable();
            }
            else
            {
                IsEnabled = true;
                Disable();
            }
        }
        
        public void Disable()
        {
            if (!IsEnabled) return;
            button.interactable = false;
            IsEnabled = false;
        }

        public void Enable()
        {
            if (IsEnabled) return;
            button.interactable = true;
            IsEnabled = true;
        }

    }
}