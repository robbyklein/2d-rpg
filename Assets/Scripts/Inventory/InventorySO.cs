using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Potion,
    Antidote
}

[System.Serializable]
public class Item {
    public ItemType ItemType;
}

[System.Serializable]
public class ItemEntry {
    public Item Item;
    public string ItemName;
    public int Quantity;

    public ItemEntry(Item item, int quantity) {
        Item = item;
        Quantity = quantity;

        switch (item.ItemType) {
            case ItemType.Potion:
                ItemName = "Potion";
                break;
            case ItemType.Antidote:
                ItemName = "Antidote";
                break;
        }
    }
}

[CreateAssetMenu(fileName = "Inventory Manager", menuName = "ScriptableObjects/Managers/InventoryManager")]
public class InventorySO : ScriptableObject {
    [SerializeField] private DatabaseSO db;

    public List<ItemEntry> itemList = new List<ItemEntry>();

    private void OnEnable() {
        itemList ??= db.Data.PlayerInventory;

        if (db.Data.PlayerInventory.Count == 0) {
            AddItem(new ItemEntry(new Item { ItemType = ItemType.Potion }, 1));
        }
    }

    public void AddItem(ItemEntry itemEntry) {
        ItemEntry entry = itemList.Find(x => x.Item.ItemType == itemEntry.Item.ItemType);

        if (entry == null) {
            itemList.Add(itemEntry);
        }
        else {
            entry.Quantity += itemEntry.Quantity;
        }

        Save();
    }

    public void RemoveItem(ItemType itemType, int quantity) {
        ItemEntry entry = itemList.Find(x => x.Item.ItemType == itemType);

        if (entry != null && entry.Quantity >= quantity) {
            entry.Quantity -= quantity;
            if (entry.Quantity <= 0) {
                itemList.Remove(entry);
            }

            Save();
        }
    }

    private void Save() {
        db.UpdatePlayerInventory(itemList);
    }

}
