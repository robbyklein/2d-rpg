using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleMenuManager : MonoBehaviour {
    private enum Menu {
        Main,
        Items
    }

    [SerializeField] PlayerInputManagerSO playerInput;
    [SerializeField] private DatabaseSO db;
    [SerializeField] private List<MenuOption> mainMenuOptions;
    [SerializeField] private List<MenuOption> itemMenuOptions;

    private UIDocument uiDocument;
    private VisualElement uiRoot;
    private VisualElement menuOptionsEl;
    private Menu selectedMenu = Menu.Main;
    private int selectedIndex = 0;


    #region Lifecycle
    private void OnEnable() {
        playerInput.OnBattleBack += HandleBack;
        playerInput.OnBattleUp += HandleUp;
        playerInput.OnBattleDown += HandleDown;
        playerInput.OnBattleSelect += HandleSelect;
        db.OnPlayerInventoryUpdate += HandleInventoryUpdate;
    }

    private void OnDisable() {
        playerInput.OnBattleBack -= HandleBack;
        playerInput.OnBattleUp -= HandleUp;
        playerInput.OnBattleDown -= HandleDown;
        playerInput.OnBattleSelect -= HandleSelect;
        db.OnPlayerInventoryUpdate -= HandleInventoryUpdate;

    }

    private void Start() {
        // Get ui doc/els
        uiDocument = GetComponent<UIDocument>();
        uiRoot = uiDocument.rootVisualElement;
        menuOptionsEl = uiRoot.Query(name: "menu-options").First();

        // Set inventory menu options 
        UpdateItemMenuOptions();

        // Build the menu
        BuildMenu();
    }
    #endregion

    private void BuildMenu() {
        // Clear old options
        menuOptionsEl.Clear();

        // Add new options
        switch (selectedMenu) {
            case Menu.Main:
                BuildMainMenu();
                break;
            case Menu.Items:
                BuildItemsMenu();
                break;
        }
    }

    private void BuildMainMenu() {
        for (int i = 0; i < mainMenuOptions.Count; i++) {
            MenuOption mainMenuOption = mainMenuOptions[i];
            menuOptionsEl.Add(UIHelpers.BuildMenuOption(mainMenuOption.Title, i, selectedIndex));
        }
    }

    private void BuildItemsMenu() {
        for (int i = 0; i < itemMenuOptions.Count; i++) {
            MenuOption itemMenuOption = itemMenuOptions[i];
            menuOptionsEl.Add(UIHelpers.BuildMenuOption(itemMenuOption.Title, i, selectedIndex));
        }
    }

    private void UpdateItemMenuOptions() {
        // Get items from db
        List<ItemEntry> items = db.Data.PlayerInventory;

        // Clear the old menu options
        itemMenuOptions.Clear();

        // Add a new one for each item
        for (int i = 0; i < items.Count; i++) {
            ItemEntry item = items[i];
            string itemTitle = $"{item.Quantity}x {item.ItemName}";
            itemMenuOptions.Insert(0, new MenuOption { Title = itemTitle, ItemType = item.ItemType });
        }

        // Add an extra to go back to main menu
        itemMenuOptions.Add(new MenuOption {
            Title = "Main menu",
            NormalAction = SwitchToMainMenu
        });
    }

    private void SwitchToMainMenu() {
        selectedMenu = Menu.Main;
        selectedIndex = 0;
        BuildMenu();
    }

    public void SwitchToItemsMenu() {
        selectedMenu = Menu.Items;
        selectedIndex = 0;
        BuildMenu();
    }

    #region Event handlers
    private void HandleInventoryUpdate() {
        UpdateItemMenuOptions();
        if (selectedMenu == Menu.Items) BuildItemsMenu();
    }

    private void HandleSelect() {
        if (selectedMenu == Menu.Main) {
            mainMenuOptions[selectedIndex].Action?.Invoke();
        }
        else if (selectedMenu == Menu.Items) {
            itemMenuOptions[selectedIndex].NormalAction.Invoke();
        }
    }

    private void HandleDown() {
        selectedIndex = (selectedIndex + mainMenuOptions.Count - 1) % mainMenuOptions.Count;
        BuildMenu();
    }

    private void HandleUp() {
        selectedIndex = (selectedIndex + 1) % mainMenuOptions.Count;
        BuildMenu();
    }

    private void HandleBack() {
        SwitchToMainMenu();
    }
    #endregion
}
