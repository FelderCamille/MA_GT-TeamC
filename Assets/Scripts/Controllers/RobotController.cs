using Core;
using Objects;
using UnityEngine;

namespace Controllers
{
    public class RobotController : MonoBehaviour, IRobot
    {
        
        [Header("Content")]
        public ResourcesManager resourcesManager;
        public GridController grid;
        
        [Header("Settings")]
        public int numberOfTile = 1;

        private LandmineController _currentLandmine;

        private void Update()
        {
            HandleMovements();
        }

        private void HandleMovements()
        {
            // Move to right
            if (Input.GetKeyDown(Constants.Actions.MoveRight) && grid.CanMoveRight(transform.position.x + numberOfTile))
            {
                transform.position += new Vector3(numberOfTile, 0f, 0f);
            }
            // Move to left
            if (Input.GetKeyDown(Constants.Actions.MoveLeft) && grid.CanMoveLeft(transform.position.x - numberOfTile))
            {
                transform.position -= new Vector3(numberOfTile, 0f, 0f);
            }
            // Move up
            if (Input.GetKeyDown(Constants.Actions.MoveUp) && grid.CanMoveUp(transform.position.z + numberOfTile))
            {
                transform.position += new Vector3(0f, 0f, numberOfTile);
            }
            // Move down
            if (Input.GetKeyDown(Constants.Actions.MoveDown) && grid.CanMoveDown(transform.position.z - numberOfTile))
            {
                transform.position -= new Vector3(0f, 0f, numberOfTile);
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