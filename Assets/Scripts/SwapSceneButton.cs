using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapSceneButton : MonoBehaviour
{
    public GameManager gameManager;
    public ComponentManager componentManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        componentManager = GameObject.Find("ComponentManager").GetComponent<ComponentManager>();
    }
    public void SwapScenes()
    {
        string curScene = SceneManager.GetActiveScene().name;
        if (curScene == "PlayerOneSelection")
        {
            gameManager.SwapVisibility("PlayerTwo");
            SceneManager.LoadScene("PlayerTwoSelection");
        }
        else if (curScene == "PlayerTwoSelection")
        {
            gameManager.SwapVisibility("PlayerOne");
            SceneManager.LoadScene("PlayerOneSelection");
        }
    }
    public void ToggleBombButtonInteractable()
    {
        componentManager.ToggleButtonInteractable(componentManager.bombButton);
    }
}
