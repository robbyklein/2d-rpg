using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float Level = 1f;
    public float Health = 100f;
    public float MaxHealth = 100f;
    public float Defense = 10f;
}

[System.Serializable]
public class Data
{
    public PlayerData PlayerData = new PlayerData();
}


[CreateAssetMenu(fileName = "Database", menuName = "ScriptableObjects/Data/Database")]
public class DatabaseSO : ScriptableObject
{
    private string filepath;
    public Data Data { get; private set; }

    public Action OnPlayerDataUpdate;

    private void OnEnable()
    {
        filepath ??= Path.Combine(Application.persistentDataPath, "savefile.json");

        if (!File.Exists(filepath))
        {
            CreateSavefile();
            SaveData();
        }
        else
        {
            LoadData();
        }
    }

    #region Updaters
    public void UpdateCharacterData(PlayerData newCharacterData)
    {
        Data.PlayerData = newCharacterData;
        OnPlayerDataUpdate?.Invoke();
        SaveData();
    }
    #endregion

    #region Create/Load/Save
    private void CreateSavefile()
    {
        Data data = new Data();
        SaveData();
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(Data);
        File.WriteAllText(filepath, json);
    }

    private void LoadData()
    {
        string json = File.ReadAllText(filepath);
        Data = JsonUtility.FromJson<Data>(json);
    }
    #endregion
}
