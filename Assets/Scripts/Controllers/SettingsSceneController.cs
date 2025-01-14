using System;
using Core;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

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
            ambientSlider.Init(UpdateAmbientVolume, SoundManager.AmbientSoundKey);
            effectsSlider.Init(UpdateEffectsVolume, SoundManager.EffectsSoundKey);
            movementsSlider.Init(UpdateMovementsVolume, SoundManager.MovementsSoundKey);
            explosionsSlider.Init(UpdateExplosionsVolume, SoundManager.ExplosionsSoundKey);
        }

        private void Back()
        {
            _sceneLoader.ShowScene(Constants.Scenes.Title);
        }
        
        private void UpdateAmbientVolume(float value)
        {
            audioMixer.SetFloat(AmbientVolumeKey, MathUtils.ToLog(value));
            PlayerPrefs.SetFloat(SoundManager.AmbientSoundKey, value);
        }
        
        private void UpdateEffectsVolume(float value)
        {
            audioMixer.SetFloat(EffectsVolumeKey, MathUtils.ToLog(value));
            PlayerPrefs.SetFloat(SoundManager.EffectsSoundKey, value);
        }
        
        private void UpdateMovementsVolume(float value)
        {
            audioMixer.SetFloat(MovementsVolumeKey, MathUtils.ToLog(value));
            PlayerPrefs.SetFloat(SoundManager.MovementsSoundKey, value);
        }
        
        private void UpdateExplosionsVolume(float value)
        {
            audioMixer.SetFloat(ExplosionsVolumeKey, MathUtils.ToLog(value));
            PlayerPrefs.SetFloat(SoundManager.ExplosionsSoundKey, value);
        }

        private void OnDisable()
        {
            PlayerPrefs.Save();
        }
    }
}