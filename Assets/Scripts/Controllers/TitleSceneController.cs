using UI;
using UnityEngine;

namespace Controllers
{
    public class TitleSceneController : MonoBehaviour
    {

        [SerializeField] private CustomButton startButton;
        [SerializeField] private CustomButton settingsButton;
        
        private SceneLoader _sceneLoader;
        
        private void Start()
        {
            // Get scene loader from the scene
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            // Initialize buttons
            startButton.Init(OnStartButtonClick);
            settingsButton.Init(OnSettingsButtonClick);
        }

        private void OnStartButtonClick()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Base);
        }
        
        private void OnSettingsButtonClick()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Settings);
        }
    }
}