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
        // UI doc / elements
        uiDocument = GetComponent<UIDocument>();

        rootUiElement = uiDocument.rootVisualElement;
        barFilledElement = rootUiElement.Q(name: "world-status-health-filled");
        healthLabel = (Label)rootUiElement.Q(name: "world-status-health-text");

        // Set initial health
        healthLabel.text = $"{playerStats.health}/{playerStats.maxHealth}";
        barFilledElement.style.width = new Length(playerStats.health / playerStats.maxHealth * 100, LengthUnit.Percent);
    }

    // Update is called once per frame
    void Update() {

    }
}
