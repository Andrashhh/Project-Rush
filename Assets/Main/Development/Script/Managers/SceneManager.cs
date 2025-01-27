using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Root
{
    public enum Scene {
        SCENE_MANAGER = 0,
        MAIN_MENU = 1,
        GAME = 2,
        LOADING = 3,
    }

    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        public GameObject LoadingScreen;

        public GameObject PauseCanvas;
        public GameObject DeathCanvas;

        public bool CanCursorLock;

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        void OnEnable() {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        }


        void OnDisable() {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        void Awake() {
            GetInstance();
            
            LoadSceneAdditive(Scene.MAIN_MENU);
        }

        public void DebugText() {
            Debug.Log("waaa");
        }

        public void ResumeGame() {
            GameManager.Instance.UpdateGameState(GameState.PLAYING);
        }

        public void PauseGame() {
            GameManager.Instance.UpdateGameState(GameState.PAUSE);
        }

        public void LoadGame() {
            ShowLoading(true);

            scenesLoading.Add(UnloadScene(Scene.MAIN_MENU));
            scenesLoading.Add(LoadSceneAdditive(Scene.GAME));


            StartCoroutine(WaitForScenesLoading());

            ResumeGame();
        }

        public void RestartGame() {
            ShowLoading(true);

            scenesLoading.Add(UnloadScene(Scene.GAME));
            scenesLoading.Add(LoadSceneAdditive(Scene.GAME));

            StartCoroutine(WaitForScenesLoading());
            
            ResumeGame();
        }

        public void LoadMainMenu() {
            ShowLoading(true);

            scenesLoading.Add(UnloadScene(Scene.GAME));
            scenesLoading.Add(LoadSceneAdditive(Scene.MAIN_MENU));

            StartCoroutine(WaitForScenesLoading());
        }

        public void QuitGame() {
            Application.Quit();
        }

        public IEnumerator WaitForScenesLoading() {
            for(int i = 0; i < scenesLoading.Count; i++) {
                while(!scenesLoading[i].isDone) {
                    yield return null;
                }
            }
            scenesLoading.Clear();
            ShowLoading(false);
        }

        void GameManagerOnGameStateChanged(GameState state) {
            PauseCanvas.SetActive(state == GameState.PAUSE);
            DeathCanvas.SetActive(state == GameState.DEATH);
        }

        //
        //
        // Utilities
        void GetInstance() {
            if(Instance != null) {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        AsyncOperation LoadSceneAdditive(Scene scene) {
            return UnitySceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        }

        AsyncOperation UnloadScene(Scene scene) {
            return UnitySceneManager.UnloadSceneAsync((int)scene);
        }

        void ShowLoading(bool boolean) {
            if(boolean) {
                LoadingScreen.SetActive(boolean);
            }
            else if(!boolean) {
                LoadingScreen.SetActive(boolean);
            }
        }
    }
}
