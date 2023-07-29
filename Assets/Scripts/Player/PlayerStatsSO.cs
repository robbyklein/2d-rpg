using System;
using UnityEngine;

public struct PlayerStats {
    public float MaxHealth;
    public float Health;
    public float Defense;
}

[CreateAssetMenu(fileName = "Player Stats Manager", menuName = "ScriptableObjects/Managers/PlayerStatsManager")]
public class PlayerStatsSO : ScriptableObject {
    // Stats
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 40f;
    [SerializeField] private float defense = 0.1f;

    // Events
    public event Action<PlayerStats> OnPlayerStatsChange;

    private PlayerStats CreatePlayerStats() {
        return new PlayerStats {
            MaxHealth = this.maxHealth,
            Health = this.health,
            Defense = this.defense * 100
        };
    }

    public PlayerStats PlayerStats() {
        return CreatePlayerStats();
    }

    public void TakeDamage(int damage) {
        float targetAmount = damage * (1 - defense);
        health = Mathf.Round(targetAmount);

        OnPlayerStatsChange?.Invoke(CreatePlayerStats());
    }
    public void Heal(int healAmount) {
        float targetAmount = health + healAmount;

        if (targetAmount > maxHealth) {
            health = maxHealth;
        } else {
            health = Mathf.RoundToInt(health);
        }

        OnPlayerStatsChange?.Invoke(CreatePlayerStats());
    }
}
