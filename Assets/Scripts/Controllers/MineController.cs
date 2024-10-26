using DefaultNamespace;
using UnityEngine;

namespace Controllers
{
    public class MineController : MonoBehaviour
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
            HandleDemining();
        }
        
        private void HandleDemining()
        {
            if (Input.GetKeyDown(Globals.Actions.ClearMine) && !questionOverlay.IsAnswering())
            {
                if (Vector3.Distance (transform.position, _robot.gameObject.transform.position) < collidingDistance)
                {
                    questionOverlay.gameObject.SetActive(true);
                    questionOverlay.SetMine(this);
                }
            }
        }
        
    }
}
