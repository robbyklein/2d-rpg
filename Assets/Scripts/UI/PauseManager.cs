using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour {
    // Enums
    private enum MenuDirection {
        Up,
        Down
    }

    private enum SubMenu {
        Main,
        Inventory,
        Equipment,
        Settings,
    }

    private enum MenuAction {
        OpenInventory,
        OpenEquipment,
        OpenSettings,
        OpenMain,
        ResumeGame,
        QuitGame,
    }

    // Components
    [SerializeField] private PlayerInputManagerSO input;
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private PlayerInventorySO inventory;
    [SerializeField] private PlayerStatsSO stats;

    // Classes
    string activeMenuClass = "menu--active";
    string menuOptionClass = "menu-option";
    string activeMenuOptionClass = "menu-option--active";

    // UI Elements
    private UIDocument uiDocument;
    private VisualElement rootUiElement;
    private VisualElement activeMenu;
    private Label healthText;

    private List<VisualElement> menuOptions;

    private void OnEnable() {
        input.OnPauseSelect += HandleSelectPress;
        input.OnPauseUp += HandleUp;
        input.OnPauseDown += HandleDown;
        stats.OnPlayerStatsChange += HandlePlayerStatsChange;

        FetchUiElements(false);
        UpdatePlayerStats(stats.PlayerStats());
        PopulateInventory();
    }

    private void OnDisable() {
        input.OnPauseSelect -= HandleSelectPress;
        input.OnPauseUp -= HandleUp;
        input.OnPauseDown -= HandleDown;
        stats.OnPlayerStatsChange -= HandlePlayerStatsChange;
    }

    private void UpdatePlayerStats(PlayerStats stats) {
        Label healthLabel = rootUiElement.Q<Label>(name: "pause-player-health-text");
        Label defenseLabel = rootUiElement.Q<Label>(name: "pause-player-defense-text");

        healthText.text = $"{stats.Health}/{stats.MaxHealth}";
        defenseLabel.text = $"{stats.Defense}";
    }

    private void HandlePlayerStatsChange(PlayerStats stats) {
        UpdatePlayerStats(stats);
    }

    private void PopulateInventory() {
        // Items container
        VisualElement itemsEl = rootUiElement.Q(name: "pause-inventory-items");
        if (itemsEl == null) return;

        // Create an item row for each one
        int i = 0;
        foreach (KeyValuePair<Item, int> entry in inventory.items) {
            VisualElement el = CreateMenuOptionElement(entry.Key, entry.Value);
            if (i == 0) el.AddToClassList(activeMenuOptionClass);
            itemsEl.Add(el);
            i++;
        }
    }

    private VisualElement CreateMenuOptionElement(Item item, int quantity) {
        // Create a new VisualElement for the inventory item
        VisualElement itemElement = new VisualElement();
        itemElement.AddToClassList("flex-row");
        itemElement.AddToClassList("items-center");
        itemElement.AddToClassList("menu-option");

        // Create the menu indicator element
        VisualElement menuIndicator = new VisualElement();
        menuIndicator.AddToClassList("menu-indicator");
        itemElement.Add(menuIndicator);

        // Create the label
        Label itemLabel = new Label();
        itemLabel.text = $"{quantity}x {item.Name}";
        itemLabel.AddToClassList("ml-5");
        itemElement.Add(itemLabel);

        return itemElement;
    }

    private void FetchUiElements(bool skipRoot) {
        if (!skipRoot) {
            uiDocument = GetComponent<UIDocument>();
            rootUiElement = uiDocument.rootVisualElement;
        }

        activeMenu = rootUiElement.Q(className: activeMenuClass);
        menuOptions = activeMenu.Query(className: menuOptionClass).ToList();
        healthText = (Label)rootUiElement.Q(name: "pause-player-health-text");
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

    private MenuAction NameToMenuAction(string name) {
        switch (name) {
            case "pause-inventory":
                return MenuAction.OpenInventory;
            case "pause-equipment":
                return MenuAction.OpenEquipment;
            case "pause-main":
                return MenuAction.OpenMain;
            case "pause-settings":
                return MenuAction.OpenSettings;
            case "pause-quit":
                return MenuAction.QuitGame;
            default:
                return MenuAction.ResumeGame;
        }
    }

    private void OpenSubMenu(SubMenu menu) {
        // Remove active from old menu
        activeMenu.RemoveFromClassList(activeMenuClass);

        // Update active menu
        switch (menu) {
            case SubMenu.Inventory:
                activeMenu = rootUiElement.Q(className: "menu--inventory");
                break;
            case SubMenu.Equipment:
                activeMenu = rootUiElement.Q(className: "menu--equipment");
                break;
            case SubMenu.Settings:
                activeMenu = rootUiElement.Q(className: "menu--settings");
                break;
            default:
                activeMenu = rootUiElement.Q(className: "menu--main");
                break;
        }

        // Add active class
        activeMenu.AddToClassList(activeMenuClass);

        // Update options
        menuOptions = activeMenu.Query(className: menuOptionClass).ToList();
    }

    private void HandleSelect(MenuAction action) {
        switch (action) {
            case MenuAction.OpenInventory:
                OpenSubMenu(SubMenu.Inventory);
                break;
            case MenuAction.OpenEquipment:
                OpenSubMenu(SubMenu.Equipment);
                break;
            case MenuAction.OpenSettings:
                OpenSubMenu(SubMenu.Settings);
                break;
            case MenuAction.QuitGame:
                Application.Quit();
                break;
            case MenuAction.ResumeGame:
                gameManager.ChangeGameState(GameState.World);
                break;
            default:
                OpenSubMenu(SubMenu.Main);
                break;
        }
    }

    private void HandleSelectPress() {
        VisualElement selectedOptionEl = activeMenu.Q(className: "menu-option--active");
        if (selectedOptionEl == null) return;

        string menuActionString = selectedOptionEl.name;
        if (menuActionString == "") menuActionString = "pause-main";

        MenuAction action = NameToMenuAction(menuActionString);
        HandleSelect(action);
    }

    private void HandleDown() {
        MoveMenuPosition(MenuDirection.Down);
    }

    private void HandleUp() {
        MoveMenuPosition(MenuDirection.Up);
    }
}
