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

        public void Cast(bool input) {
            if(input) {
                Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                if(Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
                    if(hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                        interactObj.OnInteract();
                    }
                    else {
                        // if it's not the ground than play sound
                    }
                }
            }

        }
    }
}
