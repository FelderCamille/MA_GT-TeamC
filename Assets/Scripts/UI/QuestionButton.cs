using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestionButton : MonoBehaviour
    {
        public Button button;
        public Text buttonText;
        public GameObject correctBorder;
        public GameObject wrongBorder;
        
        private const int NumberOfBlinks = 2;
        
        public void Init(string text, IEnumerator onClickCallback)
        {
            buttonText.text = text;
            button.onClick.AddListener(() => StartCoroutine(onClickCallback));
        }

        public IEnumerator ShowResult(bool isCorrectResponse)
        {
            if (isCorrectResponse)
            {
                yield return BlinkBorder(correctBorder);
            }
            else
            {
                yield return ShowBorder(wrongBorder);
            }
        }

        private IEnumerator BlinkBorder(GameObject border)
        {
            // Blink the border
            for (var i = 0; i < NumberOfBlinks; i++)
            {
                border.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                border.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
            // Show the border once more
            border.SetActive(true);
            yield return new WaitForSeconds(1);
            // Disable it again
            border.SetActive(false);
        }

        private IEnumerator ShowBorder(GameObject border)
        {
            border.SetActive(true);
            yield return new WaitForSeconds(1);
            border.SetActive(false);
        }
    }
}