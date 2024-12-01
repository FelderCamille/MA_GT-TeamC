using UnityEngine;

public class PlayerCollider : MonoBehaviour {
    public enum TYPE {
        NORMAL, EXTENDED
    }

    public NetPlayerController player;
    public TYPE cType;

    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<MineController>(out var mine)) {
            this.player.SeeMine(mine, this.cType);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.TryGetComponent<MineController>(out var mine)) {
            this.player.SeeNoMine(mine, this.cType);
        }
    }
}
