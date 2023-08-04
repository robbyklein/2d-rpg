using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryActions : MonoBehaviour {
    [SerializeField] GameManagerSO gameManager;

    public void MainMenu() {
        gameManager.ChangeGameState(GameState.Pause);
    }
}
