using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject textObj;

    public GameObject Icon;
    public Texture playerOneIcon;
    public Texture playerTwoIcon;

    //textObj.GetComponent<TextMeshProUGUI>().text = "x";

    public void ChangePlayerMenu(int player)
    {
        Texture text;
        if (player == 1)
        {
            text = playerOneIcon;
        }
        else
        {
            text = playerTwoIcon;
        }
        textObj.GetComponent<TextMeshProUGUI>().text = $"Player {player.ToString()}'s Turn";
        Icon.GetComponent<RawImage>().texture = text;
    }
}
