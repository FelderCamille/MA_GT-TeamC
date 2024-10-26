using UnityEngine;

namespace Controllers
{
    public class FollowPlayerCameraController : MonoBehaviour
    {

        public Transform robot;
        public float speed = 3f;
        public Vector3 offset = new (0, 3, 0);
        public float followDistance = 10;

        public float teleportDistanceThreshold = 100f;

        private void Update()
        {
            if (Vector3.Distance(transform.position, robot.position) > teleportDistanceThreshold)
            {
                transform.position = robot.position + offset + -transform.forward * followDistance;
            }
            else
            {
                Vector3 position = Vector3.Lerp(transform.position, robot.position + offset + -transform.forward * followDistance, speed * Time.deltaTime);
                transform.position = position;
            }
        }
    }
}