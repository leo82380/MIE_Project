using MIE.Runtime.SoundSystem.Core;
using UnityEngine;

namespace MIE.Runtime.SoundSystem.Tool
{
    public class AutoSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private string soundKey;
        [SerializeField] private SoundType soundType = SoundType.SFX;
        [SerializeField] private bool isStartOnAwake = true;

        private void Awake()
        {
            if (soundPlayer == null)
                soundPlayer = GetComponent<SoundPlayer>();
            if (soundPlayer == null)
            {
                Debug.LogError("SoundPlayer component not found on this GameObject.");
            }
        }

        private void Start()
        {
            if (!isStartOnAwake)
                return;
            if (soundPlayer != null)
            {
                soundPlayer.PlaySound(soundKey, soundType);
            }
            else
            {
                Debug.LogError("SoundPlayer component is not initialized.");
            }
        }

        public void PlaySound()
        {
            if (soundPlayer != null)
            {
                soundPlayer.PlaySound(soundKey, soundType);
            }
            else
            {
                Debug.LogError("SoundPlayer component is not initialized.");
            }
        }

        public void StopSound()
        {
            if (soundPlayer != null)
            {
                soundPlayer.StopSound(soundKey);
            }
            else
            {
                Debug.LogError("SoundPlayer component is not initialized.");
            }
        }
    }
}