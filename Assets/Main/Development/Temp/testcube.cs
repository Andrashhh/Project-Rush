using UnityEngine;

namespace Root
{
    public class testcube : MonoBehaviour, IInteractable
    {
        //[SerializeField] SoundLibrary sound;

        public void OnInteract() {
            //sound.StopSound();
        }

        //// Start is called once before the first execution of Update after the MonoBehaviour is created
        //void Start()
        //{
        //    sound.CreateSoundBuilder();
        //    sound.PlaySound(transform.position);
        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}
