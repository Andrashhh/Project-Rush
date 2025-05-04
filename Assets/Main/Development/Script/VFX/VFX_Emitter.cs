using UnityEngine;

namespace Root
{
    public class VFX_Emitter : MonoBehaviour
    {
        public GameObject Left_VFX_Emitter;
        public GameObject Middle_VFX_Emitter;
        public GameObject Right_VFX_Emitter;
        [Space(10)]
        public GameObject Ability_1;
        public GameObject Ability_Beam;
        public GameObject Ability_Beam_Hitbox;

        Vector3 crosshairPosition;
        Quaternion crosshairRotation;

        // Update is called once per frame
        void Update()
        {
            //Left_VFX_Emitter.transform.rotation.SetLookRotation();
            //Right_VFX_Emitter.transform.rotation.SetLookRotation(Crosshair.Point);

            UpdateTransform(Ability_Beam, Middle_VFX_Emitter.transform);
        }

        void UpdateTransform(GameObject obj, Transform setTransform) {
            if(!obj) {
                return;
            }
            obj.transform.position = setTransform.position;
            //obj.transform.rotation = setTransform.rotation;
        }

        void PlayAbility_1() {
            var a = Instantiate(Ability_1, Left_VFX_Emitter.transform.position, Quaternion.identity);
            a.transform.LookAt(Crosshair.Point);
            Destroy(a, .1f);
        }

        void PlayAbility_Beam() {
            Instantiate(Ability_Beam, Middle_VFX_Emitter.transform.position, Middle_VFX_Emitter.transform.rotation, Middle_VFX_Emitter.transform);
        }

        void CastHitbox() {
            Instantiate(Ability_Beam_Hitbox, Middle_VFX_Emitter.transform.position, Middle_VFX_Emitter.transform.rotation, Middle_VFX_Emitter.transform);
        }
    }
}
