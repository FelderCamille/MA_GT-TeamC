using System.Collections;
using UnityEngine;

namespace UI
{
    public class LandmineTile : Tile
    {
        private const float TimeBeforeShowingStartInSec = 0.1f;
        private const float TimeBeforeFadingStartInSec = 0.6f;
        private const float TimeBeforeFadingInSec = 1;
        private const float ShowAlpha = 0.7f;

        public GameObject landmine;
        private Material _material;
        private bool _isShown;

        private void Awake()
        {
            _material = landmine.GetComponent<Renderer>().material;
            Hide();
            StartCoroutine(ShowingThenFadeAfterSeconds(TimeBeforeShowingStartInSec, TimeBeforeFadingStartInSec));
        }

        public void Show()
        {
            if (_isShown) return;
            StartCoroutine(ShowingThenFadeAfterSeconds(0f, TimeBeforeFadingInSec));
        }

        private void Hide()
        {
            ChangeAlpha(0f);
        }
        
        private IEnumerator ShowingThenFadeAfterSeconds(float showingSeconds, float fadingSeconds)
        {
            // Set the landmine as shown
            _isShown = true;
            // Wait for the given seconds before showing
            yield return new WaitForSeconds(showingSeconds);
            // Increase linearly the alpha value to ShowAlpha
            for (var i = 0f; i <= ShowAlpha; i += 0.01f)
            {
                ChangeAlpha(i);
                yield return new WaitForSeconds(0.01f);
            }
            // Wait for the given seconds before fading
            yield return new WaitForSeconds(fadingSeconds);
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