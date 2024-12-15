using System;
using System.Net;
using System.Net.Sockets;
using UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class BaseSceneController : MonoBehaviour
    {

        [SerializeField] private GameObject chooseRoleEmplacement;
        [SerializeField] private CustomButton hostButton;
        [SerializeField] private CustomButton joinButton;
        [SerializeField] private GameObject roleChoosedEmplacement;
        [SerializeField] private InputField ipAddress;
        [SerializeField] private InputField port;
        [SerializeField] private CustomButton backButton;
        [SerializeField] private CustomButton startButton;

        private SceneLoader _sceneLoader;
        
        private bool _isHost;
        
        private void Start()
        {
            // Init network connection approval callback
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            // Get objects from the scene
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            // Initialize buttons
            hostButton.Init(OnHostButtonClick);
            joinButton.Init(OnJoinButtonClick);
            backButton.Init(OnBackButtonClick);
            startButton.Init(OnStartClick);
            // Initialize input fields
            port.text = "7777";
        }
        
        private void OnHostButtonClick()
        {
            chooseRoleEmplacement.SetActive(false);
            roleChoosedEmplacement.SetActive(true);
            ipAddress.text = GetLocalIPAddress();
            ipAddress.interactable = false;
            _isHost = true;
        }
        
        private void OnJoinButtonClick()
        {
            chooseRoleEmplacement.SetActive(false);
            roleChoosedEmplacement.SetActive(true);
            ipAddress.interactable = true;
            _isHost = false;
        }

        private void OnBackButtonClick()
        {
            roleChoosedEmplacement.SetActive(false);
            chooseRoleEmplacement.SetActive(true);
            ipAddress.interactable = true;
            port.interactable = true;
            startButton.enabled = true;
        }
        
        private void OnStartClick()
        {
            // Disable input fields and start button
            ipAddress.interactable = false;
            port.interactable = false;
            startButton.enabled = false;
            // Set connection data and start host or client
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress.text, UInt16.Parse(port.text));
            if (_isHost)
            {
                NetworkManager.Singleton.StartHost();
            }
            else
            {
                NetworkManager.Singleton.StartClient();
            }
        }
        
        private void GoToGameScene()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Game);
        }
        
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            // Do not spawn the player on this scene
            response.CreatePlayerObject = false;
            // Approve the connection
            response.Approved = true;
        }
        
        private void OnClientConnectedCallback(ulong clientId)
        {
            // Host and client connected, go to the game scene
            if (clientId > 0)
            {
                GoToGameScene();
            }
        }
        
    }
}