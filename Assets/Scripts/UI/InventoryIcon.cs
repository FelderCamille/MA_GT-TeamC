using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryIcon : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Image icon;
        [SerializeField] protected Text number;
        
        protected void Init(string sprite, float value, Color? panelColor)
        {
            icon.sprite = Resources.Load<Sprite>(sprite);
            if (panelColor != null) panel.GetComponent<Image>().color = (Color) panelColor;
            number.gameObject.SetActive(true);
            number.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
    
}