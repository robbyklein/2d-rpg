using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryPopulator : BaseMenuPopulator {
    // Fields
    [SerializeField] private DatabaseSO db;
    [SerializeField] private ItemActions itemActions;

    // State
    private List<ItemEntry> items;
    private List<MenuOption> originalMenuItems;

    protected override void OnEnable() {
        // Populate menu
        uiDocument = GetComponent<UIDocument>();
        uiRoot = uiDocument.rootVisualElement;

        // We'll need these on rebuilds
        originalMenuItems = new List<MenuOption>(menuOptions);

        // Subscribe to events
        playerInput.OnPauseDown += HandleOnPauseDown;
        playerInput.OnPauseUp += HandleOnPauseUp;
        playerInput.OnPauseSelect += HandleSelect;
        db.OnPlayerInventoryUpdate += HandleInventoryUpdate;

        // Enable menu action map
        playerInput.ChangeActionMap(PlayerInputActionMap.Pause);

        PopulateMenu();
    }

    protected override void OnDisable() {
        playerInput.OnPauseDown -= HandleOnPauseDown;
        playerInput.OnPauseUp -= HandleOnPauseUp;
        playerInput.OnPauseSelect -= HandleSelect;
    }

    private void PopulateMenu() {
        // Clear old options
        menuOptions.Clear();

        // Re-add originals
        menuOptions.InsertRange(0, originalMenuItems);

        // Get the items from the database
        items = db.Data.PlayerInventory;

        // Add menu options for each one
        for (int i = 0; i < items.Count; i++) {
            ItemEntry item = items[i];
            string itemName = keyLanguage.RetrieveValue(item.ItemName);
            string itemTitle = $"{item.Quantity}x {itemName}";
            menuOptions.Insert(0, new MenuOption { Key = item.ItemName, Text = itemTitle, ItemType = item.ItemType });
        }

        BuildMenu();
    }

    private void HandleInventoryUpdate() {
        PopulateMenu();
    }

    protected override void HandleSelect() {
        audioManager.PlaySFX(SoundClip.MenuSelect);

        MenuOption selectedOption = menuOptions[selectedIndex];

        if (selectedOption.Action != null) {
            menuOptions[selectedIndex].Action?.Invoke();
        }
        else if (selectedOption.ItemType != null) {
            itemActions?.UseItem(selectedOption.ItemType.Value);
        }
    }
}
