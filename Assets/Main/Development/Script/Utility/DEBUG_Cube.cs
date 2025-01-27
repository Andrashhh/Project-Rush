using UnityEngine;

namespace Root
{
    public class DEBUG_Cube : MonoBehaviour, IInteractable
    {
        [SerializeField] AttackLibrary attack;

        public void OnInteract() {
            attack.CastHitboxInParent(transform);

        }

        //void OnCollisionEnter(Collision collision) {
        //    PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        //    Health healthComponent = collision.gameObject.GetComponent<PlayerHealth>().health;

        //    if(healthComponent != null) {
        //        healthComponent.DebugHealth("Testing-testing... Attention please! The real Slim Shady, please stand up!");
        //        playerHealth.ChangeHealth(-10);
        //    }
        //}
    }
}
