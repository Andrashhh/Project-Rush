using UnityEngine;

namespace Root
{
    public class Barrier : MonoBehaviour, IInteractable
    {
        MeshCollider barrierCollider;
        public Material material;

        public float fade;
        bool initiateFadeOut;

        void Awake() {
            barrierCollider = gameObject.GetComponent<MeshCollider>();
            material = gameObject.GetComponent<MeshRenderer>().material;
        }

        void Start() {

            fade = 0f;

            Close();
        }

        void Update() {
            print("keep runnin runnin");
            FadeOut();
        }

        void FadeOut() {
            if(initiateFadeOut && fade < 1) {
                fade += Time.deltaTime;
                material.SetFloat("_FadeValue", fade);

                if(fade >= 1) {
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
            //print("oahn");
        }

        public void Close() {
            barrierCollider.enabled = true;
        }
    }
}
