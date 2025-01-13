using Core;
using UI;
using UnityEngine;
using UnityEngine.Audio;

namespace Controllers
{
    public class SettingsSceneController : MonoBehaviour
    {

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private CustomButton backButton;
        [SerializeField] private AudioSlider ambientSlider;
        [SerializeField] private AudioSlider effectsSlider;
        [SerializeField] private AudioSlider movementsSlider;
        [SerializeField] private AudioSlider explosionsSlider;
        
        private SceneLoader _sceneLoader;
        
        public const string AmbientVolumeKey = "AmbientVolume";
        public const string EffectsVolumeKey = "EffectsVolume";
        public const string MovementsVolumeKey = "MovementsVolume";
        public const string ExplosionsVolumeKey = "ExplosionsVolume";
        
        private void Start()
        {
            // Get scene loader from the scene
            _sceneLoader = FindFirstObjectByType<SceneLoader>();
            // Initialize buttons
            backButton.Init(Back);
            // Initialize sliders
            ambientSlider.Init(UpdateAmbientVolume, AmbientVolumeKey);
            effectsSlider.Init(UpdateEffectsVolume, EffectsVolumeKey);
            movementsSlider.Init(UpdateMovementsVolume, MovementsVolumeKey);
            explosionsSlider.Init(UpdateExplosionsVolume, ExplosionsVolumeKey);
        }

        private void Back()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Title);
        }
        
        private void UpdateAmbientVolume(float value)
        {
            audioMixer.SetFloat(AmbientVolumeKey, value);
            PlayerPrefs.SetFloat(SoundManager.AmbiantSoundKey, value);
        }
        
        private void UpdateEffectsVolume(float value)
        {
            audioMixer.SetFloat(EffectsVolumeKey, value);
            PlayerPrefs.SetFloat(SoundManager.EffectsSoundKey, value);
        }
        
        private void UpdateMovementsVolume(float value)
        {
            audioMixer.SetFloat(MovementsVolumeKey, value);
            PlayerPrefs.SetFloat(SoundManager.MovementsSoundKey, value);
        }
        
        private void UpdateExplosionsVolume(float value)
        {
            audioMixer.SetFloat(ExplosionsVolumeKey, value);
            PlayerPrefs.SetFloat(SoundManager.ExplosionsSoundKey, value);
        }
    }
}