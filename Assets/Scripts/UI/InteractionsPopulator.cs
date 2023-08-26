using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionsPopulator : MonoBehaviour {
    [SerializeField] private KeyLanguageSO keyLanguage;
    [SerializeField] private UIDocument uiDocument;
    private VisualElement uiRoot;



    private void OnEnable() {
        uiRoot = uiDocument.rootVisualElement;

        string lang = keyLanguage.RetrieveValue("interact");

        uiRoot.Q<Label>(name: "interact-label").text = lang;
    }

}
