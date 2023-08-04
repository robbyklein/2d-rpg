using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct MenuOption {
    public string Title;
    public UnityEngine.Events.UnityEvent Action;
}

public class MenuPopulator : MonoBehaviour {
    // Fields
    [SerializeField] private string title;
    [SerializeField] private List<MenuOption> menuOptions;
    [SerializeField] private PlayerInputManagerSO playerInput;

    // State
    int selectedIndex = 0;

    // UI
    private UIDocument uiDocument;

    private void OnEnable() {
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

    private void OnDisable() {
        playerInput.OnPauseDown -= HandleOnPauseDown;
        playerInput.OnPauseUp -= HandleOnPauseUp;
        playerInput.OnPauseSelect -= HandleSelect;
    }

    private void HandleOnPauseUp() {
        if (selectedIndex == 0) {
            selectedIndex = menuOptions.Count - 1;
        }
        else {
            selectedIndex -= 1;
        }

        BuildMenu();
    }

    private void HandleOnPauseDown() {
        if (selectedIndex == menuOptions.Count - 1) {
            selectedIndex = 0;
        }
        else {
            selectedIndex += 1;
        }

        BuildMenu();
    }

    private void HandleSelect() {
        menuOptions[selectedIndex].Action?.Invoke();
    }

    private void BuildMenu() {
        // Get root
        VisualElement root = uiDocument.rootVisualElement;

        // Set title
        root.Q<Label>(name: "menu-title").text = title;

        // Get options holder
        VisualElement menuOptionsEl = root.Q(name: "menu-options");

        // Clear old options
        menuOptionsEl.Clear();

        // Add options
        for (int i = 0; i < menuOptions.Count; i++) {
            MenuOption menuOption = menuOptions[i];
            menuOptionsEl.Add(BuildMenuOption(menuOption, i));
        }
    }

    private VisualElement BuildMenuOption(MenuOption menuOption, int index) {
        VisualElement root = new VisualElement();
        root.AddToClassList("flex-row");
        root.AddToClassList("items-center");
        root.AddToClassList("mt-5");

        VisualElement indicator = new VisualElement();
        indicator.AddToClassList("w-3");
        indicator.AddToClassList("h-3");
        indicator.AddToClassList("border-b");
        indicator.AddToClassList("border-r");
        indicator.AddToClassList("border-black");
        indicator.AddToClassList("mt-1");
        indicator.AddToClassList("bg-yellow-500");
        indicator.AddToClassList("mr-5");
        if (index != selectedIndex) indicator.AddToClassList("opacity-0");

        Label label = new Label();
        label.AddToClassList("text-shadow");
        label.AddToClassList("text-xl");
        label.AddToClassList("text-white");
        label.text = menuOption.Title;

        root.Add(indicator);
        root.Add(label);

        return root;
    }
}
