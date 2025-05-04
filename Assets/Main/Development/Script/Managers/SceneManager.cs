using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Root {
    public enum SceneIndex {
        PERSISTENT = 0,
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

        UnityEngine.SceneManagement.Scene sceneToLoad;

        List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

        void OnEnable() {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        }

        void OnDisable() {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
        }

        void Awake() {
            GetInstance();

            InitialStart();
        }

        public void InitialStart() {
            LoadSceneAdditive(SceneIndex.MAIN_MENU);
        }

        public void ResumeGame() {
            GameManager.Instance.UpdateGameState(GameState.PLAYING);
        }

        public void PauseGame() {
            GameManager.Instance.UpdateGameState(GameState.PAUSE);
        }

        public void LoadGame() {
            ShowLoading(true);

            scenesLoading.Add(UnloadScene(SceneIndex.MAIN_MENU));
            scenesLoading.Add(LoadSceneAdditive(SceneIndex.GAME));


            StartCoroutine(WaitForScenesLoading(SceneIndex.GAME, true));


            ResumeGame();
        }

        public void RestartGame() {
            ShowLoading(true);

            scenesLoading.Add(UnloadScene(SceneIndex.GAME));
            scenesLoading.Add(LoadSceneAdditive(SceneIndex.GAME));

            StartCoroutine(WaitForScenesLoading(SceneIndex.GAME, true));
            
            ResumeGame();
        }

        public void LoadMainMenu() {
            ShowLoading(true);

            scenesLoading.Add(UnloadScene(SceneIndex.GAME));
            scenesLoading.Add(LoadSceneAdditive(SceneIndex.MAIN_MENU));

            StartCoroutine(WaitForScenesLoading(SceneIndex.PERSISTENT, true));
        }

        public void LoadOnlyMainMenu() {

        }

        public void QuitGame() {
            Application.Quit();
        }

        public IEnumerator WaitForScenesLoading(SceneIndex sceneEnum, bool shouldSetActive) {
            for(int i = 0; i < scenesLoading.Count; i++) {
                while(!scenesLoading[i].isDone) {
                    yield return null;
                }
            }

            var unityScene = UnitySceneManager.GetSceneByBuildIndex((int)sceneEnum);

            if(shouldSetActive) {
                UnitySceneManager.SetActiveScene(unityScene);
            }

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

        AsyncOperation LoadSceneAdditive(SceneIndex scene) {
            return UnitySceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        }

        AsyncOperation UnloadScene(SceneIndex scene) {
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
