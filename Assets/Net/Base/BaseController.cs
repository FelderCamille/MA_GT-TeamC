using Unity.Netcode;
using UnityEngine;

public class BaseController : MonoBehaviour {
    // Show "availibility"
    public GameObject truc;

    void Start() {
        this.truc.SetActive(false);
    }

    void OnTriggerEnter(Collider other) {
        // Only show the base as "editable" for the localplayer
        // TODO: only the local player and for example to the player1 is this is base1
        if (other.TryGetComponent<NetPlayerController>(out var player) && player.IsLocalPlayer) {
            this.truc.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<NetPlayerController>(out var player) && player.IsLocalPlayer) {
            this.truc.SetActive(false);
        }
    }
}
