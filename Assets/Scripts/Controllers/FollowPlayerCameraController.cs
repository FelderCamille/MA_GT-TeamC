using UnityEngine;

namespace Controllers
{
    public class FollowPlayerCameraController : MonoBehaviour
    {

        [SerializeField] private float speed = 3f;
        [SerializeField] private Vector3 offset = new (0, 3, 0);
        [SerializeField] private float followDistance = 8;
        [SerializeField] private float teleportDistanceThreshold = 100f;

        private Transform _robot;

        public void Init(RobotController robot)
        {
            _robot = robot.transform;
        }

        private void Update()
        {
            // Make sure that the robot is set
            if (_robot == null) return;
            // Teleport the camera on the robot if it's too far
            if (Vector3.Distance(transform.position, _robot.position) > teleportDistanceThreshold)
            {
                transform.position = _robot.position + offset + -transform.forward * followDistance;
            }
            // Compute camera position with "Lerp" to have a smooth movement
            var targetPosition = _robot.position + offset + -transform.forward * followDistance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}