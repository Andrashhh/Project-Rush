using UnityEngine;

namespace Root
{
    public class Ballerine_Interact : MonoBehaviour, IInteractable
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

            soundBuilder.WithPosition(transform.position).Play(soundLibrary.soundData);
            GameManager.Instance.SoulLevels = SoulLevels.Level2;
            Destroy(GetComponentInChildren<ParticleSystem>().gameObject);
        }
    }
}
