using System;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class SoundManager: MonoBehaviour
    {

        public AudioSource[] buySoundSources;
        public AudioSource[] explosionSoundSources;
        public AudioSource[] beepSoundSources;
        public AudioSource cutSoundSource;
        public AudioSource warSoundSource;
        public AudioSource natureSoundSource;
        public AudioSource[] setMineSoundSources;
        public AudioSource sonarSoundSource;
        public AudioSource moveSoundSource;
        public AudioSource turnSoundSource;
        public AudioSource repairSoundSource;
        public AudioSource openMineSoundSource;
        public AudioSource openTentSoundSource;
        public AudioSource closeTentSoundSource;
        public AudioSource openBookSoundSource;
        public AudioSource closeBookSoundSource;
        //public AudioSource SoundSource;

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

        public void playOpenMineSound()
        {
            openMineSoundSource.Play();
        }

        public void playOpenTentSound()
        {
            openTentSoundSource.Play();
        }

        public void playCloseTentSound()
        {
            closeTentSoundSource.Play();
        }

        public void playOpenBookSound()
        {
            openBookSoundSource.Play();
        }

        public void playCloseBookSound()
        {
            closeBookSoundSource.Play();
        }
            
        private void PlayAmbientSound()
        {
            var mapTheme = Constants.GameSettings.GameMapTheme;
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