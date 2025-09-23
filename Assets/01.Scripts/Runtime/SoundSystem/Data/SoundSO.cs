using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace MIE.Runtime.SoundSystem.Data
{
    [CreateAssetMenu(fileName = "SoundSO", menuName = "MIE/SO/Sound/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        [SerializedDictionary("Sound Key", "Sound Data")]
        public SerializedDictionary<string, SoundData> Sounds;
    }
}