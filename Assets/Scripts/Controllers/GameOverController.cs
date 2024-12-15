using Core;
using UI;
using Unity.Netcode;
using UnityEngine;

namespace Controllers
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private StoreButton reviveButton;
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
            reviveButton.Init(Revive, "Réapparaître", Constants.Values.RevivePrice, "Icons/repair");
            quitButton.Init(Quit);
        }
        
        public void Show(ResourcesManager resourcesManager)
        {
            _resources = resourcesManager;
            gameObject.SetActive(true);
            // Check if the player can revive
            if (_resources.HasEnoughMoneyToBuy(Constants.Values.RevivePrice)) reviveButton.Enabled();
            else reviveButton.Disable();
        }
        
        public void Hide(ResourcesManager resourcesManager)
        {
            _resources = resourcesManager;
            gameObject.SetActive(false);
        }
        
        private void Revive()
        {
            if (_resources != null && _resources.HasEnoughMoneyToBuy(Constants.Values.RevivePrice))
            {
                _resources.ReduceMoney(Constants.Values.RevivePrice);
                _resources.Repair();
                var robot = _resources.GetComponent<RobotController>();
                if (_gridController != null && robot != null) _gridController.ResetRobotSpawn(robot);
            }
        }
        
        private void Quit()
        {
            // Go to result scene
            _sceneLoader.ShowScene(Constants.Scenes.Result);
            // Quit game
            NetworkManager.Singleton.Shutdown();
        }
    }
}