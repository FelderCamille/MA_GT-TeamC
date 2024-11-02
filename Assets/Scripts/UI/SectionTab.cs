using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    
    public class SectionTab : MonoBehaviour
    {
        public Text title;
        public Button button;
        
        private int _index;
        
        public string Text => title.text;

        public int Index => _index;
    
        public void Init(string text, int index, Action<SectionTab> onClickCallback)
        {
            title.text = text;
            _index = index;
            button.onClick.AddListener(() => onClickCallback(this));
        }
    }

}


