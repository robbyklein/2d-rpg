using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPopulator : BaseMenuPopulator {
    protected override void OnEnable() {
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
        menuOptions[selectedIndex].Action?.Invoke();
    }
}
