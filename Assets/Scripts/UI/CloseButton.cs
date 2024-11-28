using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CloseButton : MonoBehaviour
    {
        public Button button;
        
        public void Init(Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback());
        }
    }
}
