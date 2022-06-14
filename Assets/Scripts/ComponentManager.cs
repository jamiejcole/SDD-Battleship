using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ComponentManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject playerRadar;
    public GameObject nextPlayerButton;

    public GameObject x2u01;
    public GameObject x3u01;
    public GameObject x3u02;
    public GameObject x4u01;
    public GameObject x5u01;
    public GameObject confirmMoveButton;
    public GameObject trashIcon;
    public GameObject randomPlace;

    public GameObject CameraRotator;
    public GameObject p2CameraRotator;
    public GameObject bombButton;

    public GameObject winnerMenu;


    bool curRadarEnabled;
    bool curMenuItemsEnabled;

    public GameObject scoreboardP1;
    [SerializeField]
    private List<GameObject> scoreboardP1Objs;

    public GameObject scoreboardP2;
    [SerializeField]
    private List<GameObject> scoreboardP2Objs;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRadar = GameObject.Find("PlayerRadar");
        nextPlayerButton = GameObject.Find("NextPlayerButton");
        ToggleRadarItems();

        ToggleItem(nextPlayerButton);

        x2u01 = GameObject.Find("2U 01");
        x3u01 = GameObject.Find("3U 01");
        x4u01 = GameObject.Find("3U 02");
        x3u02 = GameObject.Find("4U 01");
        x5u01 = GameObject.Find("5U 01");
        confirmMoveButton = GameObject.Find("ConfirmMoveButton");
        trashIcon = GameObject.Find("TrashIcon");
        randomPlace = GameObject.Find("RandomPlace");

        foreach (Transform child in scoreboardP1.transform)
        {
            if (child.gameObject.name.Contains("_"))
            {
                scoreboardP1Objs.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
        scoreboardP1.SetActive(false);

        foreach (Transform child in scoreboardP2.transform)
        {
            if (child.gameObject.name.Contains("_"))
            {
                scoreboardP2Objs.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
        }
        scoreboardP2.SetActive(false);
    }

    public void Update()
    {
        curRadarEnabled = playerRadar.activeSelf;
        curMenuItemsEnabled = x2u01.activeSelf;

        if (curRadarEnabled != gameManager.radarButtonEnabled)
        {
            ToggleRadarItems();
        }
        if (curMenuItemsEnabled != gameManager.setupMenuItemsEnabled)
        {
            ToggleSetupMenuItems();
        }
        if (!gameManager.inSetupMode)
        {
            ShowScoreboards();
        }
        else if (gameManager.inSetupMode)
        {
            HideScoreboards();
        }

        curRadarEnabled = gameManager.setupMenuItemsEnabled;
        curMenuItemsEnabled = gameManager.radarButtonEnabled;
    }

    private void ShowScoreboards()
    {
        scoreboardP1.SetActive(true);
        scoreboardP2.SetActive(true);
    }
    private void HideScoreboards()
    {
        scoreboardP1.SetActive(false);
        scoreboardP2.SetActive(false);
    }

    public void TriggerBombSelection()
    {
        gameManager.BombSelection();
    }

    public void CallNextButton()
    {
        gameManager.NextButton();
    }

    public void SwapCams()
    {
        bool state = !CameraRotator.activeSelf;
        CameraRotator.SetActive(state);
        p2CameraRotator.SetActive(!state);
        gameManager.inRadarMode = !state;
        bombButton.SetActive(!state);
        nextPlayerButton.SetActive(!state);

        if (gameManager.publicCurrentViewer == "PlayerOne") { gameManager.publicCurrentViewer = "PlayerTwo"; }
        else if (gameManager.publicCurrentViewer == "PlayerTwo") { gameManager.publicCurrentViewer = "PlayerOne"; }
    }

    public void UpdateScoreboard(string player, string ship)
    {
        Debug.Log($"updating scorebaord for player {player} and ship {ship}");
        if (player == "playerOne")
        {
            if (ship == "mShip_2_01(Clone)")
            {
                scoreboardP1Objs[0].SetActive(true);
            }
            else if (ship == "mShip_3_01(Clone)")
            {
                scoreboardP1Objs[1].SetActive(true);
            }
            else if (ship == "mShip_3_02(Clone)")
            {
                scoreboardP1Objs[2].SetActive(true);
            }
            else if (ship == "mShip_4_01(Clone)")
            {
                scoreboardP1Objs[3].SetActive(true);
            }
            else if (ship == "mShip_5_01(Clone)")
            {
                scoreboardP1Objs[4].SetActive(true);
            }
        }
        if (player == "playerTwo")
        {
            if (ship == "mShip_2_01(Clone)")
            {
                scoreboardP2Objs[0].SetActive(true);
            }
            else if (ship == "mShip_3_01(Clone)")
            {
                scoreboardP2Objs[1].SetActive(true);
            }
            else if (ship == "mShip_3_02(Clone)")
            {
                scoreboardP2Objs[2].SetActive(true);
            }
            else if (ship == "mShip_4_01(Clone)")
            {
                scoreboardP2Objs[3].SetActive(true);
            }
            else if (ship == "mShip_5_01(Clone)")
            {
                scoreboardP2Objs[4].SetActive(true);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DestroyAllDontDestroyOnLoadObjects()
    {

        var go = new GameObject("Sacrificial Lamb");
        DontDestroyOnLoad(go);

        foreach (var root in go.scene.GetRootGameObjects())
        {
            Destroy(root);
        }
    }

    public void PlayAgain()
    {
        gameManager.PlayAgain();
    }

    public void ReloadScoreboards(Dictionary<string, bool> p1Sinks, Dictionary<string, bool> p2Sinks)
    {
        StartCoroutine(reloadAfterSeconds(p1Sinks, p2Sinks, 0f));
    }

    private IEnumerator reloadAfterSeconds(Dictionary<string, bool> p1Sinks, Dictionary<string, bool> p2Sinks, float s)
    {
        yield return new WaitForSeconds(s);
        scoreboardP1.SetActive(true);
        scoreboardP2.SetActive(true);
        foreach (KeyValuePair<string, bool> x in p1Sinks)
        {
            if (x.Value == true)
            {
                int y = Int32.Parse(x.Key);
                scoreboardP1Objs[y].SetActive(true);
            }
        }
        foreach (KeyValuePair<string, bool> x in p2Sinks)
        {
            //Debug.Log($"{x.Key}: {x.Value}");
            if (x.Value == true)
            {
                int y = Int32.Parse(x.Key);
                scoreboardP2Objs[y].SetActive(true);
            }
        }
    }

    public void outputList(List<GameObject> x)
    {
        foreach (var y in x)
        {
            Debug.Log(y);
        }
    }

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }

    private void ToggleItem(GameObject obj)
    {
        bool state = !obj.activeSelf;
        obj.SetActive(state);
    }

    public void ToggleButtonInteractable(GameObject obj)
    {
        Button actualButton = obj.GetComponent<Button>();
        if (obj.GetComponent<Button>().interactable == true)
        {
            actualButton.interactable = false;
        }
        else
        {
            actualButton.interactable = true;
        }
        
    }

    public void ToggleButtonInteractable(GameObject obj, bool state)
    {
        Button actualButton = obj.GetComponent<Button>();
        actualButton.interactable = state;
    }



    public void ToggleRadarItems()
    {
        playerRadar.SetActive(!playerRadar.activeSelf);
    }
    public void ToggleRadarItems(bool state)
    {
        nextPlayerButton.SetActive(state);
    }


    // Example of overloading-
    public void ToggleSetupMenuItems()
    {
        bool state = !x2u01.activeSelf;
        x2u01.SetActive(state);
        x3u01.SetActive(state);
        x3u02.SetActive(state);
        x4u01.SetActive(state);
        x5u01.SetActive(state);
        confirmMoveButton.SetActive(state);
        trashIcon.SetActive(state);
        // TODO: remove this in future when it is not needed
        randomPlace.SetActive(state);
    }
    public void ToggleSetupMenuItems(bool state)
    {
        x2u01.SetActive(state);
        x3u01.SetActive(state);
        x3u02.SetActive(state);
        x4u01.SetActive(state);
        x5u01.SetActive(state);
        confirmMoveButton.SetActive(state);
        trashIcon.SetActive(state);
        // TODO: remove this in future when it is not needed
        randomPlace.SetActive(state);
    }
}
