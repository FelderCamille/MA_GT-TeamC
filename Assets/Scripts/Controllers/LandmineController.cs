using DefaultNamespace;
using Objects;
using UnityEngine;

namespace Controllers
{
    public class LandmineController : MonoBehaviour, ILandmine
    {
        [Header("Content")]
        public QuestionController questionOverlay;
        
        [Header("Settings")]
        public float collidingDistance = 1.3f; // One case of distance, no diagonal

        private RobotController _robot;

        private void Start()
        {
            _robot = FindObjectOfType<RobotController>();
        }

        private void Update()
        {
            DetectRobotApproach();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == _robot.gameObject)
            {
                OnRobotCollided();
            }
        }

        public void DetectRobotApproach()
        {
            // Check if the user wants to clear the mine
            if (Input.GetKeyDown(Globals.Actions.ClearMine) && !questionOverlay.IsAnswering())
            {
                // Check if the distance between the robot and the landmine permits to answer the question
                if (Vector3.Distance (transform.position, _robot.gameObject.transform.position) < collidingDistance)
                {
                    // Define mine in the question overlay
                    questionOverlay.SetMine(this);
                    // Show question overlay
                    questionOverlay.gameObject.SetActive(true);
                }
            }
        }

        public void OnLandmineCleared(bool success)
        {
            // Manage result
            if (success)
            {
                _robot.ResourcesManager.ClearedLandmines += 1;
            }
            else
            {
                _robot.ResourcesManager.Health -= Globals.Values.HealthRemovedWhenExplosion;
            }
            // Remove landmine
            gameObject.SetActive(false);
            // Hide question overlay
            questionOverlay.gameObject.SetActive(false);
        }

        public void OnRobotCollided()
        {
            OnLandmineCleared(false);
        }
    }
}
