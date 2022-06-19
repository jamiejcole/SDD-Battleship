using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_SetPlayerUsername : MonoBehaviour
{
    // Used for setting the header to the players username based on the current player
    public PregameManager pregameManager;
    public TextMeshProUGUI playerText;

    private void Awake()
    {
        pregameManager = GameObject.Find("PregameManager").GetComponent<PregameManager>();
        
        playerText = gameObject.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();

        if (SceneManager.GetActiveScene().name == "PlayerOneSelection") { playerText.text = $"{pregameManager.playerOneUsername}'s Turn"; }
        else if (SceneManager.GetActiveScene().name == "PlayerTwoSelection") { playerText.text = $"{pregameManager.playerTwoUsername}'s Turn"; }
        //StartCoroutine(UpdateHeaderUsername());
    }
}
