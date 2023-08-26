using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleMessagingManager : MonoBehaviour {
    [SerializeField] private BattleManager manager;
    [SerializeField] private UIDocument uiDocument;

    private VisualElement rootUIElement;
    private VisualElement boxEl;
    private Label messageLabel;

    private void OnEnable() {
        rootUIElement = uiDocument.rootVisualElement;
        boxEl = rootUIElement.Q(name: "battle-message");
        messageLabel = rootUIElement.Q<Label>(name: "battle-message-text");

        manager.OnDisplayMessage += DisplayMessage;
    }

    private void OnDisable() {
        manager.OnDisplayMessage -= DisplayMessage;

    }

    private void DisplayMessage(string message) {
        StartCoroutine(DisplayMessageRoutine(message));
    }

    private IEnumerator DisplayMessageRoutine(string message) {
        messageLabel.text = message;
        boxEl.RemoveFromClassList("opacity-0");
        yield return new WaitForSeconds(1);
        boxEl.AddToClassList("opacity-0");
    }
}
