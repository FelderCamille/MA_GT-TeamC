using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class GameManager: MonoBehaviour
    {
        [SerializeField] private GameObject gridPrefab;
        
        private void Start()
        {
            if (!NetworkManager.Singleton.IsHost) return;
            var gridObj = Instantiate(
                gridPrefab,
                new Vector3(0, 0, 0),
                Quaternion.Euler(0, 0, 0)
            );
            gridObj.name = $"Grid";
            gridObj.GetComponent<NetworkObject>().Spawn();
        }
    }
}