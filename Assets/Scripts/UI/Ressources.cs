using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Ressources : MonoBehaviour
    {
        public Text money;
        public Text health;
        public Text clearedMines;

        public void SetMoney(int value)
        {
            money.text = value.ToString();
        }

        public void SetHealth(float value)
        {
            health.text = value.ToString();
        }

        public void SetMines(int value)
        {
            clearedMines.text = value.ToString();
        }
    }
}