using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FeedbackPopup : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Text text;
        [SerializeField] private Image image;
        
        private IEnumerator Show(string message, string icon, Color color)
        {
            text.text = message;
            image.sprite = Resources.Load<Sprite>($"Icons/{icon}");
            text.color = color;
            // Show the feedback popup
            panel.SetActive(true);
            // Hide the feedback popup after 1 second
            yield return new WaitForSeconds(1);
            panel.SetActive(false);
        }

        public void ShowMineAsCleared()
        {
            StartCoroutine(Show("+1", "clear_bomb", Color.green));
        }
        
        public void ShowHealthLost(float healthLost)
        {
            StartCoroutine(Show("-" + healthLost, "heart", Color.red));
        }
    }
}
