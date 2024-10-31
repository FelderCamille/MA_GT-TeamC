using DefaultNamespace;
using Core;
using Objects;
using UnityEngine;

namespace Controllers
{
    public class RobotController : MonoBehaviour, IRobot
    {
        
        [Header("Content")]
        public ResourcesManager resourcesManager;
        
        [Header("Settings")]
        public int numberOfCase = 1;

        private LandmineController _currentLandmine;

        private void Update()
        {
            HandleMovements();
        }

        private void HandleMovements()
        {
            // Move to right
            if (Input.GetKeyDown(Globals.Actions.MoveRight))
            {
                transform.position += new Vector3(numberOfCase, 0f, 0f);
            }
            // Move to left
            if (Input.GetKeyDown(Globals.Actions.MoveLeft))
            {
                transform.position -= new Vector3(numberOfCase, 0f, 0f);
            }
            // Move up
            if (Input.GetKeyDown(Globals.Actions.MoveUp))
            {
                transform.position += new Vector3(0f, 0f, numberOfCase);
            }
            // Move down
            if (Input.GetKeyDown(Globals.Actions.MoveDown))
            {
                transform.position -= new Vector3(0f, 0f, numberOfCase);
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