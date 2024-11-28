using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BonusIcon : MonoBehaviour
    {
        public Image icon;
        
        public void Init(string sprite)
        {
            icon.sprite = Resources.Load<Sprite>(sprite);
        }
    }
}