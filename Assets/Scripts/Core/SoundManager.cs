using UnityEngine;

namespace Core
{
    public class SoundManager: MonoBehaviour
    {
        public AudioSource buySoundSource;
        public AudioSource bombe1SoundSource;
        public AudioSource bombe2SoundSource;
        public AudioSource bombe3SoundSource;
        public AudioSource bombe4SoundSource;
        public AudioSource bombe5SoundSource;
        public AudioSource buy2SoundSource;
        public AudioSource bip1SoundSource;
        public AudioSource bip2SoundSource;
        public AudioSource cutSoundSource;
        public AudioSource WarSoundSource;
        public AudioSource NatureSoundSource;
        public AudioSource setmine1SoundSource;
        public AudioSource setmine2SoundSource;
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


        public void PlayBuySound()
        {
            int randomIndex = Random.Range(0, 2); // Génère un index entre 0 et 4
            switch (randomIndex)
            {
                case 0:
                    buySoundSource.Play();
                    break;
                case 1:
                    buy2SoundSource.Play();
                    break;
                default:
                    Debug.LogWarning("Invalid bomb sound index!");
                    break;
            }
        }
        public void PlayExplosionSound()
        {
            int randomIndex = Random.Range(0, 5); // Génère un index entre 0 et 4
            switch (randomIndex)
            {
                case 0:
                    bombe1SoundSource.Play();
                    break;
                case 1:
                    bombe2SoundSource.Play();
                    break;
                case 2:
                    bombe3SoundSource.Play();
                    break;
                case 3:
                    bombe4SoundSource.Play();
                    break;
                case 4:
                    bombe5SoundSource.Play();
                    break;
                default:
                    Debug.LogWarning("Invalid bomb sound index!");
                    break;
            }
        }

        
        public void PlayBeepSound()
        {
            int randomIndex = Random.Range(0, 2); // Génère un index entre 0 et 4
            switch (randomIndex)
            {
                case 0:
                    bip1SoundSource.Play();
                    break;
                case 1:
                    bip2SoundSource.Play();
                    break;
                default:
                    Debug.LogWarning("Invalid bip sound index!");
                    break;
            }
        }

        public void PlayCutSound()
        {
            cutSoundSource.Play();
        }

        public void PlayAmbientSound()
        {
            int randomIndex = Random.Range(0, 2); // Génère un index entre 0 et 4
            switch (randomIndex)
            {
                case 0:
                    NatureSoundSource.Play();
                    break;
                case 1:
                    WarSoundSource.Play();
                    break;
                default:
                    Debug.LogWarning("Invalid ambiant sound index!");
                    break;
            }
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

        public void PlaySetmineSound()
        {
            int randomIndex = Random.Range(0, 2); // Génère un index entre 0 et 4
            switch (randomIndex)
            {
                case 0:
                    setmine1SoundSource.Play();
                    break;
                case 1:
                    setmine2SoundSource.Play();
                    break;
                default:
                    Debug.LogWarning("Invalid setmine sound index!");
                    break;
            }
        }
        public void playRepairSound()
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

    }

}