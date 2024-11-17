using UnityEngine;
using UnityEngine.UIElements;

public class OptionsManager : MonoBehaviour
{
    public VisualElement ui;

    public GameObject mainMenu;
    public GameObject optionMenu;

    public Button backButton;
    
    private void Start()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        backButton = ui.Q<Button>("BackButton");
        backButton.clicked += OnBackButtonClicked;
    }

    private void OnBackButtonClicked()
    {
        Debug.Log("Back Button clicked");
        HideMenu(optionMenu);
        mainMenu.SetActive(true);
        
    }
    public void HideMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
}
