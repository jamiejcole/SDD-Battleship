using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject helpMenu;

    private void Awake()
    {
        helpMenu = GameObject.Find("HelpMenu");
        HelpMenu helpMenuHelpMenu = helpMenu.GetComponent<HelpMenu>();
        helpMenuHelpMenu.StartHelpMenu();
    }

    public void RemoveAllBindToCursorObjects()
    {
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
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        HidePauseMenu();
    }
}
