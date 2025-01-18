using UnityEngine;
using UnityEngine.UIElements;

namespace Root
{
    public class Barrier : MonoBehaviour, IInteractable
    {
        MeshCollider barrierCollider;
        public Material Material;

        public float Fade;
        bool initiateFadeOut;


        void Awake() {
            barrierCollider = gameObject.GetComponent<MeshCollider>();
            Material = gameObject.GetComponent<MeshRenderer>().material;

        }

        void Start() {
            Fade = 0f;

            Close();
        }

        void Update() {
            FadeOut();
        }

        void FadeOut() {
            if(initiateFadeOut && Fade < 1) {
                Fade += Time.deltaTime;
                Material.SetFloat("_FadeValue", Fade);

                if(Fade >= 1) {
                    gameObject.SetActive(false);
                }
            }
        }

        public void OnInteract() {
            initiateFadeOut = true;
            Open();
        }

        public void Open() {
            barrierCollider.enabled = false;
        }

        public void Close() {
            barrierCollider.enabled = true;
        }
    }
}
