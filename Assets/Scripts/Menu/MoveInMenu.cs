using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MoveSmoothly : MonoBehaviour
{
    public static bool Approximately(Quaternion val, Quaternion about, float range = 0.001f) {
        return Quaternion.Dot(val,about) > 1f-range;
    }

    public float speedMove = 10.0f;
    public float speedRotation = 30.0f;

    public Transform from;
    public Transform to;

    public GameObject toDisable;
    public GameObject toEnable;

    private Button button;

    void Start() {
        this.button = this.GetComponent<Button>();

        this.enabled = false;
        this.button.onClick.AddListener(() => {
            this.enabled = true;
            // this.toDisable.SetActive(false);
        });
    }

    void FixedUpdate() {
        bool noNeedToMove = this.from.position.Equals(this.to.position);
        bool noNeedToRotate = Approximately(this.from.rotation, this.to.rotation);

        if (noNeedToMove && noNeedToRotate) {
            this.toDisable.SetActive(false);
            this.toEnable.SetActive(true);
            
            this.enabled = false;

            return;
        }

        if (!noNeedToMove) {
            this.from.position = Vector3.MoveTowards(
                this.from.position,
                this.to.position,
                this.speedMove * Time.fixedDeltaTime
            );
        }

        if (!noNeedToRotate) {
            this.from.rotation = Quaternion.RotateTowards(
                this.from.rotation,
                this.to.rotation,
                this.speedRotation * Time.fixedDeltaTime
            );
        }
    }

}
