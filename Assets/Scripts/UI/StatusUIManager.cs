using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StatusUIManager : MonoBehaviour {
    [SerializeField] private DatabaseSO db;

    private UIDocument uiDocument;
    private VisualElement rootUiElement;
    private VisualElement barFilledElement;
    private Label healthLabel;

    private void OnEnable() {
        db.OnPlayerDataUpdate += HandlePlayerDataUpdate;
        FetchUIElements();
        UpdateUI();
    }

    private void OnDisable() {
        db.OnPlayerDataUpdate -= HandlePlayerDataUpdate;
    }

    private void FetchUIElements() {
        uiDocument = GetComponent<UIDocument>();
        rootUiElement = uiDocument.rootVisualElement;
        barFilledElement = rootUiElement.Q(name: "status-filled");
        healthLabel = (Label)rootUiElement.Q(name: "status-health-text");
    }

    private void UpdateUI() {
        PlayerData playerData = db.Data.PlayerData;

        int healthRounded = (int)Math.Ceiling(playerData.Health);

        healthLabel.text = $"{healthRounded}/{playerData.MaxHealth}";
        barFilledElement.style.width = new Length(playerData.Health / playerData.MaxHealth * 100, LengthUnit.Percent);
    }

    private void HandlePlayerDataUpdate() {
        UpdateUI();
    }
}
