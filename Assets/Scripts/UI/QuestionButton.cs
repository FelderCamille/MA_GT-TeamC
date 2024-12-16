using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestionButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Text buttonText;
        [SerializeField] private GameObject correctBorder;
        [SerializeField] private GameObject wrongBorder;
        
        private const int NumberOfBlinks = 2;
        
        public void Init(string text, IEnumerator onClickCallback)
        {
            buttonText.text = text;
            button.onClick.AddListener(() => StartCoroutine(onClickCallback));
        }
        
        public string GetText() => buttonText.text;

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