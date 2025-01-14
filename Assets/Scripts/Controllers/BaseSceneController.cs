using System;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class BaseSceneController : MonoBehaviour
    {

        [Header("General")]
        [SerializeField] private CustomButton generalBackButton;
        
        [Header("Choose role")]
        [SerializeField] private GameObject chooseRoleEmplacement;
        [SerializeField] private CustomButton hostButton;
        [SerializeField] private CustomButton joinButton;
        
        [Header("Role choosed")]
        [SerializeField] private GameObject roleChoosedEmplacement;
        [SerializeField] private InputField ipAddress;
        [SerializeField] private InputField port;
        [SerializeField] private InputField playerName;
        [SerializeField] private GameObject budgetEmplacement;
        [SerializeField] private Slider budgetSlider;
        [SerializeField] private Text budgetValue;
        [SerializeField] private GameObject mapThemeEmplacement;
        [SerializeField] private TMP_Dropdown mapThemeDropdown;
        [SerializeField] private CustomButton backButton;
        [SerializeField] private StartButton startButton;
        [SerializeField] private Text stateText;

        private SceneLoader _sceneLoader;
        
        private bool _isHost;
        
        private void Start()
        {
            // Init network connection approval callback
            NetworkManager.Singleton.ConnectionApprovalCallback += ConnectionApprovalCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;
            NetworkManager.Singleton.OnServerStarted += OnServerStartedCallback;
            NetworkManager.Singleton.OnServerStopped += OnServerStoppedCallback;
            // Get objects from the scene
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            // Disable join button if debug mode is enabled
            if (Constants.DebugAllowOnlyOneConnection) joinButton.enabled = false;
            // Initialize buttons
            generalBackButton.Init(OnGeneralBackButtonClick);
            hostButton.Init(OnHostButtonClick);
            joinButton.Init(OnJoinButtonClick);
            backButton.Init(OnBackButtonClick);
            startButton.Init(OnStartClick);
            // Initialize budget slider
            budgetSlider.value = Constants.GameSettings.DefaultMoney;
            budgetValue.text = Constants.GameSettings.DefaultMoney.ToString();
            budgetSlider.minValue = Constants.GameSettings.MinMoney;
            budgetSlider.maxValue = Constants.GameSettings.MaxMoney;
            budgetSlider.onValueChanged.AddListener(OnBudgetSliderValueChanged);
            // Initialize input fields
            port.text = "7777";
        }
        
        private void OnGeneralBackButtonClick()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Title);
        }
        
        private void OnHostButtonClick()
        {
            generalBackButton.Hide();
            chooseRoleEmplacement.SetActive(false);
            roleChoosedEmplacement.SetActive(true);
            ipAddress.text = GetLocalIPAddress();
            ipAddress.interactable = false;
            _isHost = true;
            budgetEmplacement.SetActive(true);
            mapThemeEmplacement.SetActive(true);
        }
        
        private void OnJoinButtonClick()
        {
            generalBackButton.Hide();
            chooseRoleEmplacement.SetActive(false);
            roleChoosedEmplacement.SetActive(true);
            ipAddress.interactable = true;
            if (Constants.DebugFillIPAddressOnClient) ipAddress.text = GetLocalIPAddress();
            _isHost = false;
            budgetEmplacement.SetActive(false);
            mapThemeEmplacement.SetActive(false);
        }

        private void OnBackButtonClick()
        {
            generalBackButton.Show();
            roleChoosedEmplacement.SetActive(false);
            chooseRoleEmplacement.SetActive(true);
            ipAddress.interactable = true;
            port.interactable = true;
            startButton.Enable();
            UpdateState("");
        }
        
        private void OnBudgetSliderValueChanged(float value)
        {
            budgetValue.text = ((int) value).ToString();
        }

        private void OnStartClick()
        {
            if (playerName.text == "")
            {
                playerName.text = "Joueur " + (_isHost ? "1" : "2");
            }
            // Disable input fields and start button
            ipAddress.interactable = false;
            port.interactable = false;
            startButton.Disable();
            // Set connection data and start host or client
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress.text, UInt16.Parse(port.text));
            try
            {
                if (_isHost)
                {
                    NetworkManager.Singleton.StartHost();
                    UpdateState("Démarrage du serveur...");
                }
                else
                {
                    NetworkManager.Singleton.StartClient();
                    UpdateState("Tentative de connexion...");
                }
            } catch (Exception e) {
                Debug.LogError(e);
                ipAddress.interactable = true;
                port.interactable = true;
                startButton.Enable();
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
        
        private void OnServerStartedCallback()
        {
            UpdateState($"Serveur démarré." + (!Constants.DebugAllowOnlyOneConnection ? " Attente d'un joueur..." : ""));
        }
        
        private void OnServerStoppedCallback(bool isHost)
        {
            UpdateState("Serveur arrêté.");
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
            if (Constants.DebugAllowOnlyOneConnection)
            {
                GoToGameScene();
            }
            else if (clientId > 0) // Host and client connected, go to the game scene
            {
                UpdateState(_isHost ? "Deuxième joueur connecté. Démarrage du jeu..." : "Connecté. Démarrage du jeu...");
                GoToGameScene();
            }
        }
        
        private void OnClientDisconnectedCallback(ulong clientId)
        {
            print("Client disconnected");
            if (clientId > 0)
            {
                UpdateState(_isHost ? "Deuxième joueur déconnecté. Attente d'un autre joueur..." : "Déconnecté.");
            }
        }
        
        private void UpdateState(string state)
        {
            stateText.text = state;
        }

        private void OnDisable()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ConnectionApprovalCallback;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectedCallback;
            NetworkManager.Singleton.OnServerStarted -= OnServerStartedCallback;
            NetworkManager.Singleton.OnServerStopped -= OnServerStoppedCallback;
        }
    }
}