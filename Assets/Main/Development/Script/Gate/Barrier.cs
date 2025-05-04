using UnityEngine;
using UnityEngine.UIElements;

namespace Root
{
    public class Barrier : MonoBehaviour, IInteractable
    {
        MeshCollider barrierCollider;
        Material material;
        AudioSource barrierNoise;

        [SerializeField] float fadeOutSpeed = 1.25f;
        float fade;
        bool initiateFadeOut;

        [SerializeField] SoundLibrary soundLibrary;
        SoundBuilder soundBuilder;

        public int GateLevel;

        void Awake() {
            barrierCollider = gameObject.GetComponent<MeshCollider>();
            material = gameObject.GetComponent<MeshRenderer>().material;
            barrierNoise = gameObject.GetComponent<AudioSource>();

            soundBuilder = SoundManager.Instance.CreateSoundBuilder();
        }

        void Start() {
            fade = 0f;

            Close();
        }

        void Update() {
            FadeOut();
        }

        void FadeOut() {
            if(initiateFadeOut && fade < 1) {
                fade += Time.deltaTime / fadeOutSpeed;
                material.SetFloat("_FadeValue", fade);
                barrierNoise.volume -= Time.deltaTime / fadeOutSpeed;

                if(fade >= 1) {
                    gameObject.SetActive(false);
                }
            }
        }

        bool IsConditionSoulClaimed(int unlockLevel) {
            Debug.Log(unlockLevel);
            Debug.Log((int)GameManager.Instance.SoulLevels);
            return unlockLevel >= (int)GameManager.Instance.SoulLevels;
        }

        public void OnInteract() {
            Open();
        }

        public void Open() {
            if((int)GameManager.Instance.SoulLevels >= GateLevel) {
                initiateFadeOut = true;
                barrierCollider.enabled = false;
                soundBuilder.WithPosition(transform.position).Play(soundLibrary.soundData);
            }
        }

        public void Close() {
            barrierCollider.enabled = true;
        }
    }
}
