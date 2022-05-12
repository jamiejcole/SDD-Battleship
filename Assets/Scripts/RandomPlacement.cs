using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    SetupManager setupManager;
    GameManager gameManager;
    ComponentManager componentManager;

    public GameObject P1T1;
    public GameObject P1T2;
    public GameObject P1T3;
    public GameObject P1T4;
    public GameObject P1T5;

    public GameObject P2T1;
    public GameObject P2T2;
    public GameObject P2T3;
    public GameObject P2T4;
    public GameObject P2T5;

    private void Start()
    {
        setupManager = GameObject.Find("SetupManager").GetComponent<SetupManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        componentManager = GameObject.Find("ComponentManager").GetComponent<ComponentManager>();
    }

    public void GenRandomPlacements(bool playerOne)
    {
        if (playerOne)
        {
            setupManager.CreateShip(P1T1, "Ship_2_01");
            setupManager.CreateShip(P1T2, "Ship_3_01");
            setupManager.CreateShip(P1T3, "Ship_3_02");
            setupManager.CreateShip(P1T4, "Ship_4_01");
            setupManager.CreateShip(P1T5, "Ship_5_01");
        }
        else if (!playerOne)
        {
            setupManager.CreateShip(P2T1, "Ship_2_01");
            setupManager.CreateShip(P2T2, "Ship_3_01");
            setupManager.CreateShip(P2T3, "Ship_3_02");
            setupManager.CreateShip(P2T4, "Ship_4_01");
            setupManager.CreateShip(P2T5, "Ship_5_01");
        }
    }

    public void TriggerHideMenuItems()
    {
        componentManager.ToggleSetupMenuItems(true);
    }
    public void TriggerShowMenuItems()
    {
        componentManager.ToggleSetupMenuItems(false);
    }
}
