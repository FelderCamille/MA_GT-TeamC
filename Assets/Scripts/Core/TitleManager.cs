using UI;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    class TitleManager : MonoBehaviour
    {
        [SerializeField] private CustomButton hostButton;
        [SerializeField] private CustomButton joinButton;
        
        private void Start()
        {
            hostButton.Init(OnHostButtonClick);
            joinButton.Init(OnJoinButtonClick);
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;
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
        
        private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.CreatePlayerObject = true;
            response.Position = new Vector3(5, 1, 5); // TODO: set spawn point of player
            response.Rotation = Quaternion.identity;
            response.Approved = true;
        }

    }   
}