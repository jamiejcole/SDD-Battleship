using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipButtonManager : MonoBehaviour
{
    public GameObject button_2_01;
    public GameObject button_3_01;
    public GameObject button_3_02;
    public GameObject button_4_01;
    public GameObject button_5_01;

    void Update()
    {
        if (GameObject.Find("mShip_2_01(Clone)") != null)
        {
            Button actualButton = button_2_01.GetComponent<Button>();
            actualButton.interactable = false;
        }
        if (GameObject.Find("mShip_3_01(Clone)") != null)
        {
            Button actualButton = button_3_01.GetComponent<Button>();
            actualButton.interactable = false;
        }
        if (GameObject.Find("mShip_3_02(Clone)") != null)
        {
            Button actualButton = button_3_02.GetComponent<Button>();
            actualButton.interactable = false;
        }
        if (GameObject.Find("mShip_4_01(Clone)") != null)
        {
            Button actualButton = button_4_01.GetComponent<Button>();
            actualButton.interactable = false;
        }
        if (GameObject.Find("mShip_5_01(Clone)") != null)
        {
            Button actualButton = button_5_01.GetComponent<Button>();
            actualButton.interactable = false;
        }
    }
}
