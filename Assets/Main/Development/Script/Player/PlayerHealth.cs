using System;
using Unity.Properties;
using UnityEngine;

namespace Root
{
    public class PlayerHealth : MonoBehaviour
    {
        public Health health;

        //public static event Action<float, float> onPlayerHealthChange;
        public delegate void HealthChangedHandler(object source, int oldHealth, int newHealth);
        public static event HealthChangedHandler OnPlayerHealthChanged;

        void Awake() {
            //health = gameObject.AddComponent<Health>(); MonoBehaviour
        }

        void Start() {
            InitialSet(80, 100);
        }

        public void InitialSet(int initialHealth, int initialMaxHealth) {
            health = new Health(initialHealth, initialMaxHealth);

            OnPlayerHealthChanged?.Invoke(this, health.OldHealth, health.CurrentHealth);
        }

        public void ChangeHealth(int amount) {
            health.ChangeHealth(amount);

            OnPlayerHealthChanged?.Invoke(this, health.OldHealth, health.CurrentHealth);
        }
    }
}
