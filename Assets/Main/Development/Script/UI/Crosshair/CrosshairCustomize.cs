using UnityEngine;
using UnityEngine.UI;

namespace Root
{
    public class CrosshairCustomize : MonoBehaviour
    {
        [SerializeField] GameObject Vertical;
        [SerializeField] GameObject Horizontal;

        RectTransform verticalRect;
        RawImage verticalRawImage;

        RectTransform horizontalRect;
        RawImage horizontalRawImage;

        [SerializeField] float vLength;
        [SerializeField] float vThickness;
        [SerializeField] float hLength;
        [SerializeField] float hThickness;
        [SerializeField] float transparency;
        [SerializeField] Color color;

        void Awake() {
            verticalRect = Vertical.GetComponent<RectTransform>();
            verticalRawImage = Vertical.GetComponent<RawImage>();

            horizontalRect = Horizontal.GetComponent<RectTransform>();
            horizontalRawImage = Horizontal.GetComponent<RawImage>();
        }

        void Start()
        {
            DefaultPreset();

            
        }

        void Update()
        {
            ApplySettings();
        }

        public void ApplySettings() {
            //verticalRect.rect.size.Set(vThickness, vLength);
            //horizontalRect.rect.size.Set(hLength, hThickness);
            verticalRect.sizeDelta = new Vector2(vThickness, vLength);
            horizontalRect.sizeDelta = new Vector2(hLength, hThickness);
            
            verticalRawImage.color = color;
            horizontalRawImage.color = color;
        }

        void DefaultPreset() {
            vLength = 15f;
            vThickness = 3f;
            hLength = 15f;
            hThickness = 3f;
            transparency = 1f;
            color = new Color(1, .5f, .5f, transparency);
        }
    }
}
