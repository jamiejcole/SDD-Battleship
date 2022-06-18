using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{
    public GameObject[] helpMenuList;
    public GameObject helpMenu;

    public void StartHelpMenu()
    {
        helpMenu = transform.gameObject;
        GameObject items = GameObject.Find("HelpItems");
        helpMenuList = new GameObject[items.transform.childCount];
        int i = 0;
        foreach (Transform child in items.transform)
        {
            helpMenuList[i] = child.gameObject;
            child.gameObject.SetActive(false);
            i += 1;
        }
        helpMenuList[0].SetActive(true);
        helpMenu.SetActive(false);
    }

    public void NextMenu(bool forwards)
    {
        for (int i = 0; i < helpMenuList.Length; i++)
        {
            if (helpMenuList[i].activeSelf)
            {
                helpMenuList[i].SetActive(false);
                if (forwards)
                {
                    if (i == helpMenuList.Length - 1)
                    {
                        helpMenuList[0].SetActive(true);
                    }
                    else
                    {
                        helpMenuList[i + 1].SetActive(true);
                    }

                }
                else
                {
                    if (i == 0)
                    {
                        helpMenuList.Last().SetActive(true);
                    }
                    else
                    {
                        helpMenuList[i - 1].SetActive(true);
                    }
                }
                return;
            }
        }
    }
}
