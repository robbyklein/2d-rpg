using System;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private BattlePlayerTurn playerTurn;

    private bool playerFirst = UnityEngine.Random.value > 0.5f;
    private int turn = 0;

    public event Action OnEnemyTurnFinished;

    #region Lifecycle
    private void OnEnable() {
        Debug.Log("Subsribing to player turn finished");
        playerTurn.OnPlayerTurnFinished += HandlePlayerTurnFinished;
    }

    private void OnDisable() {
        playerTurn.OnPlayerTurnFinished -= HandlePlayerTurnFinished;
    }

    private void Start() {
        if (playerFirst) {
            Debug.Log("Player will go first");
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
