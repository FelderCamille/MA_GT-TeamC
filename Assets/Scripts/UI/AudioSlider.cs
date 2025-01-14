using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AudioSlider : MonoBehaviour
    {
        
        public static float DefaultValue = 0.5f;
        
        [SerializeField] private Slider slider;
        
        public void Init(Action<float> onClickCallback, string key)
        {
            slider.onValueChanged.RemoveAllListeners(); // Clean up to handle updates
            slider.value = PlayerPrefs.GetFloat(key, DefaultValue);
            slider.onValueChanged.AddListener((value) => onClickCallback(value));
        }
    }
}
