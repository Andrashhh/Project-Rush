using System;
using System.Collections.Generic;
using UnityEngine;

namespace Root
{
    public class PlayerSounds : MonoBehaviour {
        public Sound InteractFailed = new();
        public Sound Footsteps = new();

        void Awake() {
            LoadSoundBuilderOf(InteractFailed);
            LoadSoundBuilderOf(Footsteps);
        }

        public void PlaySound(Sound sound) {
            if(!sound.soundLibrary) {
                return;
            }
            sound.soundBuilder.Play(sound.soundLibrary.soundData);
        }

        public void PlaySoundWithRandomPitch(Sound sound) {
            if(!sound.soundLibrary) {
                return;
            }
            sound.soundBuilder.WithRandomPitch().Play(sound.soundLibrary.soundData);
        }

        void LoadSoundBuilderOf(Sound sound) {
            sound.soundBuilder = SoundManager.Instance.CreateSoundBuilder();
        }
    }

    [Serializable]
    public class Sound {
        public SoundLibrary soundLibrary;
        public SoundBuilder soundBuilder;
    }
}
