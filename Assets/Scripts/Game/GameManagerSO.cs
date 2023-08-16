using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    MainMenu,
    World,
    Pause,
    Inventory,
    Battle
}

[CreateAssetMenu(fileName = "Game Manager", menuName = "ScriptableObjects/Managers/GameManager")]
public class GameManagerSO : ScriptableObject {
    [SerializeField] private PlayerInputManagerSO input;
    [SerializeField] private GameObject pausePrefab;
    [SerializeField] private GameObject inventoryPrefab;

    private GameObject currentPause;
    private GameObject currentInventory;

    public event Action<GameState> onGameStateChange;

    #region Lifecycle
    private void OnEnable() {
        input.OnMenuSelect += HandleStartGame;
        input.OnWorldPause += HandlePause;
        input.OnPauseResume += HandleResume;
    }

    private void OnDisable() {
        input.OnMenuSelect -= HandleStartGame;
        input.OnWorldPause -= HandlePause;
        input.OnPauseResume -= HandleResume;
    }
    #endregion

    public void ChangeGameState(GameState state) {
        switch (state) {
            case GameState.MainMenu:
                break;
            case GameState.Pause:
                ChangeToPause();
                break;
            case GameState.Inventory:
                ChangeToInventory();
                break;
            case GameState.Battle:
                break;
            case GameState.World:
                ChangeToWorld();
                break;
        }

        onGameStateChange?.Invoke(state);
    }

    private void ChangeToWorld() {
        Time.timeScale = 1f;

        if (currentPause) Destroy(currentPause);
        if (currentInventory) Destroy(currentInventory);
        if (!IsSceneLoaded("World")) SceneManager.LoadSceneAsync("World");

        input.ChangeActionMap(PlayerInputActionMap.World);
    }

    private void ChangeToPause() {
        // Stop everything else
        Time.timeScale = 0f;

        // Destroy the inventory menu if needed
        if (currentInventory) Destroy(currentInventory);

        // Create a pause menu
        currentPause = Instantiate(pausePrefab);

        // Change the input map
        input.ChangeActionMap(PlayerInputActionMap.Pause);
    }

    private void ChangeToInventory() {
        // Stop everything else
        Time.timeScale = 0f;

        // Destroy the inventory menu if needed
        if (currentPause) Destroy(currentPause);

        // Create a pause menu
        currentInventory = Instantiate(inventoryPrefab);

        // Change the input map
        input.ChangeActionMap(PlayerInputActionMap.Pause);
    }

    #region Input Handlers
    private void HandleStartGame() {
        ChangeGameState(GameState.World);
    }

    private void HandleResume() {
        ChangeGameState(GameState.World);
    }

    private void HandlePause() {
        ChangeGameState(GameState.Pause);
    }
    #endregion

    #region SceneHelpers
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
    #endregion
}
