using Unity.VisualScripting;
using UnityEngine;

namespace Root
{
    public class PlayerFootsteps : MonoBehaviour
    {
        float tempTime;

        public void UpdateFootsteps(CharacterState state, CharacterInput input, PlayerSounds sound, float stepInterval, float time) {
            if(state.Stance is not Stance.Stand or Stance.Crouch) {
                return;
            }

            if(!state.Grounded && !(tempTime > 0.3f)) {
                return;
            }

            if(input.Move != null && !(state.Stance == Stance.Slide)) {
                tempTime += time;
                if(tempTime > stepInterval / (state.Velocity.magnitude / 8)) {
                    PlayFootstepSound(sound);
                    tempTime = 0f;
                }
            }
            else {
                tempTime = 0f;
            }
        }

        void PlayFootstepSound(PlayerSounds sound) {
            sound.PlaySoundWithRandomPitch(sound.Footsteps);
        }
    }
}
