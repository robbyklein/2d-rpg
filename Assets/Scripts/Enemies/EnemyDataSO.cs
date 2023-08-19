using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyInfo {
    public string Name;
    public float Health;
    public float Defense;
    public float Attack;
    public List<Sprite> Sprites;
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Data/EnemyData")]
public class EnemyDataSO : ScriptableObject {
    [SerializeField] private List<EnemyInfo> enemyInfos;
    private Dictionary<string, EnemyInfo> enemyInfoDict;

    private void OnEnable() {
        if (enemyInfoDict == null) {
            enemyInfoDict = new Dictionary<string, EnemyInfo>();

            foreach (var enemyInfo in enemyInfos) {
                enemyInfoDict[enemyInfo.Name] = enemyInfo;
            }
        }
    }

    private string GetRandomEnemyName() {
        int index = UnityEngine.Random.Range(0, enemyInfos.Count);
        return enemyInfos[index].Name;
    }

    private float CreateMultiplier(float playerLevel) {
        float randomFloat = 1.0f + UnityEngine.Random.value;
        return randomFloat + (playerLevel / 10f);
    }

    public EnemyInfo GetEnemyData(float playerLevel) {
        string name = GetRandomEnemyName();

        if (enemyInfoDict.TryGetValue(name, out EnemyInfo enemyInfo)) {
            enemyInfo.Attack *= CreateMultiplier(playerLevel);
            enemyInfo.Defense *= CreateMultiplier(playerLevel);
            enemyInfo.Health *= CreateMultiplier(playerLevel);

            return enemyInfo;
        }
        else {
            enemyInfo = new EnemyInfo();
            enemyInfoDict[name] = enemyInfo;
            return enemyInfo;
        }
    }
}