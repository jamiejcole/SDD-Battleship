using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Script is appended to the pause menu item and is used when the user opens the pause menu
    public GameObject helpMenu;
    ComponentManager componentManager;

    private void Awake()
    {
        // Run on start of existance
        helpMenu = GameObject.Find("HelpMenu");
        HelpMenu helpMenuHelpMenu = helpMenu.GetComponent<HelpMenu>();
        componentManager = GameObject.Find("ComponentManager").GetComponent<ComponentManager>();
        helpMenuHelpMenu.StartHelpMenu();
    }

    public void RemoveAllBindToCursorObjects()
    {
        // Removes any current objects in the player's 'hand' (cursor) when the pause menu is opened
        BindToCursor[] bindToCursors = (BindToCursor[]) GameObject.FindObjectsOfType (typeof(BindToCursor));
        TrashCursor[] trashCursors = (TrashCursor[])GameObject.FindObjectsOfType(typeof(TrashCursor));
        foreach (BindToCursor bindToCursor in bindToCursors)
        {
            bindToCursor.DestroyBindToCursor();
        }
        foreach (TrashCursor trashCursor in trashCursors)
        {
            trashCursor.DestoryTrashCursor();
        }
    }

    public void SetHelpMenuState(bool state)
    {
        helpMenu.SetActive(state);
    }
    public void HidePauseMenu()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        componentManager.DestroyAllDontDestroyOnLoadObjects();
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        HidePauseMenu();
    }
}
