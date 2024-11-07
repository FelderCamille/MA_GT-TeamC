using Objects;
using UnityEngine;

namespace Controllers
{
    public class LandmineController : MonoBehaviour, ILandmine
    {
        
        [Header("Settings")]
        public float collidingDistance = 1.5f; // One tile of distance, no diagonal

        private QuestionController _questionOverlay;
        private RobotController _robot;

        private void Awake()
        {
            _questionOverlay = FindObjectOfType<QuestionController>(true);
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
            // Check if the user wants to clear the mine, if not return
            if (!Input.GetKeyDown(Constants.Actions.ClearMine) || _questionOverlay.IsAnswering()) return;
            // Check if the distance between the robot and the landmine permits to answer the question, if not return
            if (!(Vector3.Distance(transform.position, _robot.gameObject.transform.position) < collidingDistance)) return;
            // Define mine in the question overlay
            _questionOverlay.Mine = this;
            // Show question overlay
            _questionOverlay.gameObject.SetActive(true);
        }

        public void OnLandmineCleared(LandmineCleared state)
        {
            // Manage robot
            switch (state)
            {
                case LandmineCleared.answerSuccess:
                    _robot.IncreaseClearedMineCounter();
                    break;
                case LandmineCleared.answerFailure:
                    _robot.ReduceHealth(Constants.Values.HealthRemovedWhenFailure);
                    break;
                case LandmineCleared.explosion:
                    _robot.ReduceHealth(Constants.Values.HealthRemovedWhenExplosion);
                    break;
            }
            // Remove landmine
            gameObject.SetActive(false);
            // Hide question overlay
            _questionOverlay.gameObject.SetActive(false);
        }

        public void OnRobotCollided()
        {
            OnLandmineCleared(LandmineCleared.explosion);
        }
    }
}
