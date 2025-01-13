using System.Collections.Generic;
using System.Linq;
using Core;
using Objects;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class ResultSceneController : NetworkBehaviour
    {
        [SerializeField] private PlayerResult player1Result;
        [SerializeField] private PlayerResult player2Result;
        [SerializeField] private CustomButton quitButton;
        
        private SceneLoader _sceneLoader;
        private GridController _grid;
        
        // Synchronized data (need NetworkBehaviour to be synchronized)
        private readonly NetworkVariable<GameResultsData> _gameResultsData = new();

        public void Start()
        {
            // Get scene loader
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            _grid = FindFirstObjectByType<GridController>();
            // Subscribe to changes in NetworkVariable
            _gameResultsData.OnValueChanged += OnGameResultsChangedRpc;
            // Initialize the result data as host
            InitResult();
            // Initialize the quit button
            quitButton.Init(QuitGame);
        }

        private void InitResult()
        {
            // Only the host can access the connected clients
            if (!NetworkManager.Singleton.IsHost) return;
            // Manage the case where no player is connected (should not happen)
            var connectedClients = NetworkManager.Singleton.ConnectedClients;
            if (connectedClients.Count <= 0)
            {
                Debug.LogError("No connected clients");
                QuitGame();
                return;
            }
            // Retrieve the data for player 1
            var player1 = connectedClients.First().Value.PlayerObject;
            var player1Resources = player1.GetComponent<ResourcesManager>();
            var gameResults = new GameResultsData
            {
                player1Result = new PlayerResultData
                {
                    clientId = player1.OwnerClientId,
                    clearedMinesEasy = player1Resources.ClearedMinesEasy,
                    clearedMinesMedium = player1Resources.ClearedMinesMedium,
                    clearedMinesHard = player1Resources.ClearedMinesHard,
                    explodedMines = player1Resources.ExplodedMines,
                    notClearedMines = _grid.NotClearedMineCount(player1.OwnerClientId),
                    placedMines = player1Resources.PlacedMines,
                }
            };
            // Retrieve the data for player 2
            if (connectedClients.Count > 1)
            {
                var player2 = connectedClients.Last().Value.PlayerObject;
                var player2Resources = player2.GetComponent<ResourcesManager>();
                gameResults.player2Result = new PlayerResultData
                {
                    clientId = player2.OwnerClientId,
                    clearedMinesEasy = player2Resources.ClearedMinesEasy,
                    clearedMinesMedium = player2Resources.ClearedMinesMedium,
                    clearedMinesHard = player2Resources.ClearedMinesHard,
                    explodedMines = player2Resources.ExplodedMines,
                    notClearedMines = _grid.NotClearedMineCount(player2.OwnerClientId),
                    placedMines = player2Resources.PlacedMines,
                };
            }
            else
            {
                player2Result.gameObject.SetActive(false);
            }
            // Update network variable
            _gameResultsData.Value = gameResults;
        }
        
        [Rpc(SendTo.Everyone)]
        private void OnGameResultsChangedRpc(GameResultsData previousValue, GameResultsData newValue)
        {
            player1Result.Init(newValue.player1Result, newValue.player2Result);
            player2Result.Init(newValue.player2Result, newValue.player1Result);
        }

        private void QuitGame()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Title);
            NetworkManager.Singleton.Shutdown();
        }

        public override void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks
            _gameResultsData.OnValueChanged -= OnGameResultsChangedRpc;
        }
    }
}