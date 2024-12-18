using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartButton : CustomButton
    {

        [SerializeField] private Text text;
        [SerializeField] private GameObject loadingIndicator;

        private void Awake()
        {
            text.gameObject.SetActive(true);
            loadingIndicator.SetActive(false);
        }

        public new void Disable()
        {
            base.Disable();
            text.gameObject.SetActive(false);
            loadingIndicator.SetActive(true);
        }

        public new void Enable()
        {
            base.Enable();
            text.gameObject.SetActive(true);
            loadingIndicator.SetActive(false);
        }
    }
}
