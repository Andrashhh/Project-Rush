using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Root
{
    public class SoundOnHover : MonoBehaviour {
        Button playButton;

        public SoundLibrary HoverSound;
        public SoundBuilder HoverBuilder;

        void Awake() {
            HoverBuilder = SoundManager.Instance.CreateSoundBuilder();
        }

        public void PlayHoverSound() {
            HoverBuilder.Play(HoverSound.soundData);
        }
    }
}
