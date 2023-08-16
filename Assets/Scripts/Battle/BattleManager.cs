using System;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    // Components
    [SerializeField] private DatabaseSO db;
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private BattlePlayerTurn playerTurn;

    // State
    private bool playerFirst;
    private int turn = 0;
    private Enemy enemy;

    // Events
    public event Action OnEnemyTurnFinished;

    #region Lifecycle
    private void OnEnable() {
        playerTurn.OnPlayerTurnFinished += HandlePlayerTurnFinished;
    }

    private void OnDisable() {
        playerTurn.OnPlayerTurnFinished -= HandlePlayerTurnFinished;
    }

    private void Start() {
        playerFirst = UnityEngine.Random.value > 0.5f;

        // Figure out who we're fighting
        enemy = new Enemy(db.Data.PlayerData.Level);

        if (playerFirst) {
            // Enable battle map so they can go
            playerInput.ChangeActionMap(PlayerInputActionMap.Battle);
        }
        else {
            Debug.Log("Enemy goes first");
            EnemyTurn();
        }
    }
    #endregion

    private void EnemyTurn() {
        Debug.Log("Enemy does something");
        turn++;
        OnEnemyTurnFinished?.Invoke();
        Debug.Log(turn);
    }

    private void HandlePlayerTurnFinished() {
        Debug.Log("The player has finished their turn");
        turn++;
        EnemyTurn();
    }
}
