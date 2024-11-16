using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Original Code from https://github.com/Coding-with-Robby/ui-toolkit/blob/main/Assets/UI/PauseManager.cs
public class PauseManager : MonoBehaviour
{
    [SerializeField] private List<string> selectedItems;
    [SerializeField] private UIDocument uiDoc;
    private VisualElement rootEl;
    private VisualElement pauseEl;
    private VisualElement selectedItemsEl;

    private int selectedIndex = 0;
    
    private string activeClass = "pause-active";
    private string activeSelectedItemClass = "selected-item-active";

    private void OnEnable()
    {
        rootEl = uiDoc.rootVisualElement;
        pauseEl = rootEl.Q(className: "pause");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Open Pause");
            Open();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Close Pause");
            Close();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Debug.Log("Down Arrow");
            HandleDownArrow();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("Up Arrow");
            HandleUpArrow();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Debug.Log("Enter");
            HandleSelected();
        }
    }

    private VisualElement BuildMenuItem(string text, bool active, bool spaceTop)
    {
        VisualElement selectionItem = new VisualElement();
        selectionItem.AddToClassList("selected-item");
        if (active) selectionItem.AddToClassList("selected-item-active");
        if (spaceTop) selectionItem.AddToClassList("space-top");

        VisualElement textEl = new VisualElement();
        textEl.AddToClassList("selected-item-text");

        Label textElLabel = new Label(text);

        selectionItem.Add(textEl);
        textEl.Add(textElLabel);

        return selectionItem;
    }

    private void BuildSelections()
    {
        int currentIndex = 0;

        foreach (string menuItem in selectedItems)
        {
            selectedItemsEl.Add(BuildMenuItem(menuItem, currentIndex == 0, currentIndex != 0));
            currentIndex++;
        }
    }

    private void Open()
    {
        pauseEl.AddToClassList(activeClass);
    }
    private void Close()
    { 
        pauseEl.RemoveFromClassList(activeClass);
    }
    private void HandleDownArrow()
    {
        if (selectedIndex == selectedItems.Count - 1)
        {
            selectedIndex = 0;
        }
        else
        {
            selectedIndex++;
        }

        SelectSelected();
    }
    private void HandleUpArrow()
    {
        if (selectedIndex == selectedItems.Count + 1)
        {
            selectedIndex = 0;
        }
        else
        {
            selectedIndex--;
        }

        SelectSelected();
    }
    private void HandleSelected()
    {
        string selectedItem = selectedItems[selectedIndex];
        Debug.Log($"{selectedItem} has been selected, do something");
    }
    private void SelectSelected()
    {
        rootEl.Q(className: activeSelectedItemClass).RemoveFromClassList(activeSelectedItemClass);
        rootEl.Query(className: "selected-item").AtIndex(selectedIndex).AddToClassList(activeSelectedItemClass);
    }

}
