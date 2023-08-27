using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public float Level = 1f;
    public float Health = 100f;
    public float MaxHealth = 100f;
    public float Defense = 10f;
}



[System.Serializable]
public class Data {
    public PlayerData PlayerData = new PlayerData();
    public List<ItemEntry> PlayerInventory = new List<ItemEntry>();
}

[CreateAssetMenu(fileName = "Database", menuName = "ScriptableObjects/Data/Database")]
public class DatabaseSO : ScriptableObject {
    private string filepath;
    public Data Data { get; private set; }

    public Action OnPlayerDataUpdate;
    public Action OnPlayerInventoryUpdate;

    #region Updaters
    public void UpdatePlayerData(PlayerData newCharacterData) {
        Data.PlayerData = newCharacterData;
        OnPlayerDataUpdate?.Invoke();
        SaveData();
    }

    public void UpdatePlayerInventory(List<ItemEntry> newPlayerInventory) {
        Data.PlayerInventory = newPlayerInventory;
        OnPlayerInventoryUpdate?.Invoke();
        SaveData();
    }
    #endregion

    #region Create/Load/Save/Delete
    private void CreateSavefile() {
        Data = new Data();

        // Create two potions and add them to the inventory
        Item potionItem = new Item { ItemType = ItemType.Potion };
        ItemEntry potionEntry1 = new ItemEntry(potionItem, 4);
        Data.PlayerInventory.Add(potionEntry1);

        SaveData();
    }

    public void ResetDatabase() {
        filepath = Path.Combine(Application.persistentDataPath, "savefile.json");

        DeleteData();

        if (!File.Exists(filepath)) {
            CreateSavefile();
            SaveData();
        }
        else {
            LoadData();
        }
    }

    private void SaveData() {
        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(filepath, json);
    }

    private void LoadData() {
        string json = File.ReadAllText(filepath);
        Data = JsonUtility.FromJson<Data>(json);
    }

    private void DeleteData() {
        if (File.Exists(filepath)) {
            File.Delete(filepath);
        }
    }
    #endregion
}
