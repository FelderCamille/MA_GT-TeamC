using Core;
using Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class RobotController : MonoBehaviour, IRobot
    {
        
        [Header("Content")]
        public ResourcesManager resourcesManager;
        public GridController grid;
        
        [Header("Settings")]
        public int numberOfTile = 1;
        public RobotDirection Direction { get; private set; }

        private QuestionController _questionOverlay;
        private LandmineController _currentLandmine;
        
        private void Awake()
        {
            _questionOverlay = FindObjectOfType<QuestionController>(true);
        }

        private void Update()
        {
            // Do nothing if the robot is answering a question
            if (_questionOverlay.IsAnswering) return;
            // Handle movements
            HandleMovements();
        }

        private void HandleMovements()
        {
            // Move to right
            if (Input.GetKeyDown(Constants.Actions.MoveRight) && grid.CanMoveRight(transform.position.x + numberOfTile))
            {
                transform.position += new Vector3(numberOfTile, 0f, 0f);
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                Direction = RobotDirection.FacingRight;
            }
            // Move to left
            if (Input.GetKeyDown(Constants.Actions.MoveLeft) && grid.CanMoveLeft(transform.position.x - numberOfTile))
            {
                transform.position -= new Vector3(numberOfTile, 0f, 0f);
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                Direction = RobotDirection.FacingLeft;
            }
            // Move up
            if (Input.GetKeyDown(Constants.Actions.MoveUp) && grid.CanMoveUp(transform.position.z + numberOfTile))
            {
                transform.position += new Vector3(0f, 0f, numberOfTile);
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
                Direction = RobotDirection.FacingUp;
            }
            // Move down
            if (Input.GetKeyDown(Constants.Actions.MoveDown) && grid.CanMoveDown(transform.position.z - numberOfTile))
            {
                transform.position -= new Vector3(0f, 0f, numberOfTile);
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
                Direction = RobotDirection.FacingDown;
            }
        }

        public void IncreaseClearedMineCounter()
        {
            resourcesManager.IncreaseClearedMinesCounter();
        }

        public void ReduceHealth(float value)
        {
            resourcesManager.ReduceHealth(value);
        }
    }

}