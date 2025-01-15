using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField] private GameParametersManager gameParametersPrefab;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject timeManagerPrefab;
        
        private void Start()
        {
            // Play ambient sound
            SoundManager.Instance.PlayAmbientSound();
            // Do some stuff only if the player is the host
            if (!NetworkManager.Singleton.IsHost) return;
            // Instantiate grid
            var gridObj = Instantiate(
                gridPrefab,
                new Vector3(0, 0, 0),
                Quaternion.Euler(0, 0, 0)
            );
            gridObj.name = $"Grid";
            gridObj.GetComponent<NetworkObject>().Spawn();
            // Instantiate time manager
            var timerObj = Instantiate(
                timeManagerPrefab,
                new Vector3(0, 0, 0),
                Quaternion.Euler(0, 0, 0)
            );
            gridObj.name = $"TimeManager";
            timerObj.GetComponent<NetworkObject>().Spawn();
        }
    }
}