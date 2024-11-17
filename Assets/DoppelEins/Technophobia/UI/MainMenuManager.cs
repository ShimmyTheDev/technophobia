using System.Collections;
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

    private void OnNewGameButtonClicked()
    {
        // TODO: change in the future, as this means we have the main game running in the background
        Debug.Log("New Game Started");
        HideMenu();
        StartCoroutine(LoadSceneAsync("Level01"));
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

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            // Optionally, you can add a loading screen or progress bar here
            yield return null; // Wait until the next frame
        }
        
        // Optionally, you can show a message or do something after loading is complete
        Debug.Log("Scene Loaded: " + sceneName);
    }
}
