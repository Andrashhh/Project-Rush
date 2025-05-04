using System;
using UnityEngine;

namespace Root
{
    public enum GameState {
        PLAYING,
        PAUSE,
        DEATH,
        MENU,
    }

    public enum SoulLevels {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [NonSerialized] public GameState State;
        [NonSerialized] public SoulLevels SoulLevels;

        public static Action<GameState> OnGameStateChanged;

        

        void Awake() {
            GetInstance();

            SoulLevels = SoulLevels.Level0;

        }

        void Start() {
            UpdateGameState(GameState.MENU);
        }

        public void UpdateGameState(GameState newState) {
            State = newState;

            switch(State) {
                case GameState.PLAYING:
                    Cursor.lockState = CursorLockMode.Locked;
                    Time.timeScale = 1f;
                    break;
                case GameState.PAUSE:
                    Cursor.lockState = CursorLockMode.None;
                    Time.timeScale = 0f;
                    break;
                case GameState.DEATH:
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case GameState.MENU:
                    Cursor.lockState = CursorLockMode.None;
                    break;
                default:
                    break;
            }

            OnGameStateChanged?.Invoke(newState);
        }

        void GetInstance() {
            if(Instance != null) {
                Destroy(this);
                return;
            }
            Instance = this;
        }
    }
}
