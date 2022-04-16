using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetupManager : MonoBehaviour
{
    public GameObject Ship_2_01;
    public GameObject Ship_3_01;
    public GameObject Ship_3_02;
    public GameObject Ship_4_01;
    public GameObject Ship_5_01;

    public SelectionManager selectionManager;
    public List<int> occupiedTiles = new List<int>();

    public bool CreateShip(GameObject tile, string type)
    {
        GameObject spawnObj;
        spawnObj = GetField(type);
        Vector3 originalPos;

        // If you click somewhere that's not a tile, just return false.
        try
        {
            originalPos = tile.transform.position;
        }
        catch (Exception)
        {
            return false;
        }
        
        Vector3 spawnPos = new Vector3(originalPos.x + 0.5f, originalPos.y + 1, originalPos.z + 0.5f);
        Quaternion rotation;

        if (selectionManager.isFacingDefault) { rotation = new Quaternion(); }
        else { rotation = Quaternion.AngleAxis(90, Vector3.up); }

        int LENGTH = FindLengthOfShip(type);
        int TILENUM = GetTileNumFromName(tile);
        bool DEFAULT = selectionManager.isFacingDefault;


        if (CheckLegalPlacement(LENGTH, TILENUM, DEFAULT))
        {
            Instantiate(spawnObj, spawnPos, rotation);
            foreach (var x in AddItemsToOccupied(LENGTH, TILENUM, DEFAULT))
            {
                occupiedTiles.Add(x);
            }
            return true;

            // Disabling the button selector
            //GameObject buttonObj = EventSystem.current.currentSelectedGameObject;
            //Button actualButton = buttonObj.GetComponent<Button>();
            //actualButton.interactable = false;
        }
        else
        {
            return false;
        }
        // TODO: Add an else, and highlight a tile line/ship red
    }

    // works
    private List<int> AddItemsToOccupied(int Length, int tileNum, bool isFacingDefault)
    {
        List<int> list = new List<int>();
        if (isFacingDefault)
        {
            for (int i = 0; i < Length; i++)
            {
                list.Add(tileNum + i);
            }
        }
        else
        {
            for (int i = 0; i < Length; i++)
            {
                list.Add(tileNum - i*10);
            }
        }
        return list;
    }

    // works
    public int FindLengthOfShip(string name)
    {
        string newLength = name.Substring(5, 1);
        int length = Int32.Parse(newLength);
        return length;
    }

    // works
    public int GetTileNumFromName(GameObject name)
    {
        string unmodif = name.name;
        string modifString = unmodif.Substring(unmodif.Length - 3, 2);
        int modifInt = Int32.Parse(modifString);
        return modifInt;
    }

    // DOESNT WORK
    private bool CheckLegalPlacement(int Length, int tileNum, bool isFacingDefault)
    {
        bool onNewLine = CheckOnNewLine(Length, tileNum, isFacingDefault);
        bool intersects = CheckShipIntersections(Length, tileNum, isFacingDefault);

        if (!onNewLine && !intersects)
        {
            return true;
        }
        return false;
    }

    // works
    private bool CheckShipIntersections(int Length, int tileNum, bool isFacingDefault)
    {
        List<int> newBoatTiles = AddItemsToOccupied(Length, tileNum, isFacingDefault);
        List<int> occupied = occupiedTiles;

        foreach (int newTile in newBoatTiles)
        {
            foreach (int occupiedTile in occupied)
            {
                if (occupiedTile == newTile)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // works
    public bool CheckOnNewLine(int Length, int tileNum, bool isFacingDefault)
    {
        // This function determines whether the selected length of placement
        // occurs on a new line or not.

        int prevRounded = 0;
        bool onNewLine = false;

        if (isFacingDefault)
        {
            for (int i = 0; i < Length; i++)
            {
                int val = i + tileNum;
                int rounded = (int)Math.Round(((i + tileNum) - 4.5f) / 10.0) * 10;

                if (i == 0) { prevRounded = rounded; }
                if (prevRounded != rounded)
                {
                    onNewLine = true;
                }
                prevRounded = rounded;
            }
        }
        else if (!isFacingDefault)
        {
            for (int i = 0; i < Length; i++)
            {
                int val = tileNum - i * 10;
                if (val < 0)
                {
                    onNewLine = true;
                }
            }
        }
        return onNewLine;
    }

    public GameObject GetField(string x)
    {
        GameObject result = this.GetType().GetField(x).GetValue(this) as GameObject;
        return result;
    }
}
