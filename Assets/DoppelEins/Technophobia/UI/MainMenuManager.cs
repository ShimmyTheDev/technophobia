using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    public VisualElement ui;
    public GameObject mainMenu;

    public Button newGameButton;
    public Button optionButton;
    public Button creditsButton;
    public Button quitGameButton;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        newGameButton = ui.Q<Button>("NewGameButton");
        newGameButton.clicked += OnNewGameButtonClicked;

        optionButton = ui.Q<Button>("OptionButton");
        optionButton.clicked += OnOptionButtonClicked;

        creditsButton = ui.Q<Button>("CreditsButton");
        creditsButton.clicked += CreditsGameButtonClicked;
        
        quitGameButton = ui.Q<Button>("QuitGameButton");
        quitGameButton.clicked += OnQuitGameButtonClicked;
    }

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private void OnNewGameButtonClicked()
    {
        // TODO: change in the future, as this means we have the main game running in the background
        Debug.Log("New Game Started");
        HideMenu();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Level01"));
    }
    private void OnOptionButtonClicked()
    {
        Debug.Log("Option Button Clicked");
    }
    private void CreditsGameButtonClicked()
    {
        Debug.Log("Credits Button Clicked");
    }
    private void OnQuitGameButtonClicked()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }

    public void HideMenu()
    {
        mainMenu.SetActive(false);
    }
}
