using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseActions : MonoBehaviour {
    [SerializeField] GameManagerSO gameManager;

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void Resume() {
        gameManager.ChangeGameState(GameState.World);
    }

    public void OpenInventory() {
        gameManager.ChangeGameState(GameState.Inventory);
    }
}
