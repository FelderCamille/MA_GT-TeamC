using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        
        public void Init(Action<float> onClickCallback, string key)
        {
            slider.onValueChanged.RemoveAllListeners(); // Clean up to handle updates
            slider.value = PlayerPrefs.GetFloat(key, 1f);
            slider.onValueChanged.AddListener((value) => onClickCallback((float) Math.Log10(value) * 20));
        }
    }
}
