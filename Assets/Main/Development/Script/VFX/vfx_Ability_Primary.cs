using Unity.Mathematics;
using UnityEngine;

namespace Root
{
    public class vfx_Ability_Primary : MonoBehaviour
    {
        //public GameObject origin;

        public LineRenderer lineRenderer;

        public GameObject Hitbox;

        void Start() {
            Instantiate(Hitbox, Crosshair.Point, Hitbox.transform.rotation);
        }

        // Update is called once per frame
        void Update()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, Crosshair.Point);
        }
    }
}
