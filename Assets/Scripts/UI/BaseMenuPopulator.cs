using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct MenuOption {
    public string Title;
    public ItemType? ItemType;
    public UnityEngine.Events.UnityEvent Action;
    public Action NormalAction;
}

public abstract class BaseMenuPopulator : MonoBehaviour {
    // Fields
    [SerializeField] protected string title;
    [SerializeField] protected List<MenuOption> menuOptions;
    [SerializeField] protected PlayerInputManagerSO playerInput;

    // State
    protected int selectedIndex = 0;

    // UI
    protected UIDocument uiDocument;
    protected VisualElement uiRoot;

    protected abstract void OnEnable();
    protected abstract void OnDisable();

    protected void HandleOnPauseUp() {
        selectedIndex = (selectedIndex + menuOptions.Count - 1) % menuOptions.Count;
        BuildMenu();
    }

    protected void HandleOnPauseDown() {
        selectedIndex = (selectedIndex + 1) % menuOptions.Count;
        BuildMenu();
    }

    protected abstract void HandleSelect();

    protected void BuildMenu() {
        // Set title
        uiRoot.Q<Label>(name: "menu-title").text = title;

        // Get options holder
        VisualElement menuOptionsEl = uiRoot.Q(name: "menu-options");

        // Clear old options
        menuOptionsEl.Clear();

        // Add options
        for (int i = 0; i < menuOptions.Count; i++) {
            MenuOption menuOption = menuOptions[i];
            menuOptionsEl.Add(UIHelpers.BuildMenuOption(menuOption.Title, i, selectedIndex));
        }
    }
}
