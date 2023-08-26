using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum DialogState {
    Idle,
    Speaking,
    WaitingForInput
}

public class EntityDialog : MonoBehaviour, IInteractable {
    // Components
    [SerializeField] private LanguageSO language;
    [SerializeField] private PlayerInputManagerSO playerInputManager;
    [SerializeField] private UIDocument dialogUi;

    // Settings
    [SerializeField] private string npcKey;

    // Data
    private List<string> dialogs;

    // Events
    static public event Action<bool> OnDialogBusy;
    public event Action<string> OnConversationFinished;

    // State
    private DialogState dialogState = DialogState.Idle;
    int activeDialogIndex = 0;

    // UI
    VisualElement rootUiElement;
    Label textUiElement;

    public void SwapNpcKey(string key) {
        npcKey = key;
        activeDialogIndex = 0;
        dialogs = language.retrieveDialog(npcKey);
    }

    private void OnEnable() {
        playerInputManager.OnDialogSelect += HandleDialogSelect;
        dialogs = language.retrieveDialog(npcKey);
    }

    private void OnDisable() {
        playerInputManager.OnDialogSelect -= HandleDialogSelect;
    }

    void IInteractable.OnInteract() {
        StartConversation();
    }

    private void ChangeState(DialogState newState) {
        dialogState = newState;

        switch (dialogState) {
            case DialogState.Speaking:
                // Let everyone know we're busy
                OnDialogBusy?.Invoke(true);

                // Disable all input while speaking
                playerInputManager.DisableAllActionMaps();

                // Type the text in ui
                StartCoroutine(SpeakDialog());

                break;
            case DialogState.WaitingForInput:
                // Let everyone know we're not busy
                OnDialogBusy?.Invoke(false);

                // Enable input
                playerInputManager.ChangeActionMap(PlayerInputActionMap.Dialog);

                break;
            default:
                // Reset state and ui
                ResetConversation();

                // Enable input
                playerInputManager.ChangeActionMap(PlayerInputActionMap.World);

                // Let everyone know whos been spoke to
                OnConversationFinished?.Invoke(npcKey);

                break;
        }
    }

    public void StartConversation() {
        if (dialogs.Count == 0) return;

        // Show the text box
        EnableUI();

        ChangeState(DialogState.Speaking);
    }

    private void EndConversation() {
        ChangeState(DialogState.Idle);
    }

    private void ResetConversation() {
        textUiElement.text = "";
        activeDialogIndex = 0;
        dialogUi.enabled = false;
    }

    private void HandleDialogSelect() {
        // Ignore inactive npc's
        if (dialogState == DialogState.Idle) return;

        // We need to go to the next dialog
        activeDialogIndex++;

        // Disable all input till we finish
        playerInputManager.DisableAllActionMaps();

        // Is the conversation over?
        bool shouldEndConversation = activeDialogIndex == dialogs.Count;

        if (shouldEndConversation) {
            EndConversation();
        }
        else {
            ChangeState(DialogState.Speaking);
        }
    }

    IEnumerator SpeakDialog() {
        // Clear the old text
        textUiElement.text = "";

        // Get the dialog to be spoke
        string dialog = dialogs[activeDialogIndex];

        // Type it
        int letterIndex = 0;

        while (dialog.Length > letterIndex) {
            textUiElement.text += dialog[letterIndex];
            letterIndex++;
            yield return new WaitForSeconds(0.05f);
        }

        // Let everyone know we're done
        ChangeState(DialogState.WaitingForInput);
    }

    private void EnableUI() {
        dialogUi.enabled = true;

        rootUiElement = dialogUi.rootVisualElement;
        textUiElement = rootUiElement.Q<Label>(name: "dialog-text");
    }

}