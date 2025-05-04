using UnityEngine;

namespace Root
{
    public class Ballerine : MonoBehaviour
    {
        public SoundLibrary SL;
        public SoundBuilder SB;

        Animator animator;
        public LayerMask layerMask;

        bool triggeredOnce;


        void Awake() {
            triggeredOnce = false;

            animator = gameObject.GetComponent<Animator>();

            SB = SoundManager.Instance.CreateSoundBuilder();
        }

        void Update() {
            var t = Time.time;
            if(Physics.SphereCast(gameObject.transform.position, 10f, -gameObject.transform.forward / 100f, out RaycastHit hit, 120f, layerMask) && triggeredOnce == false) {
                
                triggeredOnce = true;
                Debug.Log("I see you");
                animator.SetTrigger("Transfer");
                SB.WithPosition(gameObject.transform.position).Play(SL.soundData);
            }
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gameObject.transform.position - (gameObject.transform.forward / 100) * 120f, 10f);
        }
    }
}
