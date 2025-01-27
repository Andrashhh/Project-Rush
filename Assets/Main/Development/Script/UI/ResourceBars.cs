using UnityEngine;
using UnityEngine.UI;

namespace Root
{
    public class ResourceBars : MonoBehaviour
    {
        [SerializeField] Slider healthBar;


        void OnEnable() {
            PlayerHealth.OnPlayerHealthChanged += UpdateHealthBar;
        }

        void OnDisable() {
            PlayerHealth.OnPlayerHealthChanged -= UpdateHealthBar;

        }

        void UpdateHealthBar(object source, int oldHealth, int currentHealth) {

            healthBar.value = currentHealth;
        }
    }
}
