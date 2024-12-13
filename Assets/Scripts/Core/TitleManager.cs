using Unity.Netcode;
using UnityEngine;

namespace UI
{
    class TitleManager : MonoBehaviour
    {
        [SerializeField] private CustomButton hostButton;
        [SerializeField] private CustomButton joinButton;
        
        private void Start()
        {
            hostButton.Init(OnHostButtonClick);
            joinButton.Init(OnJoinButtonClick);
        }
        
        private void OnHostButtonClick()
        {
            Debug.Log("Host button clicked");
            NetworkManager.Singleton.StartHost();
        }
        
        private void OnJoinButtonClick()
        {
            Debug.Log("Join button clicked");
            NetworkManager.Singleton.StartClient();
        }

    }   
}