using System;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    // Components
    [SerializeField] private DatabaseSO db;
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private BattlePlayerTurn playerTurn;
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private SpriteRenderer enemySprite;

    // State
    private bool playerFirst;
    private int turn = 0;
    private EnemyInfo enemyInfo;

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
        initiateBattle();
    }
    #endregion

    private void initiateBattle() {
        // See who goes first
        playerFirst = UnityEngine.Random.value > 0.5f;

        // Get an enemy to fight
        enemyInfo = enemyData.GetEnemyData(db.Data.PlayerData.Level);

        // Set enemy sprite
        System.Random rand = new System.Random();
        int randomSpriteIndex = rand.Next(enemyInfo.Sprites.Count);
        enemySprite.sprite = enemyInfo.Sprites[randomSpriteIndex];

        Debug.Log($"Battle initiated! You're fighting: // Type: ${enemyInfo.Type}\nAttack: ${enemyInfo.Attack} // Health: ${enemyInfo.Health} // Defense: ${enemyInfo.Defense}");

        // Start battle
        if (playerFirst) {
            // Enable battle map so they can go
            playerInput.ChangeActionMap(PlayerInputActionMap.Battle);
        }
        else {
            Debug.Log("Enemy goes first");
            EnemyTurn();
        }
    }


    private void EnemyTurn() {
        Debug.Log("Enemy does something");
        turn++;
        OnEnemyTurnFinished?.Invoke();
    }

    private void HandlePlayerTurnFinished() {
        Debug.Log("The player has finished their turn");
        turn++;
        EnemyTurn();
    }
}
