using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats Manager", menuName = "ScriptableObjects/Managers/PlayerStatsManager")]
public class PlayerStatsSO : ScriptableObject {
    // Database
    [SerializeField] private DatabaseSO db;

    // Stats
    [SerializeField] private float level = 1;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 40f;

    private void OnEnable() {
        level = db.Data.PlayerData.Level;
        health = db.Data.PlayerData.Health;
        maxHealth = db.Data.PlayerData.MaxHealth;
    }
}
