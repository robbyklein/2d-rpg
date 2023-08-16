using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Cerberus,
    Boneworm,
    // Ooze,
    // Rat,
    // Scorpion,
    // Skull,
    // Slime
}

[System.Serializable]
public struct EnemyInfo {
    public EnemyType Type;
    public string Name;
    public float Health;
    public float Defense;
    public float Attack;
    public List<Sprite> Sprites;
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Data/EnemyData")]
public class EnemyDataSO : ScriptableObject {
    [SerializeField] private List<EnemyInfo> enemyInfos;
    private Dictionary<EnemyType, EnemyInfo> enemyInfoDict;

    private void OnEnable() {
        if (enemyInfoDict == null) {
            enemyInfoDict = new();

            foreach (var enemyInfo in enemyInfos) {
                enemyInfoDict[enemyInfo.Type] = enemyInfo;
            }
        }
    }

    private EnemyType GetRandomEnemyType() {
        Array values = Enum.GetValues(typeof(EnemyType));
        return (EnemyType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }


    public EnemyInfo GetEnemyData(float playerLevel) {
        EnemyType type = GetRandomEnemyType();

        if (enemyInfoDict.TryGetValue(type, out EnemyInfo enemyInfo)) {
            float randomFloat = 1.0f + UnityEngine.Random.value;
            float multiplayer = randomFloat + (playerLevel / 10f);

            enemyInfo.Attack *= multiplayer;
            enemyInfo.Defense *= multiplayer;
            enemyInfo.Health *= multiplayer;

            return enemyInfo;
        }
        else {
            // Create a default enemy if not found
            enemyInfo = new EnemyInfo();
            enemyInfoDict[type] = enemyInfo;
            return enemyInfo;
        }
    }
}
