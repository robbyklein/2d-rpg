using System;
using System.Collections;
using UnityEngine;

enum MessageType {
    DamageGiven,
    DamageReceived,
    EscapeFailed,
    EnemyMissed,
    PlayerFirst,
    EnemyFirst,
    PlayerDefending,
    PlayerUseItem,
    UsedItem,
}

public class BattleManager : MonoBehaviour {
    #region Components
    [SerializeField] private AudioManagerSO audioManager;
    [SerializeField] private KeyLanguageSO keyLanguage;
    [SerializeField] private BattleMenuManager menuManager;
    [SerializeField] private DatabaseSO db;
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private ItemActions itemActions;
    [SerializeField] private GameManagerSO gameManager;
    #endregion

    #region State
    private bool playerFirst;
    private int turn = 0;
    private EnemyInfo enemyInfo;
    private bool playerDefending = false;
    #endregion

    #region Events
    public Action<string> OnDisplayMessage;
    #endregion

    #region Lifecycle
    private void OnEnable() {
        menuManager.OnBattleUseItem += UseItem;
    }

    private void OnDisable() {
        menuManager.OnBattleUseItem -= UseItem;
    }

    private void Start() {
        initiateBattle();
    }

    private void initiateBattle() {
        // See who goes first
        playerFirst = UnityEngine.Random.value > 0.5f;

        // Get an enemy to fight
        enemyInfo = enemyData.GetEnemyData(db.Data.PlayerData.Level);

        // Set enemy sprite
        System.Random rand = new System.Random();
        int randomSpriteIndex = rand.Next(enemyInfo.Sprites.Count);
        enemySprite.sprite = enemyInfo.Sprites[randomSpriteIndex];

        // Start battle
        if (playerFirst) {
            OnDisplayMessage?.Invoke(CreateMessage(MessageType.PlayerFirst, 0));
            playerInput.ChangeActionMap(PlayerInputActionMap.Battle);
        }
        else {
            OnDisplayMessage?.Invoke(CreateMessage(MessageType.EnemyFirst, 0));
            StartCoroutine(EnemyTurn());
        }
    }
    #endregion

    #region Player Actions
    public void Attack() {
        playerInput.DisableAllActionMaps();
        StartCoroutine(AttackRoutine());
    }

    public void Defend() {
        playerInput.DisableAllActionMaps();
        playerDefending = true;
        OnDisplayMessage?.Invoke(CreateMessage(MessageType.PlayerDefending, 0));
        StartCoroutine(EnemyTurn());
    }

    public void Run() {
        playerInput.DisableAllActionMaps();
        float randomRoll = UnityEngine.Random.value;

        if (randomRoll > 0.7) {
            gameManager.ChangeGameState(GameState.World);
        }
        else {
            OnDisplayMessage?.Invoke(CreateMessage(MessageType.EscapeFailed, 0));
            StartCoroutine(EnemyTurn());
        }
    }

