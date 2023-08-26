using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
struct KeyLanguage {
    public string Key;
    public List<KeyLanguageItem> KeyLanguageItems;
}

[System.Serializable]
struct KeyLanguageItem {
    public string Key;
    public string Value;
}

[CreateAssetMenu(fileName = "Language", menuName = "ScriptableObjects/Data/KeyLanguage")]
public class KeyLanguageSO : ScriptableObject {
    [SerializeField] private List<KeyLanguage> keyLanguage;

    public Dictionary<string, Dictionary<string, string>> KeyLanguageDict { get; private set; }

    void OnEnable() {
        KeyLanguageDict = new Dictionary<string, Dictionary<string, string>>();

        foreach (var lang in keyLanguage) {
            Dictionary<string, string> keyValuesDict = new();
            foreach (var keyValue in lang.KeyLanguageItems) {
                keyValuesDict[keyValue.Key] = keyValue.Value;
            }
            KeyLanguageDict[lang.Key] = keyValuesDict;
        }
    }

    public string RetrieveValue(string key) {
        string playerLanguage = PlayerPrefs.GetString("playerLanguage", "en");

        if (KeyLanguageDict.ContainsKey(playerLanguage) && KeyLanguageDict[playerLanguage].ContainsKey(key)) {
            return KeyLanguageDict[playerLanguage][key];
        }

        return null;
    }

}
