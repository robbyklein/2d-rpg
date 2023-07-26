using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct Language {
    public string Key;
    public List<NpcData> NpcData;
}

[System.Serializable]
struct NpcData {
    public string Key;
    public List<string> Dialog;
}

[CreateAssetMenu(fileName = "Langauage", menuName = "ScriptableObjects/Data/Language")]
public class LanguageSO : ScriptableObject {
    [SerializeField] private List<Language> language;

    public Dictionary<string, Dictionary<string, List<string>>> LanguageDict { get; private set; }

    void OnEnable() {
        // For testing
        PlayerPrefs.SetString("playerLanguage", "en");

        LanguageDict = new();

        foreach (var lang in language) {
            Dictionary<string, List<string>> npcsDict = new Dictionary<string, List<string>>();
            foreach (var npc in lang.NpcData) {
                npcsDict[npc.Key] = npc.Dialog;
            }
            LanguageDict[lang.Key] = npcsDict;
        }
    }

    public List<string> retrieveDialog(string npcKey) {
        string playerLanguage = PlayerPrefs.GetString("playerLanguage", "en");
        return LanguageDict[playerLanguage][npcKey];
    }
}
