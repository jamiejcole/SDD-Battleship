using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    PauseMenu pauseMenuPauseMenu;
    public bool isPaused = false;

    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenuPauseMenu = pauseMenu.GetComponent<PauseMenu>();
        pauseMenuPauseMenu.HidePauseMenu();
    }
    void Update()
    {
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
