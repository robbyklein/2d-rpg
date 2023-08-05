using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryPopulator : BaseMenuPopulator {
    // Fields
    [SerializeField] private InventorySO inventory;
    [SerializeField] private DatabaseSO db;

    // State
    private List<ItemEntry> items;

    protected override void OnEnable() {
        // Get the items from the database
        items = db.Data.PlayerInventory;

        // Add menu options for each one
        for (int i = 0; i < items.Count; i++) {
            string itemTitle = $"{items[i].Quantity}x {items[i].ItemName}";
            menuOptions.Insert(0, new MenuOption { Title = itemTitle });
        }

        // Subscribe to events
        playerInput.OnPauseDown += HandleOnPauseDown;
        playerInput.OnPauseUp += HandleOnPauseUp;
        playerInput.OnPauseSelect += HandleSelect;

        // Enable menu action map
        playerInput.ChangeActionMap(PlayerInputActionMap.Pause);

        // Populate menu
        uiDocument = GetComponent<UIDocument>();

        BuildMenu();
    }


    protected override void HandleSelect() {
        if (menuOptions[selectedIndex].Action != null) {
            menuOptions[selectedIndex].Action?.Invoke();
        }
        else {
            Debug.Log("Use the item!");
        }
    }
}
