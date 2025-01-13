using UnityEngine;

namespace Controllers
{
    public class EncyclopediaOpenCloseController : MonoBehaviour
    {

        [Header("Content")]
        [SerializeField] private EncyclopediaController encyclopedia;
        
        private void Update()
        {
            HandleOpening();
        }

        private void HandleOpening()
        {
            if (!Input.GetKeyDown(Constants.Actions.OpenCloseEncyclopedia)) return;
            encyclopedia.gameObject.SetActive(!encyclopedia.IsOpened);
        }
    }
}
