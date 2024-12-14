/*using UnityEngine;

namespace Controllers
{
    public class FollowPlayerCameraController : MonoBehaviour
    {

        private float _speed = 3f;
        public Vector3 offset = new (0, 3, 0);
        public float followDistance = 10;
        public float teleportDistanceThreshold = 100f;
        
        private Transform _robot;

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
}*/

using UnityEngine;

namespace Controllers
{
    public class FollowPlayerCameraController : MonoBehaviour
    {
        [Header("Camera Follow Settings")]
        private float _speed = 3f;  // Vitesse de suivi
        public Vector3 offset = new(0, 3, -7);  // D�calage de la cam�ra par rapport au robot (derri�re et en hauteur)
        public float followDistance = 5f;  // Distance de la cam�ra par rapport au robot
        public float teleportDistanceThreshold = 10f;  // Distance � partir de laquelle la cam�ra se t�l�porte
        public float rotationSpeed = 5f;  // Vitesse de la rotation de la cam�ra pour la suivre

        private Transform _robot;  // Le robot que cette cam�ra suit

        // M�thode pour initialiser la cam�ra avec le robot qu'elle doit suivre
        public void SetRobotToFollow(Transform robotTransform)
        {
            _robot = robotTransform;
        }

        private void Update()
        {
            if (_robot == null)
                return;

            // Si la cam�ra est trop �loign�e, t�l�porter � la position du robot
            if (Vector3.Distance(transform.position, _robot.position) > teleportDistanceThreshold)
            {
                transform.position = _robot.position + offset + -transform.forward * followDistance;
            }
            else
            {
                // Calculer la position de la cam�ra en utilisant Lerp pour un mouvement fluide
                Vector3 targetPosition = _robot.position + offset + -transform.forward * followDistance;
                transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);

                // Faire tourner la cam�ra en douceur pour regarder vers le robot
                Quaternion targetRotation = Quaternion.LookRotation(_robot.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}


