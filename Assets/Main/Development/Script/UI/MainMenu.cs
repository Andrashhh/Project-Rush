using UnityEngine;
using UnityEngine.UI;

namespace Root
{
    public class MainMenu : MonoBehaviour
    {
        public void OnPlayClicked() {
            SceneManager.Instance.LoadGame();
        }

        public void OnQuitClicked() {
            Application.Quit();
        }
    }
}
