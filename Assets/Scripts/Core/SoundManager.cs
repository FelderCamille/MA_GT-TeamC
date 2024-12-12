using UnityEngine;

namespace Core
{
    public class SoundManager: MonoBehaviour
    {
        public AudioSource buySoundSource;
        
        public void PlayBuySound()
        {
            buySoundSource.Play();
        }
    }

}