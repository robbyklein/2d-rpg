using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPopulator : BaseMenuPopulator {
    protected override void OnEnable() {
        // Populate menu
        uiDocument = GetComponent<UIDocument>();
        uiRoot = uiDocument.rootVisualElement;

        // Subscribe to events
        playerInput.OnPauseDown += HandleOnPauseDown;
        playerInput.OnPauseUp += HandleOnPauseUp;
        playerInput.OnPauseSelect += HandleSelect;

        // Enable menu action map
        playerInput.ChangeActionMap(PlayerInputActionMap.Pause);

        BuildMenu();
    }

    protected override void OnDisable() {
        playerInput.OnPauseDown -= HandleOnPauseDown;
        playerInput.OnPauseUp -= HandleOnPauseUp;
        playerInput.OnPauseSelect -= HandleSelect;
    }

    protected override void HandleSelect() {
        audioManager.PlaySFX(SoundClip.MenuSelect);
        menuOptions[selectedIndex].Action?.Invoke();
    }
}
