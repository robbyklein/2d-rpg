using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public float Level = 1f;
    public float Health = 40f;
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

    private void OnEnable() {
        filepath ??= Path.Combine(Application.persistentDataPath, "savefile.json");

#if UNITY_EDITOR
        DeleteData();
#endif

        if (!File.Exists(filepath)) {
            CreateSavefile();
            SaveData();
        }
        else {
            LoadData();
        }

    }

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
        SaveData();
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
            Debug.Log("Deleted the save file");
        }
    }
    #endregion
}
