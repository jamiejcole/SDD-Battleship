using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PregameManager: MonoBehaviour
{
    public GameObject InputWindowPrefab;
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
}
