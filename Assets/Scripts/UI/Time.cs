using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Time : MonoBehaviour
    {
        [SerializeField] private Text time;

        public void SetTime(float value)
        {
            var timeSpan = TimeSpan.FromSeconds(value);
            time.text = timeSpan.ToString(@"mm\:ss");
        }
    }
}