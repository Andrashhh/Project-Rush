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

    public class Interact : MonoBehaviour
    {
        public Transform InteractSource;
        public float InteractRange;

        public void Cast(bool input, PlayerSounds playerSounds) {
            if(input) {
                Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.magenta);
                if(Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
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
