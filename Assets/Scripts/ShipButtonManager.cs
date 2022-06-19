using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipButtonManager : MonoBehaviour
{
    // Used to handle the interactibility of the ship placement buttons based on
    // whether the corresponding ships have been placed or not

    public GameObject button_2_01;
    public GameObject button_3_01;
    public GameObject button_3_02;
    public GameObject button_4_01;
    public GameObject button_5_01;
    public GameObject confirmMoveButton;

    public List<GameObject> buttons;
    public bool allDisabled = false;

    private void Start()
    {
        buttons.Add(button_2_01);
        buttons.Add(button_3_01);
        buttons.Add(button_3_02);
        buttons.Add(button_4_01);
        buttons.Add(button_5_01);
    }

    void Update()
    {
        allDisabled = true;
        foreach (GameObject button in buttons)
        {
            if (button.GetComponent<Button>().interactable == true)
            {
                allDisabled = false;
            }
        }
        if (allDisabled)
        {
            confirmMoveButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            confirmMoveButton.GetComponent<Button>().interactable = false;
        }

    }

    public void ToggleButton(string name, bool enabled)
    {
        if (name == "mShip_2_01(Clone)")
        {
            Button actualButton = button_2_01.GetComponent<Button>();
            actualButton.interactable = enabled;
        }
        if (name == "mShip_3_01(Clone)")
        {
            Button actualButton = button_3_01.GetComponent<Button>();
            actualButton.interactable = enabled;
        }
        if (name == "mShip_3_02(Clone)")
        {
            Button actualButton = button_3_02.GetComponent<Button>();
            actualButton.interactable = enabled;
        }
        if (name == "mShip_4_01(Clone)")
        {
            Button actualButton = button_4_01.GetComponent<Button>();
            actualButton.interactable = enabled;
        }
        if (name == "mShip_5_01(Clone)")
        {
            Button actualButton = button_5_01.GetComponent<Button>();
            actualButton.interactable = enabled;
        }
    }

    public void ConfirmMove()
    {
        // call some shiz from GameManager, update its PlayerShip Lists
    }
}
