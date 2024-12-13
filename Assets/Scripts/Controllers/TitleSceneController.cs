using UI;
using UnityEngine;

namespace Controllers
{
    public class TitleSceneController : MonoBehaviour
    {

        [SerializeField] private CustomButton startButton;
        [SerializeField] private CustomButton encyclopediaButton;
        [SerializeField] private CustomButton settingsButton;
        
        private SceneLoader _sceneLoader;
        
        private void Start()
        {
            // Get scene loader from the scene
            _sceneLoader = FindObjectOfType<SceneLoader>();
            // Initialize buttons
            startButton.Init(OnStartButtonClick);
            encyclopediaButton.Init(OnEncyclopediaButtonClick);
            settingsButton.Init(OnSettingsButtonClick);
        }

        private void OnStartButtonClick()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Base);
        }
        
        private void OnEncyclopediaButtonClick()
        {
            Debug.Log("Encyclopedia button not implemented");
        }
        
        private void OnSettingsButtonClick()
        {
            Debug.Log("Settings button not implemented");
        }
    }
}