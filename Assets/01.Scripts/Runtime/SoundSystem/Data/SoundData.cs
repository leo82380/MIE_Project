using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MIE.Runtime.SoundSystem.Data
{
    [Serializable]
    public class SoundData
    {
        public AudioResource AudioResource;
        [Range(0f, 1f)]
        public float Volume = 1f;
        [Range(0f, 10f)]
        public float Pitch = 1f;
        [TextArea]
        public string Description;
    }
}