using UnityEngine;
using UnityEngine.UI;

namespace Root
{
    public class Damageable : MonoBehaviour
    {
        public GameObject Corpse;

        public SoundLibrary soundLibrary;
        public SoundBuilder soundBuilder;

        public Health Health = new(1000, 1000);
        public Slider HealthBar;

        void Awake() {
            Health.SetValues(1000, 1000);

            soundBuilder = SoundManager.Instance.CreateSoundBuilder();

            HealthBar.maxValue = Health.MaxHealth;
            HealthBar.value = Health.CurrentHealth;
        }

        void Update() {
            HealthBar.transform.rotation = Camera.main.transform.rotation;
        }

        void OnTriggerEnter(Collider other) {
            print("doo-doo head");

            HealthBar.maxValue = Health.MaxHealth;
            HealthBar.value = Health.CurrentHealth;

            soundBuilder.WithPosition(transform.position).WithRandomPitch().Play(soundLibrary.soundData);

            if(Health.CurrentHealth <= 0) {
                Instantiate(Corpse, transform.position, Corpse.transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
