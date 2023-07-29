using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Start,
    World,
    Pause,
    Battle
}

[CreateAssetMenu(fileName = "Game Manager", menuName = "ScriptableObjects/Managers/GameManager")]
public class GameManagerSO : ScriptableObject {
    private GameState gameState = GameState.World; // While deving
    [SerializeField] private PlayerInputManagerSO input;

    private void OnEnable() {
        input.OnPause += HandlePause;
        input.OnResume += HandleResume;
    }

    private void OnDisable() {
        input.OnPause -= HandlePause;
        input.OnResume -= HandleResume;
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
                break;
            case GameState.Battle:
                break;
            default: // World
                Time.timeScale = 1f;
                SceneManager.UnloadSceneAsync("Pause");
                input.ChangeActionMap(PlayerInputActionMap.World);
                break;
        }
    }
}
