using UnityEngine;

namespace Root
{
    public class Gate : MonoBehaviour
    {
        Barrier barrier;

        float sec;

        void Awake() {
            barrier = GetComponentInChildren<Barrier>();

            sec = 20f;
        }

        void Update() {
            
        }

        void OpenAfter10Sec() {
            
            
            if(sec > 0f) {
                sec -= Time.deltaTime;
            }
            if(sec <= 0f) {
                barrier?.Open();
            }
        }
    }
}
