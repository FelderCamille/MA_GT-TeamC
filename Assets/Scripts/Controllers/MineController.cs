using DefaultNamespace;
using UI;
using UnityEngine;

public class MineController : MonoBehaviour
{

    [Header("Content")]
    public GameObject Robot;
    public GameObject QuestionOverlay;
    
    [Header("Settings")]
    public float CollidingDistance = 1f;

    private bool _answering = false;

    private void Update()
    {
        // Display question when entering in area
        if (Vector3.Distance (transform.position, Robot.transform.position) < CollidingDistance && !_answering)
        {
            _answering = true;
            QuestionOverlay.SetActive(true);
        }
    }

    /**
     * Deactivate the object when it has collided with a "Robot" game object
     */
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == Robot)
        {
            // TODO: "explode" object
            this.gameObject.SetActive(false);
        }
    }

}
