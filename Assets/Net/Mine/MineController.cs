using Unity.Netcode;
using UnityEngine;

public class MineController : MonoBehaviour {

    // Indicator that th mine is at seen range
    public GameObject asSeen;
    // Indicator that the mine editable (de-mine)
    public GameObject asEditable;

    public bool firstInit = false;

    void OnTriggerEnter(Collider other) {
        // TODO: explosion (on robot)

        if (other.TryGetComponent<NetPlayerController>(out var player)) {
            if (this.firstInit) {
                // [Temporary]: avoid the explosion when setting the mine
                this.firstInit = false;
                return;
            }

            // No matter the robot
            player.robot.AddExplosionForce(200, other.ClosestPoint(player.robot.position), 1);

            this.gameObject.SetActive(false);
            // TODO: notify player? (if it has a list of seen mine)
            // Destroy(this.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        // TODO?
    }

    // Player sees the mine and could de-mine it
    public void ShowAsUsabe() {
        this.asEditable.SetActive(true);
    }
    public void UnshowAsUsabe() {
        this.asEditable.SetActive(false);
    }


    // Player sees the mine
    public void MarkAsShown() {
        this.asSeen.SetActive(true);
    }

    public void MarkAsUnshown() {
        this.asSeen.SetActive(false);
    }
}
