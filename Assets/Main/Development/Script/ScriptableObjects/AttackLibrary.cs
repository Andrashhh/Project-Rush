using Unity.VisualScripting;
using UnityEngine;

namespace Root
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Scriptable Objects/New Attack")]
    public class AttackLibrary : ScriptableObject
    {
        public GameObject HitboxPrefab;
        
        public float HitboxPersistence;
        public int Damage;

        public void CastHitboxInParent(Transform parent) {
            HitboxPrefab.GetComponent<CastEnemyHitbox>().Persistence = HitboxPersistence;
            HitboxPrefab.GetComponent<CastEnemyHitbox>().Damage = Damage;

            Instantiate(HitboxPrefab, parent);
        }
    }
}
