using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryIcon : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Image icon;
        [SerializeField] protected Text number;
        
        protected void InitWithoutNumber(string sprite)
        {
            icon.sprite = Resources.Load<Sprite>(sprite);
            number.gameObject.SetActive(false);
        }
        
        protected void InitWithNumber(string sprite, Color? panelColor)
        {
            icon.sprite = Resources.Load<Sprite>(sprite);
            if (panelColor != null) panel.GetComponent<Image>().color = (Color) panelColor;
            number.gameObject.SetActive(true);
            number.text = "0";
        }
    }
    
}