using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Root
{
    public enum Slot {
        First, Second, Third, Fourth
    }

    public struct AbilityInput {
        public bool Slot1;
        public bool Slot2;
        public bool Slot3;
        public bool Slot4;
    }

    public class AbilitySlots : MonoBehaviour
    {
        [SerializeField] GameObject slot1Object;
        [SerializeField] GameObject slot2Object;
        [SerializeField] GameObject slot3Object;
        [SerializeField] GameObject slot4Object;

        Slider slot1Slider;
        Slider slot2Slider;
        Slider slot3Slider;
        Slider slot4Slider;

        public static Action OnAbility1;
        public static Action OnAbility2;
        public static Action OnAbility3;
        public static Action OnAbility4;

        bool isCastForbid;

        void OnEnable() {
            PlayerArms.OnCastForbid += BlockCasting;
        }

        void OnDisable() {
            PlayerArms.OnCastForbid -= BlockCasting;

        }

        void Update() {
            UpdateSliderCD(slot1Slider);
            UpdateSliderCD(slot2Slider);
            UpdateSliderCD(slot3Slider);
            UpdateSliderCD(slot4Slider);

            SetAllToMaxCD();
        }

        public void Initialize() {
            LoadAllSliderComponents();
            ResetAllCD();
        }

        public void HideSlot(GameObject slot, bool isHidden) {
            slot.SetActive(isHidden);
        }

        public void UpdateMaxCD(Slot slot, float cd) {
            switch(slot) {
                case Slot.First:
                    slot1Slider.maxValue = cd;
                    break;
                case Slot.Second:
                    slot2Slider.maxValue = cd;
                    break;
                case Slot.Third:
                    slot3Slider.maxValue = cd;
                    break;
                case Slot.Fourth:
                    slot4Slider.maxValue = cd;
                    break;
                default:
                    break;
            }
        }

        public void UpdateSlots(AbilityInput input) {
            if(isCastForbid) {
                return;
            }
            if(input.Slot1 && slot1Slider.value <= 0f) {
                OnCastingAbility1();
            }
            if(input.Slot2 && slot2Slider.value <= 0f) {
                OnCastingAbility2();
            }
            if(input.Slot3 && slot3Slider.value <= 0f) {
                OnCastingAbility3();
            }
            if(input.Slot4 && slot4Slider.value <= 0f) {
                OnCastingAbility4();
            }
        }

        void OnCastingAbility1() {
            slot1Slider.value = slot1Slider.maxValue;
            OnAbility1?.Invoke();
        }

        void OnCastingAbility2() {
            slot2Slider.value = slot2Slider.maxValue;
            OnAbility2?.Invoke();
        }

        void OnCastingAbility3() {
            slot3Slider.value = slot3Slider.maxValue;
            OnAbility3?.Invoke();
        }

        void OnCastingAbility4() {
            slot4Slider.value = slot4Slider.maxValue;
            OnAbility4?.Invoke();
        }

        void UpdateSliderCD(Slider slot) {
            if(slot.value > 0f) {
                slot.value -= Time.deltaTime;
            }
        }

        void BlockCasting(bool boolean) {
            isCastForbid = boolean;
        }

        void LoadAllSliderComponents() {
            slot1Slider = slot1Object.GetComponentInChildren<Slider>();
            slot2Slider = slot2Object.GetComponentInChildren<Slider>();
            slot3Slider = slot3Object.GetComponentInChildren<Slider>();
            slot4Slider = slot4Object.GetComponentInChildren<Slider>();
        }

        void ResetCD(Slider slot) {
            slot.value = slot.minValue;
        }

        public void ResetAllCD() {
            ResetCD(slot1Slider);
            ResetCD(slot2Slider);
            ResetCD(slot3Slider);
            ResetCD(slot4Slider);
        }

        void SetAllToMaxCD() {
            if(Keyboard.current.yKey.wasPressedThisFrame) {
                slot1Slider.value = slot1Slider.maxValue;
                slot2Slider.value = slot2Slider.maxValue;
                slot3Slider.value = slot3Slider.maxValue;
                slot4Slider.value = slot4Slider.maxValue;
            }
        }
    }
}
