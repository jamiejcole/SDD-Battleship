using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    public GameObject playerRadar;

    public GameObject x2u01;
    public GameObject x3u01;
    public GameObject x3u02;
    public GameObject x4u01;
    public GameObject x5u01;
    public GameObject confirmMoveButton;
    public GameObject trashIcon;
    public GameObject randomPlace;

    private void Start()
    {
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

    public void ToggleRadar()
    {
        playerRadar.SetActive(!playerRadar.activeSelf);
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
