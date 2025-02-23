using Controllers;
using Objects;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using Utils;
using Random = UnityEngine.Random;

namespace Core
{
    public class SoundManager: MonoBehaviour
    {

        public static readonly string AmbientSoundKey = "AmbientSound";
        public static readonly string EffectsSoundKey = "EffectsSound";
        public static readonly string MovementsSoundKey = "MovementsSound";
        public static readonly string ExplosionsSoundKey = "ExplosionsSound";
        
        public static SoundManager Instance;

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource[] buySoundSources;
        [SerializeField] private AudioSource[] explosionSoundSources;
        [SerializeField] private AudioSource[] beepSoundSources;
        [SerializeField] private AudioSource cutSoundSource;
        [SerializeField] private AudioSource warSoundSource;
        [SerializeField] private AudioSource natureSoundSource;
        [SerializeField] private AudioSource[] setMineSoundSources;
        [SerializeField] private AudioSource sonarSoundSource;
        public AudioSource moveSoundSource;
        public AudioSource turnSoundSource;
        [SerializeField] private AudioSource repairSoundSource;
        [SerializeField] private AudioSource openMineSoundSource;
        [SerializeField] private AudioSource openTentSoundSource;
        [SerializeField] private AudioSource closeTentSoundSource;
        [SerializeField] private AudioSource openBookSoundSource;
        [SerializeField] private AudioSource closeBookSoundSource;
        [SerializeField] private AudioSource visionSoundSource;
        [SerializeField] private AudioSource deniedSoundSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            LoadVolumes();
        }

        private void LoadVolumes()
        {
            var ambientVolume = PlayerPrefs.GetFloat(SettingsSceneController.AmbientVolumeKey, AudioSlider.DefaultValue);
            var effectsVolume = PlayerPrefs.GetFloat(SettingsSceneController.EffectsVolumeKey, AudioSlider.DefaultValue);
            var movementsVolume = PlayerPrefs.GetFloat(SettingsSceneController.MovementsVolumeKey, AudioSlider.DefaultValue);
            var explosionsVolume = PlayerPrefs.GetFloat(SettingsSceneController.ExplosionsVolumeKey, AudioSlider.DefaultValue);
            audioMixer.SetFloat(SettingsSceneController.AmbientVolumeKey, MathUtils.ToLog(ambientVolume));
            audioMixer.SetFloat(SettingsSceneController.EffectsVolumeKey, MathUtils.ToLog(effectsVolume));
            audioMixer.SetFloat(SettingsSceneController.MovementsVolumeKey, MathUtils.ToLog(movementsVolume));
            audioMixer.SetFloat(SettingsSceneController.ExplosionsVolumeKey, MathUtils.ToLog(explosionsVolume));
        }

        public void PlayBuySound()
        {
            var randomIndex = Random.Range(0, buySoundSources.Length);
            buySoundSources[randomIndex].Play();
        }
        
        public void PlayExplosionSound()
        {
            var randomIndex = Random.Range(0, explosionSoundSources.Length);
            explosionSoundSources[randomIndex].Play();
        }
        
        public void PlayBeepSound()
        {
            var randomIndex = Random.Range(0, beepSoundSources.Length);
            beepSoundSources[randomIndex].Play();
        }

        public void PlayCutSound()
        {
            cutSoundSource.Play();
        }

        public void PlayTankGoSound()
        {
            moveSoundSource.Play();
        }

        public void PlayTankTurnSound()
        {
            turnSoundSource.Play();
        }

        public void PlaySonarSound()
        {
            sonarSoundSource.Play();
        }

        public void PlaySetMineSound()
        {
            var randomIndex = Random.Range(0, setMineSoundSources.Length);
            setMineSoundSources[randomIndex].Play();
        }
        
        public void PlayRepairSound()
        {
            repairSoundSource.Play();
        }

        public void PlayOpenMineSound()
        {
            openMineSoundSource.Play();
        }

        public void PlayOpenTentSound()
        {
            openTentSoundSource.Play();
        }

        public void PlayCloseTentSound()
        {
            closeTentSoundSource.Play();
        }

        public void PlayOpenBookSound()
        {
            openBookSoundSource.Play();
        }

        public void PlayCloseBookSound()
        {
            closeBookSoundSource.Play();
        }

        public void PlayVisionSound()
        {
            visionSoundSource.Play();
        }
        
        public void StopVisionSound()
        {
            visionSoundSource.Stop();
        }

        public void PlayDeniedSound()
        {
            deniedSoundSource.Play();
        }

        public void PlayAmbientSound()
        {
            var mapTheme = GameParametersManager.Instance.MapTheme;
            switch (mapTheme)
            {
                case MapTheme.Nature:
                    natureSoundSource.Play();
                    break;
                case MapTheme.War:
                    warSoundSource.Play();
                    break;
                default:
                    Debug.LogWarning("Invalid ambient sound index!");
                    break;
            }

        }

    }

}