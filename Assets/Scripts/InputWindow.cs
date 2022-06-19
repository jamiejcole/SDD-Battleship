using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputWindow : MonoBehaviour
{
    // Class used to manage the instantiated input windows used for collecting player usernames

    private PregameManager pregameManager;
    private TMP_InputField input;
    public int playerNum;

    private void Awake()
    {
        pregameManager = GameObject.Find("PregameManager").GetComponent<PregameManager>();
        input = transform.GetChild(2).GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Ok();
        }
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
        pregameManager.CreatePopup($"Set Player {playerNum}'s name to '{output}'!", 2f);
    }
}
