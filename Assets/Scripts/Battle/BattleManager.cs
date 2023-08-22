using System;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // Components
    [SerializeField] private BattleMenuManager menuManager;
    [SerializeField] private DatabaseSO db;
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private EnemyDataSO enemyData;
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private ItemActions itemActions;

    // State
    private bool playerFirst;
    private int turn = 0;
    private EnemyInfo enemyInfo;

    private void OnEnable()
    {
        menuManager.OnBattleUseItem += UseItem;
    }

    private void OnDisable()
    {
        menuManager.OnBattleUseItem -= UseItem;
    }

    private void Start()
    {
        initiateBattle();
    }

    private void initiateBattle()
    {
        // See who goes first
        playerFirst = UnityEngine.Random.value > 0.5f;

        // Get an enemy to fight
        enemyInfo = enemyData.GetEnemyData(db.Data.PlayerData.Level);

        // Set enemy sprite
        System.Random rand = new System.Random();
        int randomSpriteIndex = rand.Next(enemyInfo.Sprites.Count);
        enemySprite.sprite = enemyInfo.Sprites[randomSpriteIndex];

        Debug.Log($"Battle initiated! You're fighting: // Type: {enemyInfo.Name} Attack: {enemyInfo.Attack} // Health: ${enemyInfo.Health} // Defense: {enemyInfo.Defense}");

        // Start battle
        if (playerFirst)
        {
            playerInput.ChangeActionMap(PlayerInputActionMap.Battle);
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    public void Attack()
    {
        playerInput.DisableAllActionMaps();
        StartCoroutine(AttackRoutine());
    }

    public void UseItem(ItemType type)
    {
        playerInput.DisableAllActionMaps();
        itemActions.UseItem(type);
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator AttackRoutine()
    {
        enemyInfo.Health -= calculateAttackDamage();
        yield return StartCoroutine(AttackMovement(playerSprite, enemySprite));
        StartCoroutine(EnemyTurn());
        yield return null;
        CheckBattleFinished();
    }

    private void CheckBattleFinished()
    {
        PlayerData playerStats = db.Data.PlayerData;

        Debug.Log($"Player {playerStats.Health}. Enemy: {enemyInfo.Health}");

        if (enemyInfo.Health <= 0)
        {
            Debug.Log("You win!");
        }
        else if (playerStats.Health <= 0)
        {
            Debug.Log("You lose");
        }

    }

    private float calculateAttackDamage()
    {
        PlayerData playerStats = db.Data.PlayerData;
        return playerStats.Level * 50 * (1 - (enemyInfo.Defense / 100));
    }

    private float calculateDamageTaken()
    {
        PlayerData playerStats = db.Data.PlayerData;
        return enemyInfo.Attack * (1 - (playerStats.Defense / 100));
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        // Move enemy sprite towards player
        yield return StartCoroutine(AttackMovement(enemySprite, playerSprite));

        PlayerData playerStats = db.Data.PlayerData;
        playerStats.Health -= calculateDamageTaken();
        db.UpdatePlayerData(playerStats);

        turn++;
        playerInput.ChangeActionMap(PlayerInputActionMap.Battle);


        CheckBattleFinished();
    }

    private IEnumerator AttackMovement(SpriteRenderer attacker, SpriteRenderer target)
    {
        int initialSortingOrder = attacker.sortingOrder;
        Vector3 initialPosition = attacker.transform.position;
        Vector3 targetPosition = target.transform.position;

        attacker.sortingOrder = 999;

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            attacker.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            attacker.transform.position = Vector3.Lerp(targetPosition, initialPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        attacker.sortingOrder = initialSortingOrder;
    }
}
