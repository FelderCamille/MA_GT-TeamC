using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class StoreButton : MonoBehaviour
    {
        
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Text buttonName;
        [SerializeField] private Text price;
        [SerializeField] private GameObject disabledOverlay;
        
        protected void Init(Action onClickCallback, string text, int money, string sprite)
        {
            button.onClick.RemoveAllListeners(); // Clean up to handle updates
            button.onClick.AddListener(() => onClickCallback());
            buttonName.text = text;
            price.text = money.ToString();
            icon.sprite = Resources.Load<Sprite>(sprite);
            Enabled();
        }
        
        public void Enabled()
        {
            button.interactable = true;
            disabledOverlay.SetActive(false);
        }

        public void Disable()
        {
            button.interactable = false;
            disabledOverlay.SetActive(true);
        }
    }
    
}
