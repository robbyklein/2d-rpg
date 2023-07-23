using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NpcDialog : MonoBehaviour {
    [SerializeField] string npcKey;
    [SerializeField] LanguageSO language;
    private List<string> dialogs;
    [SerializeField] UIDocument dialogUi;
    [SerializeField] PlayerInputManagerSO playerInputManager;

    string playerLanguage = "es";

    VisualElement rootUiElement;
    Label textUiElement;

    bool npcActive = false;
    int activeDialogIndex = 0;

    private void OnEnable() {
        playerInputManager.OnDialogSelect += HandleDialogSelect;

        dialogs = language.LanguageDict[playerLanguage][npcKey];
    }

    private void OnDisable() {
        playerInputManager.OnDialogSelect -= HandleDialogSelect;
    }

    public void StartConversation() {
        if (dialogs.Count > 0) {
            npcActive = true;

            // Show the chat box
            dialogUi.enabled = true;

            // Get the elements
            rootUiElement = dialogUi.rootVisualElement;
            textUiElement = rootUiElement.Query<Label>(className: "dialog-text").First();

            // Disable input while talking
            playerInputManager.DisableAllActionMaps();

            // Start the speak coroutine
            StartCoroutine(SpeakDialog());
        }
    }

    private void ResetConversation() {
        textUiElement.text = "";
        activeDialogIndex = 0;
        dialogUi.enabled = false;
        npcActive = false;
    }

    private void EndConversation() {
        ResetConversation();
        playerInputManager.ChangeActionMap(PlayerInputActionMap.World);
    }

    private void HandleDialogSelect() {
        if (!npcActive) return;

        activeDialogIndex++;

        // Disable input while talking
        playerInputManager.DisableAllActionMaps();

        if (activeDialogIndex == dialogs.Count) {
            EndConversation();
        } else {
            StartCoroutine(SpeakDialog());
        }
    }


    IEnumerator SpeakDialog() {
        // Reset text
        textUiElement.text = "";

        // Type of the active dialog
        int letterIndex = 0;
        string dialog = dialogs[activeDialogIndex];

        while (dialog.Length > letterIndex) {
            textUiElement.text += dialog[letterIndex];
            letterIndex++;
            yield return new WaitForSeconds(0.05f);
        }

        // Enable the dialog input
        playerInputManager.ChangeActionMap(PlayerInputActionMap.Dialog);
    }
}