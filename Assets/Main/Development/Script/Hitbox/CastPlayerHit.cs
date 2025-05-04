using UnityEngine;

namespace Root
{
    public class CastPlayerHit : MonoBehaviour
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
            Damageable damageable = other.gameObject.GetComponentInParent<Damageable>();

            if(damageable != null) {
                //healthComponent.DebugHealth("Testing-testing... Attention please! The real Slim Shady, please stand up!");
                damageable.Health.ChangeHealth(Damage * -1);
            }
        }
    }
}
