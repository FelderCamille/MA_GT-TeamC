using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuestionButton : MonoBehaviour
    {
        public Button button;
        public Text buttonText;
        
        public void Init(string text, Action<QuestionButton> onClickCallback)
        {
            buttonText.text = text;
            button.onClick.AddListener(() => onClickCallback(this));
        }
    }
}