using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour {
    [SerializeField] private PlayerInputManagerSO input;
    [SerializeField] private GameManagerSO gameManager;

    private UIDocument uiDocument;
    private VisualElement rootUiElement;
    private VisualElement activeMenu;
    private List<VisualElement> menuOptions;

    private enum MenuDirection {
        Up,
        Down
    }

    private void OnEnable() {
        input.OnSelect += HandleSelectPress;
        input.OnUp += HandleUp;
        input.OnDown += HandleDown;

        uiDocument = GetComponent<UIDocument>();
        rootUiElement = uiDocument.rootVisualElement;
        activeMenu = rootUiElement.Q(className: "menu--active");
        menuOptions = activeMenu.Query(className: "menu-option").ToList();
    }

    private void OnDisable() {
        input.OnSelect -= HandleSelectPress;
        input.OnUp -= HandleUp;
        input.OnDown -= HandleDown;

        // Better safe then sorry
        uiDocument = null;
        rootUiElement = null;
        activeMenu = null;
        menuOptions = null;
    }

    private void MoveMenuPosition(MenuDirection direction) {
        // Make sure there are multiple items
        if (menuOptions.Count <= 1) return;

        // Get active item
        VisualElement activeOption = activeMenu.Q(className: "menu-option--active");
        if (activeOption == null) return;

        // Get active index
        int index = activeOption.parent.IndexOf(activeOption);

        // New index
        int newIndex = direction == MenuDirection.Up ? index - 1 : index + 1;

        // If the new index is out of range, loop around.
        if (newIndex < 0) {
            newIndex = menuOptions.Count - 1;
        } else if (newIndex >= menuOptions.Count) {
            newIndex = 0;
        }

        // Remove old active class
        activeOption.RemoveFromClassList("menu-option--active");

        // Set new one
        menuOptions[newIndex].AddToClassList("menu-option--active");
    }

    private void HandleSelect(string action) {
        Debug.Log(action);

        switch (action) {
            case "pause-inventory":
                activeMenu.RemoveFromClassList("menu--active");
                activeMenu = rootUiElement.Q(className: "menu--inventory");
                menuOptions = activeMenu.Query(className: "menu-option").ToList();
                activeMenu.AddToClassList("menu--active");
                break;
            case "pause-equipment":
                break;
            case "pause-quit":
                Application.Quit();
                break;
            default:
                gameManager.ChangeGameState(GameState.World);
                break;
        }
    }

    private void HandleDown() {
        MoveMenuPosition(MenuDirection.Down);
    }

    private void HandleUp() {
        MoveMenuPosition(MenuDirection.Up);
    }

    private void HandleSelectPress() {
        VisualElement selectedOptionEl = activeMenu.Q(className: "menu-option--active");
        if (selectedOptionEl == null) return;

        string menuAction = selectedOptionEl.name;

        HandleSelect(menuAction);
    }
}
