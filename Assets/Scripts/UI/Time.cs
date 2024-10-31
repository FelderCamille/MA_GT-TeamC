using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Time : MonoBehaviour
    {
        public Text time;

        public void SetTime(float value)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(value);
            time.text = timeSpan.ToString(@"mm\:ss");
        }
    }
}