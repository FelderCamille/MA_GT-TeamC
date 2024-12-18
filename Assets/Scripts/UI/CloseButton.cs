using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        public void Init(Action onClickCallback)
        {
            button.onClick.AddListener(() => onClickCallback());
        }
    }
}
