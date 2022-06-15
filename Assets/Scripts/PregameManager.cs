using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PregameManager: MonoBehaviour
{
    public GameObject InputWindowPrefab;
    public GameObject messagePopupPrefab;

    public string playerOneUsername;
    public string playerTwoUsername;

    public void CreateInputPopup(string text)
    {
        int num = Int32.Parse(text.Substring(0, 1));
        string str = text.Substring(1, text.Length - 1);
        GameObject x = Instantiate(InputWindowPrefab, GameObject.Find("Canvas").transform);
        x.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = str;
        x.GetComponent<InputWindow>().playerNum = num;
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

    public void StartGame()
    {
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
