using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Used between scenes to manage the pausemenu and it's state

    public GameObject pauseMenu;
    PauseMenu pauseMenuPauseMenu;
    public bool isPaused = false;

    private void Awake()
    {
        // Initalising the pause menu
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenuPauseMenu = pauseMenu.GetComponent<PauseMenu>();
        pauseMenuPauseMenu.HidePauseMenu();
    }
    void Update()
    {
        // Run once per frame
        if (pauseMenu.activeSelf)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }

        if (Input.GetKeyDown("p"))
        {
            if (!isPaused)
            {
                pauseMenu.SetActive(true);
                pauseMenuPauseMenu.RemoveAllBindToCursorObjects();
            }
            else
            {
                pauseMenuPauseMenu.SetHelpMenuState(false);
                pauseMenuPauseMenu.HidePauseMenu();
            }
        }
    }
}
