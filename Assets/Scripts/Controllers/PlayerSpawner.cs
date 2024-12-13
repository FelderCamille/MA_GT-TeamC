using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class PlayerSpawner : NetworkBehaviour
    {
        
        [SerializeField] private RobotController playerPrefab;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoaded;
        }

        private void OnSceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsFailed)
        {
            if (IsHost && sceneName == Constants.Scenes.Game)
            {
                foreach (var clientId in clientsCompleted)
                {
                    SpawnRobot(clientId);
                }
            }
        }
        
        private void SpawnRobot(ulong clientId)
        {
            var robot = Instantiate(playerPrefab);
            robot.transform.position = new Vector3(5, 0, 5);
            robot.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
}