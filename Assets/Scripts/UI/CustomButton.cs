using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomButton : MonoBehaviour
    {

        public Button button;
        
        public void Init(Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback());
        }
    }
}
