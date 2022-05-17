using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject playerRadar;

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


    bool curRadarEnabled;
    bool curMenuItemsEnabled;



    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerRadar = GameObject.Find("PlayerRadar");
        ToggleRadar();

        x2u01 = GameObject.Find("2U 01");
        x3u01 = GameObject.Find("3U 01");
        x4u01 = GameObject.Find("3U 02");
        x3u02 = GameObject.Find("4U 01");
        x5u01 = GameObject.Find("5U 01");
        confirmMoveButton = GameObject.Find("ConfirmMoveButton");
        trashIcon = GameObject.Find("TrashIcon");
        randomPlace = GameObject.Find("RandomPlace");
        
    }

    public void Update()
    {
        curRadarEnabled = playerRadar.activeSelf;
        curMenuItemsEnabled = x2u01.activeSelf;

        if (curRadarEnabled != gameManager.radarButtonEnabled)
        {
            ToggleRadar();
        }
        if (curMenuItemsEnabled != gameManager.setupMenuItemsEnabled)
        {
            ToggleSetupMenuItems();
        }

        curRadarEnabled = gameManager.setupMenuItemsEnabled;
        curMenuItemsEnabled = gameManager.radarButtonEnabled;
    }

    public void TriggerBombSelection()
    {
        gameManager.BombSelection();
    }

    public void SwapCams()
    {
        bool state = !CameraRotator.activeSelf;
        CameraRotator.SetActive(state);
        p2CameraRotator.SetActive(!state);
        gameManager.inRadarMode = !state;
        bombButton.SetActive(!state);

        if (gameManager.publicCurrentViewer == "PlayerOne") { gameManager.publicCurrentViewer = "PlayerTwo"; }
        else if (gameManager.publicCurrentViewer == "PlayerTwo") { gameManager.publicCurrentViewer = "PlayerOne"; }
    }


    public void ToggleRadar()
    {
        playerRadar.SetActive(!playerRadar.activeSelf);
    }
    public void ToggleRadar(bool state)
    {
        playerRadar.SetActive(state);
    }

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
