using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI
{
    public class StoreButton : MonoBehaviour
    {
        public Button button;
        public Image icon;
        public Text buttonName;
        public Text price;
        
        public void Init(Action onClickCallback, string text, int money, string sprite)
        {
            button.onClick.AddListener(() => onClickCallback());
            buttonName.text = text;
            price.text = money.ToString();
            icon.sprite = Resources.Load<Sprite>(sprite);
        }

        public void InitRepairButton(Action onClickCallback)
        {
            Init(onClickCallback, "Réparer", Constants.Prices.Repair, "Icons/repair");
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