    public void UseItem(ItemType type) {
        playerInput.DisableAllActionMaps();
        itemActions.UseItem(type);
        OnDisplayMessage?.Invoke(CreateMessage(MessageType.UsedItem, 0));
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator AttackRoutine() {
        // Calculate damagae
        float damageGiven = calculateAttackDamage();

        // Subtract it from enemy health
        enemyInfo.Health -= damageGiven;

        // Show movement
        yield return StartCoroutine(AttackMovement(playerSprite, enemySprite));

        // Check if the battles done
        CheckBattleFinished();

        // Display message
        OnDisplayMessage?.Invoke(CreateMessage(MessageType.DamageGiven, damageGiven));

        // Start enemy turn
        StartCoroutine(EnemyTurn());

        yield return null;
    }
    #endregion

    #region Enemy Actions
    private IEnumerator EnemyTurn() {
        // Delay to make it feel more natural
        yield return new WaitForSeconds(2f);

        // Generate a random number
        float randomRoll = UnityEngine.Random.value;

        // Did they miss?
        if (randomRoll <= 0.2) {
            OnDisplayMessage?.Invoke(CreateMessage(MessageType.EnemyMissed, 0));
        }
        else {
            // Move toward the player
            yield return StartCoroutine(AttackMovement(enemySprite, playerSprite));

            // Grab the player stats
            PlayerData playerStats = db.Data.PlayerData;

            // Calculate damage
            float damageGiven = calculateDamageTaken();

            // Enemy tired
            if (turn > 10) damageGiven *= 0.7f;

            // Display message
            OnDisplayMessage?.Invoke(CreateMessage(MessageType.DamageReceived, damageGiven));

            // Subtract damage
            playerStats.Health -= damageGiven;
            db.UpdatePlayerData(playerStats);
        }

        // End the turn
        yield return new WaitForSeconds(1f);
        FinishEnemyTurn();
    }

    private void FinishEnemyTurn() {
        playerDefending = false;
        turn++;
        playerInput.ChangeActionMap(PlayerInputActionMap.Battle);
        CheckBattleFinished();
    }
    #endregion

    #region Helpers
    private string CreateMessage(MessageType messageType, float damage) {
        switch (messageType) {
            case MessageType.EnemyMissed:
                return keyLanguage.RetrieveValue("enemyMissed");
            case MessageType.EscapeFailed:
                return keyLanguage.RetrieveValue("runFailed");
            case MessageType.DamageGiven:
                return ReplaceMessageDamage(keyLanguage.RetrieveValue("damageGiven"), damage);
            case MessageType.DamageReceived:
                return ReplaceMessageDamage(keyLanguage.RetrieveValue("damageReceived"), damage);
            case MessageType.EnemyFirst:
                return ReplaceMessageEnemy(keyLanguage.RetrieveValue("enemyFirst"));
            case MessageType.PlayerFirst:
                return ReplaceMessageEnemy(keyLanguage.RetrieveValue("playerFirst"));
            case MessageType.PlayerDefending:
                return keyLanguage.RetrieveValue("playerDefending");
            case MessageType.UsedItem:
                return keyLanguage.RetrieveValue("playerUsedItem");
            default:
                return "";
        }
    }

    private string ReplaceMessageEnemy(string message) {
        string messagePost = message.Replace("{$}", enemyInfo.Name);
        return messagePost;
    }

    private string ReplaceMessageDamage(string message, float damage) {
        int damageGiven = (int)Math.Ceiling(damage);
        string messagePost = message.Replace("{$}", damageGiven.ToString());
        return messagePost;
    }

    private void CheckBattleFinished() {
        PlayerData playerStats = db.Data.PlayerData;

        Debug.Log($"Player {playerStats.Health}. Enemy: {enemyInfo.Health}");

        if (enemyInfo.Health <= 0) {
            gameManager.ChangeGameState(GameState.World);
        }
        else if (playerStats.Health <= 0) {
            gameManager.ChangeGameState(GameState.GameOver);
        }

    }

    private float calculateAttackDamage() {
        PlayerData playerStats = db.Data.PlayerData;
        return playerStats.Level * 80 * (1 - (enemyInfo.Defense / 100));
    }

    private float calculateDamageTaken() {
        float multiplayer = 0.5f + UnityEngine.Random.value;

        PlayerData playerStats = db.Data.PlayerData;
        float damage = enemyInfo.Attack * multiplayer * (1 - (playerStats.Defense / 100));

        if (playerDefending) damage *= 0.5f;

        return damage;
    }

    private IEnumerator AttackMovement(SpriteRenderer attacker, SpriteRenderer target) {
        int initialSortingOrder = attacker.sortingOrder;
        Vector3 initialPosition = attacker.transform.position;
        Vector3 targetPosition = target.transform.position;

        attacker.sortingOrder = 999;

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            attacker.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioManager.PlaySFX(SoundClip.Attack);

        elapsedTime = 0f;
        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            attacker.transform.position = Vector3.Lerp(targetPosition, initialPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        attacker.sortingOrder = initialSortingOrder;
    }
    #endregion
}
