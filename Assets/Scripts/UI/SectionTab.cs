using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    
    public class SectionTab : MonoBehaviour
    {
        [SerializeField] private Text title;
        [SerializeField] private Button button;

        public string Text => title.text;

        public int Index { get; private set; }

        public void Init(string text, int index, Action<SectionTab> onClickCallback)
        {
            title.text = text;
            Index = index;
            button.onClick.AddListener(() => onClickCallback(this));
        }
    }

}


