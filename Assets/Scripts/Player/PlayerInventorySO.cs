using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Inventory Manager", menuName = "ScriptableObjects/Managers/PlayerInventoryManager")]
public class PlayerInventorySO : ScriptableObject {
    [SerializeField] private PlayerStatsSO stats;

    public Dictionary<Item, int> items { get; private set; } = new Dictionary<Item, int>();

    private void OnEnable() {
        AddItem(new Potion(), 3);
    }

    public void AddItem(Item item, int quantity = 1) {
        if (items.ContainsKey(item)) {
            items[item] += quantity;
        } else {
            items[item] = quantity;
        }
    }

    public bool RemoveItem(Item item, int quantity = 1) {
        if (items.ContainsKey(item)) {
            items[item] -= quantity;

            if (items[item] <= 0) {
                items.Remove(item);
            }
            return true;
        } else {
            return false;
        }
    }

    public void UseItem() {

    }

}
