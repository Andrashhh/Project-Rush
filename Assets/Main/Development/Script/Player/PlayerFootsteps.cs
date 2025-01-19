using Unity.VisualScripting;
using UnityEngine;

namespace Root
{
    public class PlayerFootsteps : MonoBehaviour
    {
        float tempTime;

        public void UpdateFootsteps(CharacterState state, CharacterInput input, PlayerSounds sound, float stepInterval, float time) {
            var velocity = (state.Velocity.magnitude / 5);

            if(state.Stance is not Stance.Stand or Stance.Crouch) {
                return;
            }

            if(state.Grounded && !input.Jump) {
                if(input.Move != Vector2.zero) {
                    tempTime += time;
                    if(tempTime > stepInterval / velocity) {
                        PlayFootstepSound(sound);
                        tempTime = 0f;
                    }
                }
                else {
                    tempTime = 0.35f;
                }
            }
        }

        void PlayFootstepSound(PlayerSounds sound) {
            sound.PlayRandomSoundsWithRandomPitch(sound.Footsteps);
        }
    }
}
