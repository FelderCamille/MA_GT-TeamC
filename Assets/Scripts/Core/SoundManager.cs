using System;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class SoundManager: MonoBehaviour
    {

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

        private void Start()
        {
            PlayAmbientSound();
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

        private void PlayAmbientSound()
        {
            const MapTheme mapTheme = Constants.GameSettings.GameMapTheme;
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