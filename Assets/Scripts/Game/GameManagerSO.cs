using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Start,
    World,
    Pause,
    Inventory,
    Battle
}

[CreateAssetMenu(fileName = "Game Manager", menuName = "ScriptableObjects/Managers/GameManager")]
public class GameManagerSO : ScriptableObject {
    //private GameState gameState = GameState.World; // While deving
    [SerializeField] private PlayerInputManagerSO input;

    private void OnEnable() {
        input.OnWorldPause += HandlePause;
        input.OnPauseResume += HandleResume;
    }

    private void OnDisable() {
        input.OnWorldPause -= HandlePause;
        input.OnPauseResume -= HandleResume;
    }

    private void HandleResume() {
        ChangeGameState(GameState.World);
    }

    private void HandlePause() {
        ChangeGameState(GameState.Pause);
    }

    public void ChangeGameState(GameState state) {
        switch (state) {
            case GameState.Start:
                break;
            case GameState.Pause:
                Time.timeScale = 0f;
                input.ChangeActionMap(PlayerInputActionMap.Pause);
                SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
                UnloadScene("Inventory");
                break;
            case GameState.Inventory:
                Time.timeScale = 0f;
                input.ChangeActionMap(PlayerInputActionMap.Pause);
                SceneManager.LoadSceneAsync("Inventory", LoadSceneMode.Additive);
                UnloadScene("Pause");
                break;
            case GameState.Battle:
                break;
            default: // World
                Time.timeScale = 1f;
                UnloadScene("Pause");
                UnloadScene("Inventory");
                input.ChangeActionMap(PlayerInputActionMap.World);
                break;
        }
    }

    private void UnloadScene(string scene) {
        if (IsSceneLoaded(scene)) {
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    private bool IsSceneLoaded(string sceneName) {
        for (int i = 0; i < SceneManager.sceneCount; ++i) {
            if (SceneManager.GetSceneAt(i).name == sceneName) {
                return true;
            }
        }
        return false;
    }
}
