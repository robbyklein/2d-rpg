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
    public ItemType ItemType;

    public ItemEntry(Item item, int quantity) {
        Item = item;
        Quantity = quantity;
        ItemType = item.ItemType;

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


    public void AddItem(ItemEntry itemEntry) {
        ItemEntry entry = db.Data.PlayerInventory.Find(x => x.Item.ItemType == itemEntry.Item.ItemType);

        if (entry == null) {
            db.Data.PlayerInventory.Add(itemEntry);
        }
        else {
            entry.Quantity += itemEntry.Quantity;
        }

        Save();
    }

    public void RemoveItem(ItemType itemType, int quantity) {
        ItemEntry entry = db.Data.PlayerInventory.Find(x => {
            Debug.Log(x.Item.ItemType);
            return x.Item.ItemType == itemType;
        });


        if (entry != null && entry.Quantity >= quantity) {
            entry.Quantity -= quantity;

            if (entry.Quantity <= 0) {
                db.Data.PlayerInventory.Remove(entry);
            }

            Save();
        }
    }

    private void Save() {
        db.UpdatePlayerInventory(db.Data.PlayerInventory);
    }

}
