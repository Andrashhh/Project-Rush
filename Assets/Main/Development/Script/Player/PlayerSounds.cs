using System;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

namespace Root
{
    public class PlayerSounds : MonoBehaviour {
        public Sound InteractFailed;
        public Sound InteractSuccesful;
        public Sound[] Footsteps;

        void Awake() {
            LoadSoundBuilderOf(InteractFailed);
            LoadSoundBuilderOf(InteractSuccesful);
            LoadSoundBuilderOf(null, Footsteps);
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

        public void PlayRandomSoundsWithRandomPitch(Sound[] sounds) {
            var temp = Rand.Range(0, sounds.Length);
            sounds[temp].soundBuilder.WithRandomPitch().Play(sounds[temp].soundLibrary.soundData);
        }

        void LoadSoundBuilderOf(Sound sound = null, Sound[] soundArray = null) {
            if(sound != null) {
                sound.soundBuilder = SoundManager.Instance.CreateSoundBuilder();
            }

            if(soundArray != null) {
                for(int i = 0; i < soundArray.Length; i++) {
                    soundArray[i].soundBuilder = SoundManager.Instance.CreateSoundBuilder();
                }
            }
        }
    }

    [Serializable]
    public class Sound {
        public SoundLibrary soundLibrary;
        public SoundBuilder soundBuilder;
    }
}
