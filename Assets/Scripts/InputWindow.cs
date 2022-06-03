using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputWindow : MonoBehaviour
{
    private PregameManager pregameManager;
    private TMP_InputField input;
    public int playerNum;

    private void Awake()
    {
        pregameManager = GameObject.Find("PregameManager").GetComponent<PregameManager>();
        input = transform.GetChild(2).GetComponent<TMP_InputField>();
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Ok()
    {
        string output = input.text;

        pregameManager.updateUsername(output, playerNum);
        Destroy();
    }
}
