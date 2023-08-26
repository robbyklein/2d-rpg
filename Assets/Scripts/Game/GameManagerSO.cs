using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    MainMenu,
    World,
    Pause,
    Inventory,
    Battle,
    GameOver,
}

[CreateAssetMenu(fileName = "Game Manager", menuName = "ScriptableObjects/Managers/GameManager")]
public class GameManagerSO : ScriptableObject {
    [SerializeField] private PlayerInputManagerSO input;
    [SerializeField] private GameObject pausePrefab;
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private DatabaseSO db;

    private GameObject currentPause;
    private GameObject currentInventory;

    public event Action<GameState> onGameStateChange;

    #region Lifecycle
    private void OnEnable() {
        input.OnWorldPause += HandlePause;
        input.OnPauseResume += HandleResume;
    }

    private void OnDisable() {
        input.OnWorldPause -= HandlePause;
        input.OnPauseResume -= HandleResume;
    }
    #endregion

    private GameObject FindPlayerVisual() {
        GameObject playerVisualInScene = GameObject.FindGameObjectWithTag("PlayerVisual");
        return playerVisualInScene;
    }

    public void ChangeGameState(GameState state) {
        switch (state) {
            case GameState.MainMenu:
                ChangeToMainMenu();
                break;
            case GameState.Pause:
                ChangeToPause();
                break;
            case GameState.Inventory:
                ChangeToInventory();
                break;
            case GameState.Battle:
                ChangeToBattle();
                break;
            case GameState.World:
                ChangeToWorld();
                break;
            case GameState.GameOver:
                ChangeToGameOver();
                break;
        }

        onGameStateChange?.Invoke(state);
    }

    private void ChangeToGameOver() {
        SceneManager.LoadSceneAsync("GameOver");
        input.ChangeActionMap(PlayerInputActionMap.Menu);
    }

    private void ChangeToMainMenu() {
        SceneManager.LoadSceneAsync("MainMenu");
        input.ChangeActionMap(PlayerInputActionMap.Menu);
    }

    private void ChangeToWorld() {
        // Unpause
        Time.timeScale = 1f;

        // Get rid of menus
        if (currentPause) Destroy(currentPause);
        if (currentInventory) Destroy(currentInventory);

        // Unload other scenes
        if (IsSceneLoaded("Battle")) SceneManager.UnloadSceneAsync("Battle");

        // Load world
        if (!IsSceneLoaded("World")) SceneManager.LoadSceneAsync("World");

        // Set action map
        input.ChangeActionMap(PlayerInputActionMap.World);
    }

    private void ChangeToBattle() {
        // Load battle scene
        SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
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
