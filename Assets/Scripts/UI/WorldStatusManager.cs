using UnityEngine;
using UnityEngine.UIElements;

public class WorldStatusManager : MonoBehaviour {
    [SerializeField] private PlayerStatsSO playerStats;

    // UI elements
    private UIDocument uiDocument;
    private VisualElement rootUiElement;
    private VisualElement barFilledElement;
    private Label healthLabel;

    // Start is called before the first frame update
    private void OnEnable() {
        // Subscribe to changes
        playerStats.OnPlayerStatsChange += HandlePlayerStatsChange;

        // UI doc / elements
        uiDocument = GetComponent<UIDocument>();
        rootUiElement = uiDocument.rootVisualElement;
        barFilledElement = rootUiElement.Q(name: "world-status-filled");
        healthLabel = (Label)rootUiElement.Q(name: "world-status-health-text");

        // Set initial health
        UpdateUi(playerStats.PlayerStats());
    }

    private void UpdateUi(PlayerStats stats) {
        healthLabel.text = $"{stats.Health}/{stats.MaxHealth}";
        barFilledElement.style.width = new Length(stats.Health / stats.MaxHealth * 100, LengthUnit.Percent);
    }

    private void OnDisable() {
        playerStats.OnPlayerStatsChange -= HandlePlayerStatsChange;
    }

    private void HandlePlayerStatsChange(PlayerStats stats) {
        UpdateUi(stats);
    }
}
