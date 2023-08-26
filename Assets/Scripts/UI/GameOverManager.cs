using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[System.Serializable]
struct GameOverOption {
    public string Key;
    public UnityEngine.Events.UnityEvent Action;
}


public class GameOverManager : MonoBehaviour {
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private KeyLanguageSO keyLanguage;
    [SerializeField] private List<GameOverOption> menuOptions;
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private UIDocument uiDocument;

    private VisualElement uiRoot;
    private VisualElement menuOptionsEl;

    private int selectedIndex = 0;

    private void OnEnable() {
        uiRoot = uiDocument.rootVisualElement;
        menuOptionsEl = uiRoot.Q("menu-options");

        playerInput.OnMenuLeft += HandleLeft;
        playerInput.OnMenuRight += HandleRight;
        playerInput.OnMenuSelect += HandleSelect;

        BuildMenu();
    }

    private void OnDisable() {
        playerInput.OnMenuLeft -= HandleLeft;
        playerInput.OnMenuRight -= HandleRight;
        playerInput.OnMenuSelect -= HandleSelect;
    }

    private void HandleRight() {
        if (selectedIndex == menuOptions.Count - 1) {
            selectedIndex = 0;
        }
        else {
            selectedIndex++;
        }

        BuildMenu();
    }

    private void HandleLeft() {
        if (selectedIndex == 0) {
            selectedIndex = menuOptions.Count - 1;
        }
        else {
            selectedIndex--;
        }

        BuildMenu();
    }

    private void HandleSelect() {
        menuOptions[selectedIndex].Action?.Invoke();
    }

    private void BuildMenu() {
        menuOptionsEl.Clear();

        for (int i = 0; i < menuOptions.Count; i++) {
            string text = keyLanguage.RetrieveValue(menuOptions[i].Key);
            Label option = BuildMenuOption(text, i);
            menuOptionsEl.Add(option);
        }
    }

    private Label BuildMenuOption(string text, int index) {
        Label label = new Label(text);
        label.AddToClassList("text-white");
        label.AddToClassList("text-2xl");
        label.AddToClassList("border-b-2");

        if (index == selectedIndex) {
            label.AddToClassList("border-b-yellow-500");
            label.AddToClassList("pb-3");
        }

        if (index > 0) {
            label.AddToClassList("ml-12");
        }

        return label;
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    public void MainMenu() {
        gameManager.ChangeGameState(GameState.MainMenu);
    }
}
