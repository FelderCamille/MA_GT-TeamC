using UnityEngine;

namespace UI
{
    public class FollowPlayerCameraController : MonoBehaviour
    {

        public Transform Robot;
        public float Speed;
        public Vector3 Offset;
        public float FollowDistance;
        public Quaternion rotation;

        public float teleportDistanceThreshold = 100f;

        private void Update()
        {
            if (Vector3.Distance(transform.position, Robot.position) > teleportDistanceThreshold)
            {
                transform.position = Robot.position + Offset + -transform.forward * FollowDistance;
            }
            else
            {
                Vector3 position = Vector3.Lerp(transform.position, Robot.position + Offset + -transform.forward * FollowDistance, Speed * Time.deltaTime);
                transform.position = position;
            }
            transform.rotation = rotation;
        }
    }
}