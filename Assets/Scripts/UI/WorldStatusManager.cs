using UnityEngine;
using UnityEngine.UIElements;

public class WorldStatusManager : MonoBehaviour {
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
        barFilledElement = rootUiElement.Q(name: "world-status-filled");
        healthLabel = (Label)rootUiElement.Q(name: "world-status-health-text");
    }

    private void UpdateUI() {
        PlayerData playerData = db.Data.PlayerData;

        Debug.Log(playerData.Health);

        healthLabel.text = $"{playerData.Health}/{playerData.MaxHealth}";
        barFilledElement.style.width = new Length(playerData.Health / playerData.MaxHealth * 100, LengthUnit.Percent);
    }

    private void HandlePlayerDataUpdate() {
        UpdateUI();
    }
}
