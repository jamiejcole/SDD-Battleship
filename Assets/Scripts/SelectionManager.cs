using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // Defining vars, setting default params
    public Material highlightMat;
    public Material defaultMat;
    public Material redMat;
    public List<GameObject> tiles = new List<GameObject>();
    private GameObject tileParent;
    public int selectedLength = 3;
    public bool isFacingDefault = true;

    string previousHit;
    private List<GameObject> redTiles = new List<GameObject>();
    public SetupManager setupManager;
    public string currentCursorShip;

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
        string l = newLength;
        string n = pref.name.Substring(4, 1);
        currentCursorShip = "Ship_" + l + "_0" + n;
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

            // Calls function to determine whether selection is on new line. Calling
            // from external instance of object for sake of reducing redundancy.
            bool onNewLine = setupManager.CheckOnNewLine(Length, hitNumber, isFacingDefault);
            
            // Swaps each tile colour if on same line
            if (onNewLine == false) {
                if (isFacingDefault)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        tiles[hitNumber + i].GetComponent<MeshRenderer>().material = mat;
                    }
                }
                else if (!isFacingDefault)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        tiles[hitNumber - i*10].GetComponent<MeshRenderer>().material = mat;
                    }
                }
            }

            else
            {
                // Finds the nearest lowest 10 of the hitNumber.
                // I.e. 78 returns 70, 43 returns 40 and so on.
                if (isFacingDefault)
                {
                    int rounded = (int)Math.Round((hitNumber - 4.5f) / 10.0) * 10;
                    for (int i = 0; i < 10; i++)
                    {
                        tiles[rounded + i].GetComponent<MeshRenderer>().material = redMat;
                        redTiles.Add(tiles[rounded + i]);
                    }
                }
                else if (!isFacingDefault)
                {
                    char smallest = hitNumber.ToString()[hitNumber.ToString().Length - 1];
                    string small = smallest.ToString();
                    for (int i = 0; i < 10; i++)
                    {
                        int intSmall = Int32.Parse(small);
                        int next = intSmall + (i) * 10;
                        tiles[next].GetComponent<MeshRenderer>().material = redMat;
                        redTiles.Add(tiles[next]);
                        if (next == 90) 
                        {
                            tiles[next + 10].GetComponent<MeshRenderer>().material = redMat;
                            redTiles.Add(tiles[next + 10]);
                        }
                    }
                }
            }
        }
        catch {
            //Debug.Log("E:" + e);
        }
    }

    public void cleanTiles()
    {
        foreach (GameObject obj in tiles)
        {
            obj.GetComponent<MeshRenderer>().material = defaultMat;
        }
    }

    public void mouseDownOnHighlight()
    {
        setupManager.CreateShip(GameObject.Find(MakeCast()), currentCursorShip);
    }

    public string MakeCast()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            if (hitInfo.transform.gameObject.tag == "FloorPlaceable")
            {
                // If we hit any tiles, we want to return them
                string hitName = hitInfo.transform.gameObject.name;
                return hitName;
            }
            else { return "false"; }
        }
        else { return "false"; }
    }

    // Legacy
    IEnumerator waiter(GameObject obj)
    {
        obj.GetComponent<MeshRenderer>().material = highlightMat;
        yield return new WaitForSeconds(0.1f);
        obj.GetComponent<MeshRenderer>().material = defaultMat;
    }
}
