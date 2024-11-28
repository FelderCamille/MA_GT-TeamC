using UnityEngine;

namespace Controllers
{
    public class EncyclopediaOpenCloseController : MonoBehaviour
    {

        [Header("Content")]
        public EncyclopediaController encyclopedia;
        
        private void Update()
        {
            HandleOpening();
        }

        private void HandleOpening()
        {
            if (Input.GetKeyDown(Constants.Actions.OpenCloseEncyclopedia))
            {
                if (encyclopedia.IsOpened)
                {
                    encyclopedia.gameObject.SetActive(false);
                }
                else
                {
                    encyclopedia.gameObject.SetActive(true);
                }
            }
        }
    }
}
