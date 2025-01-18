using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "New Sound", menuName = "Scriptable Objects/New Sound")]
    public class SoundLibrary : ScriptableObject
    {
        [Space(30)]
        public string soundName;
        public SoundData soundData;

    }
}
