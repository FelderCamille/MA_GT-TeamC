using Unity.Netcode;
using UnityEngine;

public class MineController : MonoBehaviour {

    public GameObject asSeen;
    public GameObject asEditable;

    void OnTriggerEnter(Collider other) {
        // TODO: explosion (on robot)

        if (other.TryGetComponent<NetPlayerController>(out var player)) {
            Debug.Log("explosion");

            player.robot.AddExplosionForce(300, other.ClosestPoint(player.robot.position), 1);
        }
    }

    void OnTriggerExit(Collider other) {
        // TODO?
    }

    public void ShowAsUsabe() {
        this.asEditable.SetActive(true);
    }
    public void UnshowAsUsabe() {
        this.asEditable.SetActive(false);
    }


    public void MarkAsShown() {
        this.asSeen.SetActive(true);
    }

    public void MarkAsUnshown() {
        this.asSeen.SetActive(false);
    }
}
