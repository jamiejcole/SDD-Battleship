using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Defining vars, setting default params
    public Material highlightMat;
    public Material defaultMat;
    public Material redMat;
    public List<GameObject> tiles = new List<GameObject>();
    private GameObject tileParent;
    public int selectedLength = 3;

    string previousHit;
    private List<GameObject> redTiles = new List<GameObject>();

    private void Start()
    {
        // Appends each tilemap block to the tiles array
        tileParent = GameObject.Find("FloorBlocks");
        foreach (Transform child in tileParent.transform)
        {
            tiles.Add(child.gameObject);
        }
    }

    // Instantiates an image at the cursor of the player when button is clicked
    public void createShip(GameObject pref)
    {
        string newLength = pref.name.Substring(0, 1);
        selectedLength = Int32.Parse(newLength);
        GameObject ship = Instantiate(pref, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    void Update()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            if (hitInfo.transform.gameObject.tag == "FloorPlaceable")
            {
                // If we hit any tiles, we want to highlight them
                string hitName = hitInfo.transform.gameObject.name;
                swapTiles(hitName, selectedLength, true);

                // But, if the new tile is different to the old tile, make the previous tile non-highlighted
                if (previousHit != hitName && previousHit != null)
                {
                    swapTiles(previousHit, selectedLength, false);
                    for (int i = 0; i < redTiles.Count; i++)
                    {
                        redTiles[i].GetComponent<MeshRenderer>().material = defaultMat;
                    }
                }

                previousHit = hitName;
            }
            else
            {
                // If we don't hit any tiles, make the previous tile non-highlighted
                swapTiles(previousHit, selectedLength, false);
            }
        }
    }

    // Swaps a set of tiles between highlighted and non-highlighted
    private void swapTiles(string Hit, int Length, bool isHighlighted)
    {
        try
        {
            Material mat;
            if (isHighlighted) { mat = highlightMat; } else { mat = defaultMat; }
            string preFormat = Hit.Remove(Hit.Length - 1);
            preFormat = preFormat.Substring(Hit.Length - 3);
            int hitNumber = Int32.Parse(preFormat);

            // This for loop determines whether the selected length of placement
            // occurs on a new line or not. This will affect whether the line is
            // red highlighted (error) or highlighted yellow (fine) in the next
            // if-else statement.
            int prevRounded = 0;
            bool onNewLine = false;
            for (int i = 0; i < selectedLength; i++)
            {
                int val = i + hitNumber;
                int rounded = (int)Math.Round(((i + hitNumber) - 4.5f) / 10.0) * 10;

                if (i == 0) { prevRounded = rounded; }
                if (prevRounded != rounded)
                {
                    onNewLine = true;
                }
                prevRounded = rounded;
            }

            // Swaps each tile colour if on same line
            if (onNewLine == false) {
                for (int i = 0; i < Length; i++)
                {
                    tiles[hitNumber + i].GetComponent<MeshRenderer>().material = mat;
                }
            }

            else
            {
                // Finds the nearest lowest 10 of the hitNumber.
                // I.e. 78 returns 70, 43 returns 40 and so on.
                int rounded = (int)Math.Round((hitNumber - 4.5f) / 10.0) * 10;
                for (int i = 0; i < 10; i++)
                {
                    tiles[rounded + i].GetComponent<MeshRenderer>().material = redMat;
                    redTiles.Add(tiles[rounded + i]);
                }
            }
        }
        catch (Exception e) { Debug.Log("E:" + e); }
    }

    // Legacy
    IEnumerator waiter(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material = highlightMat;
        yield return new WaitForSeconds(0.1f);
        obj.GetComponent<MeshRenderer>().material = defaultMat;
    }
}
