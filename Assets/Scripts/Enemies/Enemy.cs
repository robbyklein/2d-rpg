using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Cerberus,
    Boneworm,
    Ooze,
    Rat,
    Scorpion,
    Skull,
    Slime
}

public class Enemy {
    public EnemyType type { get; private set; }
    public string EnemyName { get; private set; }
    public float Attack { get; private set; }
    public float Defense { get; private set; }
    public float Health { get; private set; }

    public Enemy(float playerLevel) {
        Debug.Log("made it here!");

        type = GetRandomEnemyType();

        Debug.Log($"You're fighting a {type}");
    }

    public EnemyType GetRandomEnemyType() {
        Array values = Enum.GetValues(typeof(EnemyType));
        return (EnemyType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}
