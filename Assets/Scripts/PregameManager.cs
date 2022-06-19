using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PregameManager: MonoBehaviour
{
    // This script manages the logic for pre-game setup, including setting usernames and game configuration

    public GameObject InputWindowPrefab;
    public GameObject messagePopupPrefab;

    public string playerOneUsername;
    public string playerTwoUsername;

    public GameObject[] helpMenuList;
    public GameObject helpMenu;

    public void CreateInputPopup(string text)
    {
        // Creates an input window based on a string
        if (GameObject.Find("InputWindow(Clone)") == null) 
        {
            int num = Int32.Parse(text.Substring(0, 1));
            string str = text.Substring(1, text.Length - 1);
            GameObject x = Instantiate(InputWindowPrefab, GameObject.Find("Canvas").transform);
            x.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = str;
            x.GetComponent<InputWindow>().playerNum = num;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void updateUsername(string input, int playerNum)
    {
        if (playerNum == 1)
        {
            playerOneUsername = input;
        }
        else if (playerNum == 2)
        {
            playerTwoUsername = input;
        }
    }

    public void Start()
    {
        // On Game Open
        helpMenu = GameObject.Find("HelpMenu");
        GameObject items = GameObject.Find("HelpItems");
        helpMenuList = new GameObject[items.transform.childCount];
        int i = 0;
        foreach (Transform child in items.transform)
        {
            helpMenuList[i] = child.gameObject;
            child.gameObject.SetActive(false);
            i += 1;
        }
        helpMenuList[0].SetActive(true);
        helpMenu.SetActive(false);
    }

    public void NextMenu(bool forwards)
    {
        // Used for traversing the user help menu
        for (int i = 0; i < helpMenuList.Length; i++)
        {
            if (helpMenuList[i].activeSelf)
            {
                helpMenuList[i].SetActive(false);
                if (forwards)
                {
                    if (i == helpMenuList.Length - 1)
                    {
                        helpMenuList[0].SetActive(true);
                    }
                    else
                    {
                        helpMenuList[i + 1].SetActive(true);
                    }
                    
                }
                else
                {
                    if (i == 0)
                    {
                        helpMenuList.Last().SetActive(true);
                    }
                    else
                    {
                        helpMenuList[i - 1].SetActive(true);
                    }
                }
                return;
            }
        }
    }

    public void SetHelpMenuState(bool state)
    {
        helpMenu.SetActive(state);
    }

    public void StartGame()
    {
        // Calls the game start, and checks whether both player's usernames have been set.
        if (playerOneUsername != "" && playerTwoUsername != "")
        {
            // start
            Debug.Log("Starting game...");
            SceneManager.LoadScene(1);
            StartCoroutine(SetUsernamesAfterSeconds());
        }
        else
        {
            CreatePopup("You haven't set both usernames!");
        }
    }

    IEnumerator SetUsernamesAfterSeconds(float seconds = 1f)
    {
        yield return new WaitForSeconds(seconds);
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.SetUsernames(playerOneUsername, playerTwoUsername);
    }

    public void CreatePopup(string text, float deleteSeconds = 3f)
    {
        GameObject popup = Instantiate(messagePopupPrefab, GameObject.Find("Canvas").transform);
        popup.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(DeleteObjectAfterSeconds(popup, deleteSeconds));
    }
    public IEnumerator DeleteObjectAfterSeconds(GameObject obj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(obj);
    }
}
