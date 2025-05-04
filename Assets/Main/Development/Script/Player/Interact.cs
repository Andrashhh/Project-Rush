using System;
using UnityEngine;

namespace Root
{
    interface IInteractable {
        public void OnInteract();
    }

    public struct InteractInput {
        public bool Interact;
    }

    public static class Crosshair {
        public static Vector3 Point;

        public static float MaximumLength = 200f;

        public static Vector3 GetDirection(Vector3 originPosition) {
            return Point - originPosition;
        }
    }

    public class Interact : MonoBehaviour
    {
        public Transform InteractSource;
        public float InteractRange;

        Ray crosshairRay;

        void FixedUpdate() {
            DrawCrosshair(ref crosshairRay, Crosshair.MaximumLength);

            Debug.DrawRay(Camera.main.transform.position, Crosshair.Point - Camera.main.transform.position, Color.magenta);

        }

        void DrawCrosshair(ref Ray castRay, float length) {;
            castRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            crosshairRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward * length);

            

            if(Physics.Raycast(castRay, out RaycastHit hitInfo, length, ~LayerMask.GetMask("Player", "PlayerAttacks"))) {
                //Debug.DrawRay(Camera.main.transform.position, hitInfo.point - Camera.main.transform.position, Color.magenta);
                Crosshair.Point = hitInfo.point;
            }
            else {
                Crosshair.Point = crosshairRay.GetPoint(length);
                //Debug.DrawRay(Camera.main.transform.position, crosshairRay.GetPoint(200) - Camera.main.transform.position, Color.magenta);
            }

            

        }

        public void Cast(bool input, PlayerSounds playerSounds) {
            if(input) {
                if(Physics.Raycast(crosshairRay, out RaycastHit hitInfo, InteractRange)) {
                    if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                        interactObj.OnInteract();
                        playerSounds.PlaySound(playerSounds.InteractSuccesful);
                    }
                    else {
                        playerSounds.PlaySound(playerSounds.InteractFailed);
                    }
                }
                else {
                    playerSounds.PlaySound(playerSounds.InteractFailed);

                }
            }
        }
    }
}
