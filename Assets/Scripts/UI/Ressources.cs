using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Ressources : MonoBehaviour
    {
        [SerializeField] private Text money;
        [SerializeField] private Text health;
        [SerializeField] private Text clearedMines;
        [SerializeField] private Text placedMines;

        public void SetMoney(int value)
        {
            money.text = value.ToString();
        }

        public void SetHealth(float value)
        {
            health.text = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetClearedMines(int value)
        {
            clearedMines.text = value.ToString();
        }
        
        public void SetPlacedMines(int value)
        {
            placedMines.text = value.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}