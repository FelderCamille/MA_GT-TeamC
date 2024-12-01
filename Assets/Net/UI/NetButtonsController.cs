using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetButtons : MonoBehaviour
{
    public Button btnHost, btnJoin, btnQuit;

    void Start() {
        btnHost.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        btnJoin.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
        btnQuit.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
        });
    }

    void LateUpdate() {
        var net = NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;

        btnHost.gameObject.SetActive(!net);
        btnJoin.gameObject.SetActive(!net);
        btnQuit.gameObject.SetActive(net);
    }
}
