using System;
using UnityEngine;

namespace Root
{
    public class PlayerArms : MonoBehaviour
    {
        public Animator ArmsAnimator;

        public static Action<bool> OnCastForbid;

        void OnEnable() {
            AbilitySlots.OnAbility1 += () => TriggerAbility("Ability_1");
            AbilitySlots.OnAbility2 += () => TriggerAbility("Ability_2");
            AbilitySlots.OnAbility3 += () => TriggerAbility("Ability_3");
            AbilitySlots.OnAbility4 += () => TriggerAbility("Ability_4");

            
        }

        void OnDisable() {
            AbilitySlots.OnAbility1 -= () => TriggerAbility("Ability_1");
            AbilitySlots.OnAbility2 -= () => TriggerAbility("Ability_2");
            AbilitySlots.OnAbility3 -= () => TriggerAbility("Ability_3");
            AbilitySlots.OnAbility4 -= () => TriggerAbility("Ability_4");
        }

        void TriggerAbility(string name) {
            ArmsAnimator.SetTrigger(name);
            OnCastForbid?.Invoke(true);
        }

        void EnableCast() => OnCastForbid?.Invoke(false);
    }
}
