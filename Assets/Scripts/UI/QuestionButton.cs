using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestionButton : MonoBehaviour
    {
        public Button Button;
        public Text ButtonText;
        
        public void Init(string text)
        {
            ButtonText.text = text;
        }
    }
}