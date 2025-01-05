using UnityEngine;

namespace Root
{
    public class CameraLean : MonoBehaviour
    {
        [SerializeField] float attackDamping = .5f;
        [SerializeField] float decayDamping = .3f;
        [SerializeField] float walkStrenghtMultiplier = .75f;
        [SerializeField] float slideStrenghtMultiplier = .8f;
        [SerializeField] float changeResponsiveness = 5f;

        Vector3 dampedAcceleration;
        Vector3 dampedAccelerationVelocity;

        float smoothStrength;

        public void Initialize() {
            smoothStrength = walkStrenghtMultiplier;
        }

        public void UpdateLean(float deltaTime, CharacterState state, Vector3 acceleration, Vector3 velocity, Vector3 up) {
            var planarAcceleration = Vector3.ProjectOnPlane(acceleration, up);
            var damping = planarAcceleration.magnitude > dampedAcceleration.magnitude ? attackDamping : decayDamping;
            dampedAcceleration = Vector3.SmoothDamp(dampedAcceleration, planarAcceleration, ref dampedAccelerationVelocity, damping, float.PositiveInfinity, deltaTime);
            var leanAxis = Vector3.Cross(dampedAcceleration.normalized, up).normalized;
            transform.localRotation = Quaternion.identity;

            var targetStrength = walkStrenghtMultiplier;
            smoothStrength = Mathf.Lerp(smoothStrength, targetStrength, 1f - Mathf.Exp(-changeResponsiveness * deltaTime));

            targetStrength = state.Stance is Stance.Slide ? Mathf.SmoothStep(walkStrenghtMultiplier, slideStrenghtMultiplier, 1f - Mathf.Exp(-changeResponsiveness * deltaTime)) : walkStrenghtMultiplier;
            targetStrength = (targetStrength - (velocity.magnitude / 100f));

            transform.rotation = Quaternion.AngleAxis(-dampedAcceleration.magnitude * (targetStrength), leanAxis) * transform.rotation;
        }
    }
}
