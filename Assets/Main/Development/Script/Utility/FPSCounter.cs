using TMPro;
using UnityEngine;

namespace Root
{
    public class FPSCounter : MonoBehaviour
    {
        TMP_Text text;

        /* Assign this script to any object in the Scene to display frames per second */
        public float updateInterval = 0.5f; //How often should the number update

        float accum = 0.0f;
        int frames = 0;
        float timeleft;
        public static float fps;

        void Awake() {
            text = GetComponent<TMP_Text>();
        }

        // Use this for initialization
        void Start() {

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 360;

            timeleft = updateInterval;
        }

        // Update is called once per frame
        void Update() {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if(timeleft <= 0.0) {
                // display two fractional digits (f2 format)
                fps = (accum / frames);
                timeleft = updateInterval;
                accum = 0.0f;
                frames = 0;
            }

            text.text = fps.ToString();
        }
    }
}
