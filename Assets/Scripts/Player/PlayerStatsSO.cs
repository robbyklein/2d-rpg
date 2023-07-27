using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats Manager", menuName = "ScriptableObjects/Managers/PlayerStatsManager")]
public class PlayerStatsSO : ScriptableObject {
    public int maxHealth { get; private set; } = 100;
    public int health { get; private set; } = 100;
    private float defense = 0.1f;

    public event Action<int> OnPlayerHealthChange;

    private void OnEnable() {
        OnPlayerHealthChange?.Invoke(health);
    }

    public void TakeDamage(int damage) {
        float targetAmount = damage * (1 - defense);
        health = Mathf.RoundToInt(targetAmount);

        OnPlayerHealthChange?.Invoke(health);
    }
    public void Heal(int healAmount) {
        float targetAmount = health + healAmount;

        if (targetAmount > maxHealth) {
            health = maxHealth;
        } else {
            health = Mathf.RoundToInt(health);
        }

        OnPlayerHealthChange?.Invoke(health);
    }
}
