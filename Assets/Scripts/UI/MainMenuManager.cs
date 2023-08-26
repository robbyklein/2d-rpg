using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] private AudioManager audio;
    [SerializeField] private DatabaseSO db;
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private UIDocument uiDocument;
    private VisualElement uiRoot;
    private VisualElement menuOptions;

    private int selectedIndex = 0;
    private List<string> languages = new List<string> { "English", "Spanish" };


    private void OnEnable() {
        playerInput.OnMenuLeft += HandleLeft;
        playerInput.OnMenuRight += HandleRight;
        playerInput.OnMenuSelect += StartGame;

    }

    private void OnDisable() {
        playerInput.OnMenuLeft -= HandleLeft;
        playerInput.OnMenuRight -= HandleRight;
        playerInput.OnMenuSelect -= StartGame;
    }

    private void HandleRight() {
        if (selectedIndex == languages.Count - 1) {
            selectedIndex = 0;
        }
        else {
            selectedIndex++;
        }

        BuildMenu();
        audio.PlaySFX(MusicFile.MenuChange);
    }

    private void HandleLeft() {
        if (selectedIndex == 0) {
            selectedIndex = languages.Count - 1;
        }
        else {
            selectedIndex--;
        }

        BuildMenu();
        audio.PlaySFX(MusicFile.MenuChange);
    }

    private IEnumerator Start() {
        PlayerPrefs.DeleteKey("playerLanguage");

        uiDocument = GetComponent<UIDocument>();
        uiRoot = uiDocument.rootVisualElement;
        menuOptions = uiRoot.Q(name: "main-menu-languages");
        BuildMenu();

        yield return new WaitForSeconds(1);
        uiRoot.Q(name: "main-menu-overlay").AddToClassList("opacity-0");
        yield return null;
    }

    private void StartGame() {
        string language = selectedIndex == 0 ? "en" : "es";
        PlayerPrefs.SetString("playerLanguage", language);
        db.ResetDatabase();
        gameManager.ChangeGameState(GameState.World);
        audio.PlaySFX(MusicFile.MenuSelect);
    }

    private void BuildMenu() {
        menuOptions.Clear();

        for (int i = 0; i < languages.Count; i++) {
            Label option = BuildMenuOption(languages[i], i == selectedIndex, i);
            menuOptions.Add(option);
        }
    }

    private Label BuildMenuOption(string text, bool selected, int i) {
        Label label = new Label(text);

        // Apply classes for styling
        label.AddToClassList("text-white");
        label.AddToClassList("text-xl");
        label.AddToClassList("my-5");
        label.AddToClassList("pb-3");

        if (i != 0) {
            label.AddToClassList("ml-10");
        }

        if (selected) {
            label.AddToClassList("border-b-2");
            label.AddToClassList("border-yellow-500");
        }

        return label;
    }
}
