using Core;
using Objects;
using UnityEngine;

namespace Controllers
{
    public class RobotController : MonoBehaviour, IRobot
    {
        private const int NumberOfTile = Constants.GameSettings.NumberOfTiles;
        
        public RobotDirection Direction { get; private set; }

        private ResourcesManager _resourcesManager;
        private QuestionController _questionOverlay;
        private GridController _grid;
        private LandmineController _currentLandmine;

        private void Start()
        {
            _grid = FindObjectOfType<GridController>();
            _questionOverlay = FindObjectOfType<QuestionController>(true);
            _resourcesManager = gameObject.AddComponent<ResourcesManager>();
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
            if (Input.GetKeyDown(Constants.Actions.MoveRight) && _grid.CanMoveRight(transform.position.x + NumberOfTile))
            {
                transform.position += new Vector3(NumberOfTile, 0f, 0f);
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                Direction = RobotDirection.FacingRight;
            }
            // Move to left
            if (Input.GetKeyDown(Constants.Actions.MoveLeft) && _grid.CanMoveLeft(transform.position.x - NumberOfTile))
            {
                transform.position -= new Vector3(NumberOfTile, 0f, 0f);
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
                Direction = RobotDirection.FacingLeft;
            }
            // Move up
            if (Input.GetKeyDown(Constants.Actions.MoveUp) && _grid.CanMoveUp(transform.position.z + NumberOfTile))
            {
                transform.position += new Vector3(0f, 0f, NumberOfTile);
                transform.eulerAngles = new Vector3(0f, -90f, 0f);
                Direction = RobotDirection.FacingUp;
            }
            // Move down
            if (Input.GetKeyDown(Constants.Actions.MoveDown) && _grid.CanMoveDown(transform.position.z - NumberOfTile))
            {
                transform.position -= new Vector3(0f, 0f, NumberOfTile);
                transform.eulerAngles = new Vector3(0f, 90f, 0f);
                Direction = RobotDirection.FacingDown;
            }
        }

        public void IncreaseClearedMineCounter()
        {
            _resourcesManager.IncreaseClearedMinesCounter();
        }

        public void ReduceHealth(float value)
        {
            _resourcesManager.ReduceHealth(value);
        }
    }

}