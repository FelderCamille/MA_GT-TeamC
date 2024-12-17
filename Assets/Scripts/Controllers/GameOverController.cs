using Core;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private GameOverReviveButton reviveButton;
        [SerializeField] private CustomButton quitButton;
        
        private SceneLoader _sceneLoader;
        private GridController _gridController;
        private ResourcesManager _resources; // Set by Show/Hide methods
        
        private void Start()
        {
            // Get references
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            _gridController = FindFirstObjectByType<GridController>();
            // Init buttons
            reviveButton.Init(Revive);
            quitButton.Init(Quit);
        }
        
        public void Show(ResourcesManager resourcesManager)
        {
            // Set resources manager
            _resources = resourcesManager;
            // Hide the robot
            _resources.GetComponent<RobotController>().Hide();
            // Show game over screen
            gameObject.SetActive(true);
            // Check if the player can revive
            if (_resources.HasEnoughMoneyToBuy(Constants.Prices.Revive)) reviveButton.Enabled();
            else reviveButton.Disable();
        }
        
        public void Hide(ResourcesManager resourcesManager)
        {
            _resources = resourcesManager;
            gameObject.SetActive(false);
        }
        
        private void Revive()
        {
            if (_resources != null && _resources.HasEnoughMoneyToBuy(Constants.Prices.Revive))
            {
                // Repair the robot
                _resources.ReduceMoney(Constants.Prices.Revive);
                _resources.Repair();
                // Reset robot spawn
                var robot = _resources.GetComponent<RobotController>();
                if (_gridController != null && robot != null) _gridController.ResetRobotSpawn(robot);
                // Show the robot
                if (robot != null) robot.Show();
            }
        }
        
        private void Quit()
        {
            // Go to result scene
            _sceneLoader.ShowScene(Constants.Scenes.Result);
            // Quit game
            NetworkManager.Singleton.Shutdown(); // TODO: do not do that.
        }
    }
}