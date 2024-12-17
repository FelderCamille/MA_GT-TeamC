using UnityEngine;
using UnityEngine.UI;
using System;
using Objects;

namespace UI
{
    public class StoreButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Text buttonName;
        [SerializeField] private Text price;
        
        protected void Init(Action onClickCallback, string text, int money, string sprite)
        {
            button.onClick.AddListener(() => onClickCallback());
            buttonName.text = text;
            price.text = money.ToString();
            icon.sprite = Resources.Load<Sprite>(sprite);
        }
        
        public void Enabled()
        {
            button.interactable = true;
        }

        public void Disable()
        {
            button.interactable = false;
        }
    }
    
}
