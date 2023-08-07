using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct MenuOption {
    public string Title;
    public ItemType? ItemType;
    public UnityEngine.Events.UnityEvent Action;
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

    protected abstract void OnEnable();

    private void OnDisable() {
        playerInput.OnPauseDown -= HandleOnPauseDown;
        playerInput.OnPauseUp -= HandleOnPauseUp;
        playerInput.OnPauseSelect -= HandleSelect;
    }


    protected void HandleOnPauseUp() {
        if (selectedIndex == 0) {
            selectedIndex = menuOptions.Count - 1;
        }
        else {
            selectedIndex -= 1;
        }

        BuildMenu();
    }

    protected void HandleOnPauseDown() {
        if (selectedIndex == menuOptions.Count - 1) {
            selectedIndex = 0;
        }
        else {
            selectedIndex += 1;
        }

        BuildMenu();
    }

    protected abstract void HandleSelect();

    protected VisualElement BuildMenuOption(string title, int index) {
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
        label.text = title;

        root.Add(indicator);
        root.Add(label);

        return root;
    }

    protected void BuildMenu() {
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
            menuOptionsEl.Add(BuildMenuOption(menuOption.Title, i));
        }
    }
}
