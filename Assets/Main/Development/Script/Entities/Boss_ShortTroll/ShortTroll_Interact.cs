using UnityEngine;

namespace Root
{
    public class ShortTroll_Interact : MonoBehaviour, IInteractable
    {
        [SerializeField] SoundLibrary soundLibrary;
        SoundBuilder soundBuilder;

        bool isInteractable = true;

        void Awake() {
            soundBuilder = SoundManager.Instance.CreateSoundBuilder();
        }

        public void OnInteract() {
            if(!isInteractable) {
                return;
            }
            isInteractable = false;

            print("kraaaagh... uh... aaaaa...");

            soundBuilder.WithPosition(transform.position).Play(soundLibrary.soundData);
            GameManager.Instance.SoulLevels = SoulLevels.Level1;
            Destroy(GetComponentInChildren<ParticleSystem>().gameObject);

            // PlayDrainAnimation();
        }
    }
}
