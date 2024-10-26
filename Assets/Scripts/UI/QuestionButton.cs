using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestionButton : MonoBehaviour
    {
        public Button button;
        public Text buttonText;
        
        public void Init(string text)
        {
            buttonText.text = text;
        }
    }
}