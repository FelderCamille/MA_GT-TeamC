using UnityEngine;

namespace Controllers
{
    public class FollowPlayerCameraController : MonoBehaviour
    {

        private float _speed = 3f;
        public Vector3 offset = new (0, 3, 0);
        public float followDistance = 10;
        public float teleportDistanceThreshold = 100f;
        
        public Transform _robot;

        private void Start()
        {
            _robot = FindObjectOfType<RobotController>().transform;
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _robot.position) > teleportDistanceThreshold)
            {
                transform.position = _robot.position + offset + -transform.forward * followDistance;
            }
            else
            {
                var position = Vector3.Lerp(transform.position, _robot.position + offset + -transform.forward * followDistance, _speed * Time.deltaTime);
                transform.position = position;
            }
        }
    }
}