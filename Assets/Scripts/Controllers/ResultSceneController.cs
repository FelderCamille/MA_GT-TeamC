using Core;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class ResultSceneController : MonoBehaviour
    {
        [SerializeField] private PlayerResult player1Result;
        [SerializeField] private PlayerResult player2Result;
        [SerializeField] private CustomButton quitButton;
        
        private SceneLoader _sceneLoader;

        public void Start()
        {
            // Get scene loader
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            // Manage the case where no player is connected (should not happen)
            if (NetworkManager.Singleton.ConnectedClients.Count <= 0)
            {
                Debug.LogError("No connected clients");
                QuitGame();
                return;
            }
            // Retrieve the data for player 1
            var player1 = NetworkManager.Singleton.ConnectedClients[0].PlayerObject;
            var player1Resources = player1.GetComponent<ResourcesManager>();
            // Retrieve the data for player 2
            if (NetworkManager.Singleton.ConnectedClients.Count <= 1)
            {
                player1Result.Init(player1.OwnerClientId, player1.IsOwner, player1Resources, null);
                player2Result.gameObject.SetActive(false);
            }
            else
            {
                var player2 = NetworkManager.Singleton.ConnectedClients[1].PlayerObject;
                var player2Resources = player2.GetComponent<ResourcesManager>();
                player1Result.Init(player1.OwnerClientId, player1.IsOwner, player1Resources, player2Resources);
                player2Result.Init(player2.OwnerClientId, player2.IsOwner, player1Resources, player2Resources);
            }
            // Initialize the quit button
            quitButton.Init(QuitGame);
        }

        private void QuitGame()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Title);
            NetworkManager.Singleton.Shutdown();
        }
    }
}