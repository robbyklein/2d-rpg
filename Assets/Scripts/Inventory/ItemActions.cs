using UnityEngine;

[CreateAssetMenu(fileName = "ItemActions", menuName = "ScriptableObjects/Actions/ItemActions")]
public class ItemActions : ScriptableObject {
    [SerializeField] private DatabaseSO db;
    [SerializeField] private InventorySO inventory;

    private void UsePotion() {
        PlayerData playerData = db.Data.PlayerData;

        float targetHealth = playerData.Health + 50;

        playerData.Health = Mathf.Min(targetHealth, playerData.MaxHealth);

        inventory.RemoveItem(ItemType.Potion, 1);
        db.UpdatePlayerData(playerData);
    }

    private void UseAntidote() {
    }

    public void UseItem(ItemType itemType) {
        switch (itemType) {
            case ItemType.Potion:
                UsePotion();
                break;
            case ItemType.Antidote:
                UseAntidote();
                break;
        }
    }
}
