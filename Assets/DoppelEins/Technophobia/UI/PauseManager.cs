using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseManager : MonoBehaviour
{
    public VisualElement ui;
    public Button resumeGameButton;
    public Button optionButton;
    public Button quitGameButton;

    private void Start()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        resumeGameButton = ui.Q<Button>("ResumeButton");
        resumeGameButton.clicked += OnResumeGameButtonClicked;

        optionButton = ui.Q<Button>("OptionButton");
        optionButton.clicked += OnOptionButtonClicked;

        quitGameButton = ui.Q<Button>("QuitGameButton");
        quitGameButton.clicked += OnQuitGameButtonClicked;
    }

    private void OnResumeGameButtonClicked()
    {
        Debug.Log("Resume Button Clicked");
    }
    private void OnOptionButtonClicked()
    {
        Debug.Log("Option Button Clicked");
    }
    private void OnQuitGameButtonClicked()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
    }
}
