using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {
    [SerializeField] private DatabaseSO db;
    [SerializeField] private EnemyDataSO enemyData;

    private string enemyName;
    private float attack;
    private float defense;
    private float health;

    private void Start() {
        attack = enemyData.Attack;
        defense = enemyData.Defense;
        health = enemyData.Health;
        name = enemyData.Name;
    }

    protected void PerformAttack() {
        PlayerData playerData = db.Data.PlayerData;
        playerData.Health -= attack * (1 - (playerData.Defense / 100));
        db.UpdatePlayerData(playerData);
    }

}
