using UnityEngine;

namespace Root
{
    public struct CameraInput {
        public Vector2 Look;
    }

    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] float cameraDefaultSensitivity = .1f;
        [Space]
        [SerializeField] float FOV = 90f;
        [SerializeField] float FOVChangeResponsiveness = .2f;
        [SerializeField] float sprintFovStrength = 1.2f;

        Vector3 eulerAngles;

        float calculatedFOV;

        public void Initialize(Transform target) {
            transform.position = target.position;
            transform.eulerAngles = eulerAngles = target.eulerAngles;
        }

        public void UpdateRotation(CameraInput input) {
            eulerAngles += new Vector3(-input.Look.y, input.Look.x) * cameraDefaultSensitivity;
            eulerAngles.x = Mathf.Clamp(eulerAngles.x, -89.9f, 89.9f);
            transform.eulerAngles = eulerAngles;
        }

        public void UpdatePosition(Transform target) {
            transform.position = target.position;
        }

        public void UpdateFov(CharacterState state, CharacterInput input) {
            var originalFOV = FOV;
            var cameraCurrentFOV = Camera.main.fieldOfView;

            var velX = state.Velocity.x;
            var velZ = state.Velocity.z;
            Vector2 velocity = new Vector2(velX, velZ);

            var magnitude = velocity.magnitude;

            var targetFOV = originalFOV + magnitude * sprintFovStrength;

            if(input.Sprint || state.Stance is Stance.Slide || state.SlideJump) {
                calculatedFOV = targetFOV;
            }
            else {
                calculatedFOV = originalFOV;
            }
            calculatedFOV = Mathf.Clamp(calculatedFOV, originalFOV, originalFOV + 60);

            cameraCurrentFOV = Mathf.Lerp(cameraCurrentFOV, calculatedFOV, 1f - Mathf.Exp(-FOVChangeResponsiveness * Time.deltaTime));
            Camera.main.fieldOfView = cameraCurrentFOV;
        }
    }
}
