using UnityEngine;

namespace Root
{
    public class CameraSpring : MonoBehaviour
    {
        [Min(0.01f)]
        [SerializeField] float dampingRation = 0.9f;
        [SerializeField] float angularFrequency = 18f;
        [SerializeField] float angularDisplacement = 2f;
        [SerializeField] float linearDisplacement = 0.12f;

        Vector3 springPosition;
        Vector3 springVelocity;

        public void Initialize() {
            springPosition = transform.position;
            springVelocity = Vector3.zero;
        }

        public void UpdateSpring(float deltaTime, Vector3 up) {
            transform.localPosition = Vector3.zero;

            Spring(ref springPosition, ref springVelocity, transform.position, dampingRation, angularFrequency, deltaTime);

            var relativeSpringPosition = springPosition - transform.position;
            var springHeight = Vector3.Dot(relativeSpringPosition, up);

            transform.localEulerAngles = new Vector3(-springHeight * angularDisplacement, 0f, 0f);
            transform.position += relativeSpringPosition * linearDisplacement;
        }

        public static void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float dampingRatio, float angularFrequency, float timeStep) {
            float f = 1.0f + 2.0f * timeStep * dampingRatio * angularFrequency;
            float oo = angularFrequency * angularFrequency;
            float hoo = timeStep * oo;
            float hhoo = timeStep * hoo;
            float detInv = 1.0f / (f + hhoo);
            var detX = f * current + timeStep * velocity + hhoo * target;
            var detV = velocity + hoo * (target - current);
            current = detX * detInv;
            velocity = detV * detInv;
        }
    }
}
