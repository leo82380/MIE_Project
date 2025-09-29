using System.Collections.Generic;
using DG.Tweening;
using MIE.Attribute.Conditional;
using MIE.Runtime.SoundSystem.Data;
using UnityEngine;

namespace MIE.Runtime.SoundSystem.Core
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundSO soundData;
        [SerializeField] private int audioSourcePoolSize = 10;

        private List<AudioSource> audioSources;

        private void Awake()
        {
            audioSources = new List<AudioSource>();
            for (int i = 0; i < audioSourcePoolSize; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                audioSources.Add(source);
            }
        }

        public void PlaySound(string key, SoundType soundType = SoundType.SFX)
        {
            if (!soundData.Sounds.TryGetValue(key, out var sound))
            {
                Debug.LogWarning($"Sound with key {key} not found.");
                return;
            }

            var source = GetAvailableOrNewAudioSource();

            if (source.outputAudioMixerGroup == null)
            {
                // source.outputAudioMixerGroup = Managers.Instance.GetManager<SoundManager>()
                //     .GetAudioMixerGroup(soundType);
            }

            if (sound.AudioResource is AudioClip)
            {
                source.pitch = sound.Pitch;
                source.volume = sound.Volume;
            }

            source.resource = sound.AudioResource;
            source.loop = soundType == SoundType.BGM;
            source.Play();
        }

        public void PlaySoundFade(string key, float fadeDuration = 1f)
        {
            if (!soundData.Sounds.TryGetValue(key, out var sound))
            {
                Debug.LogWarning($"Sound with key {key} not found.");
                return;
            }

            // var mixerGroup = Managers.Instance.GetManager<SoundManager>()
            //     .GetAudioMixerGroup(SoundType.BGM);

            // foreach (var source in audioSources)
            // {
            //     if (source.isPlaying && source.outputAudioMixerGroup == mixerGroup)
            //     {
            //         source.DOFade(0f, fadeDuration).OnComplete(() => source.Stop());
            //     }
            // }

            var newSource = GetAvailableOrNewAudioSource();
            //newSource.outputAudioMixerGroup = mixerGroup;

            newSource.pitch = sound.Pitch;
            newSource.volume = 0f;
            newSource.loop = true;
            newSource.resource = sound.AudioResource;

            newSource.Play();
            newSource.DOFade(sound.Volume, fadeDuration);
        }

        public void StopSound(string key)
        {
            if (!soundData.Sounds.TryGetValue(key, out var sound))
            {
                Debug.LogWarning($"Sound with key {key} not found.");
                return;
            }

            foreach (var source in audioSources)
            {
                if (source.clip == sound.AudioResource && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }

        private AudioSource GetAvailableOrNewAudioSource()
        {
            foreach (var source in audioSources)
            {
                if (!source.isPlaying)
                    return source;
            }

            var newSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(newSource);
            return newSource;
        }
    }

    public enum SoundType
    {
        BGM,
        SFX,
    }
}
