using UnityEngine;

namespace Root
{
    public class Health
    {
        int oldHealth;
        int currentHealth;
        int currentMaxHealth;

        public int OldHealth {
            get {
                return oldHealth;
            }

            set {
                oldHealth = value;
            }
        }

        public int CurrentHealth {
            get {
                return currentHealth;
            }

            set {
                currentHealth = value;
            }
        }

        public int MaxHealth {
            get {
                return currentMaxHealth;
            }

            set {
                currentMaxHealth = value;
            }
        }

        public Health(int health, int maxHealth) {
            currentHealth = health;
            currentMaxHealth = maxHealth;
        }

        public void SetValues(int health, int maxHealth) {
            currentHealth = health;
            currentMaxHealth = maxHealth;
        }

        public void DamageEntity(int damageAmount) {
            if(currentHealth > 0) {
                currentHealth -= damageAmount;
            }
        }

        public void HealEntity(int healAmount) {
            if(currentHealth < currentMaxHealth) {
                currentHealth += healAmount;
            }
            if(currentHealth > currentMaxHealth) {
                currentHealth = currentMaxHealth;
            }
        }

        public void ChangeHealth(int amount) {
            oldHealth = currentHealth;
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        }

        public void DebugHealth(string s) {
            Debug.Log(s);
        }
    }
}
