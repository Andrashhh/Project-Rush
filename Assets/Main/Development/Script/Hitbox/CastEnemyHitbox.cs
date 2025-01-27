using UnityEngine;

namespace Root
{
    public class CastEnemyHitbox : MonoBehaviour
    {
        public int Damage;
        public float Persistence;

        //public HitboxToPlayerLogic(int damage, int upTime) {
        //    this.Damage = damage;
        //    this.Persistence = upTime;
        //}

        void OnEnable() {
            Destroy(gameObject, Persistence);
        }

        void OnTriggerEnter(Collider other) {
            PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();

            if(playerHealth != null) {
                //healthComponent.DebugHealth("Testing-testing... Attention please! The real Slim Shady, please stand up!");
                playerHealth.ChangeHealth(Damage * -1);
            }
        }
    }
}
