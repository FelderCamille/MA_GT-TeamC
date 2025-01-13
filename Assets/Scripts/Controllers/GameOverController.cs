using System.Linq;
using Core;
using UI;
using UnityEngine;

namespace Controllers
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private GameOverReviveButton reviveButton;
        [SerializeField] private CustomButton quitButton;
        [SerializeField] private GameObject actions;
        
        private GridController _gridController;
        private RobotController _robot;
        private RobotController _otherRobot;
        
        private bool _isInitialized;
        
        private void Init()
        {
            // Init robot and resources
            var robots = FindObjectsByType<RobotController>(FindObjectsSortMode.None);
            _robot = robots.First(r => r.IsOwner);
            _otherRobot = robots.First(r => !r.IsOwner);
            // Get references
            _gridController = FindFirstObjectByType<GridController>();
            // Init buttons
            reviveButton.Init(Revive);
            quitButton.Init(Quit);
            // Set initialized to true
            _isInitialized = true;
        }
        
        public void Show()
        {
            if (!_isInitialized) Init();
            // Hide the robot
            _robot.Hide();
            // Show game over screen
            panel.SetActive(true);
            // Check if the player can revive
            var resources = _robot.GetComponent<ResourcesManager>();
            if (resources.HasEnoughMoneyToBuy(Constants.Prices.Revive)) reviveButton.Enabled();
            else reviveButton.Disable();
        }
        
        public void Hide()
        {
            panel.SetActive(false);
        }
        
        private void Revive()
        {
            var resources = _robot.GetComponent<ResourcesManager>();
            if (!resources.HasEnoughMoneyToBuy(Constants.Prices.Revive)) return;
            // Repair the robot
            resources.ReduceMoney(Constants.Prices.Revive);
            resources.Repair();
            // Reset robot spawn
            if (_gridController != null) _gridController.ResetRobotSpawn(_robot);
            // Show the robot
            _robot.Show();
        }
        
        private void Quit()
        {
            var robotResources = _robot.GetComponent<ResourcesManager>();
            var otherRobotResources = _otherRobot.GetComponent<ResourcesManager>();
            // Handle method according to the other robot state
            if (!robotResources.HasEnoughMoneyToBuy(Constants.Prices.Revive) && otherRobotResources.IsDead)
            {
                FindFirstObjectByType<TimeManager>().StopRpc();
            }
            else
            {
                // Set camera on the other robot
                FindFirstObjectByType<FollowPlayerCameraController>().Init(_otherRobot);
                // Hide the robot
                _gridController.ResetRobotSpawn(_robot);
                _robot.Hide();
                robotResources.Hide(); // Hide overlay
                actions.SetActive(false);
                // Show the other robot
                _otherRobot.Show(force: true);
                // Hide the game over screen
                panel.SetActive(false);
            }
        }
    }
}