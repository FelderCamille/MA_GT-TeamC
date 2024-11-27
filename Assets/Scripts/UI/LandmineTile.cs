using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class LandmineTile : Tile
    {
        private const int ShowTimeInSec = 1;
        private const float ShowAlpha = 0.7f;

        public GameObject landmine;
        private Material _material;
        private bool _isShown;
        

        private void Awake()
        {
            _material = landmine.GetComponent<Renderer>().material;
            Hide();
        }

        public void Show()
        {
            if (_isShown) return;
            _isShown = true;
            ChangeAlpha(ShowAlpha);
            StartCoroutine(FadeAfterSeconds(ShowTimeInSec));
        }

        private void Hide()
        {
            ChangeAlpha(0f);
        }
        
        private IEnumerator FadeAfterSeconds(float seconds)
        {
            // Wait for the given seconds
            yield return new WaitForSeconds(seconds);
            // Decrease linearly the alpha value to 0
            for (var i = ShowAlpha; i >= 0; i -= 0.01f)
            {
                ChangeAlpha(i);
                yield return new WaitForSeconds(0.01f);
            }
            // Enable to show the landmine again
            _isShown = false;
        }
        
        private void ChangeAlpha(float alphaVal)
        {
            var oldColor = _material.color;
            var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
            _material.SetColor("_Color", newColor);

        }
    }
}