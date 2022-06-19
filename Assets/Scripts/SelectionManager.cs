using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{  
    // This script is used for managing the 'highlighting' of tile's based on the user's current ship selection.

    // Defining vars, setting default params
    public Material highlightMat;
    public Material defaultMat;

    public Material PlayerOneDefaultMat;
    public Material PlayerTwoDefaultMat;

    public Material redMat;

    public List<GameObject> tiles = new List<GameObject>();
    public List<GameObject> p2tiles = new List<GameObject>();

    private GameObject tileParent;
    private GameObject p2tileParent;
    public int selectedLength = 3;
    public bool isFacingDefault = true;
    public bool isHighlighting = false;

    string previousHit;
    private List<GameObject> redTiles = new List<GameObject>();
    public SetupManager setupManager;
    public string currentCursorShip;

    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // Appends each tilemap block to the tiles array
        tileParent = GameObject.Find("FloorBlocks");
        foreach (Transform child in tileParent.transform)
        {
            tiles.Add(child.gameObject);
        }
        p2tileParent = GameObject.Find("p2FloorBlocks");
        foreach (Transform child in p2tileParent.transform)
        {
            p2tiles.Add(child.gameObject);
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

    public void openTrash(GameObject pref)
    {
        GameObject trash = Instantiate(pref, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);

    }

    private Material GetDefaultMat()
    {
        // Determines the 'default' material for tiles based on which player's turn it is
        if (gameManager.publicCurrentViewer == "PlayerOne")
        {
            return PlayerOneDefaultMat;
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo")
        {
            return PlayerTwoDefaultMat;
        }
        else
        {
            return defaultMat;
        }
    }

    private Material GetDefaultMatInverse()
    {
        // Opposite of GetDefaultMat()
        if (gameManager.publicCurrentViewer == "PlayerOne")
        {
            return PlayerTwoDefaultMat;
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo")
        {
            return PlayerOneDefaultMat;
        }
        else
        {
            return defaultMat;
        }
    }

    void Update()
    {
        if (isHighlighting)
        {
            // Resetting the tiles each frame, if we are currently highlighting
            cleanTiles();

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
                            redTiles[i].GetComponent<MeshRenderer>().material = GetDefaultMat();
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
    }

    // Swaps a set of tiles between highlighted and non-highlighted
    private void swapTiles(string Hit, int Length, bool isHighlighted)
    {
        try
        {
            List<GameObject> tilesToUse = GetListToUse();

            Material mat;
            if (isHighlighted) { mat = highlightMat; } else { mat = GetDefaultMat(); }
            string preFormat = Hit.Remove(Hit.Length - 1);
            preFormat = preFormat.Substring(Hit.Length - 3);
            int hitNumber = Int32.Parse(preFormat);

            // Calls function to determine whether selection is on new line. Calling
            // from external instance of object for sake of reducing redundancy.
            bool onNewLine = setupManager.CheckOnNewLine(Length, hitNumber, isFacingDefault);
            
            // Swaps each tile colour if on same line
            if (onNewLine == false && setupManager.CheckLegalPlacement(Length, hitNumber, isFacingDefault)) {
                if (isFacingDefault)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        tilesToUse[hitNumber + i].GetComponent<MeshRenderer>().material = mat;
                    }
                }
                else if (!isFacingDefault)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        tilesToUse[hitNumber - i*10].GetComponent<MeshRenderer>().material = mat;
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
                        tilesToUse[rounded + i].GetComponent<MeshRenderer>().material = redMat;
                        redTiles.Add(tilesToUse[rounded + i]);
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
                        tilesToUse[next].GetComponent<MeshRenderer>().material = redMat;
                        redTiles.Add(tilesToUse[next]);
                        if (next == 90) 
                        {
                            tilesToUse[next + 10].GetComponent<MeshRenderer>().material = redMat;
                            redTiles.Add(tilesToUse[next + 10]);
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
        // Wipes all the current tiles to be the defualt materials/colours
        if (gameManager.publicCurrentViewer == "PlayerOne" && SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            foreach (GameObject obj in tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerOneDefaultMat;
            }
            foreach (GameObject obj in p2tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerTwoDefaultMat;
            }
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo" && SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            foreach (GameObject obj in tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerOneDefaultMat;
            }
            foreach (GameObject obj in p2tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerTwoDefaultMat;
            }
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo" && SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            foreach (GameObject obj in tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerTwoDefaultMat;
            }
            foreach (GameObject obj in p2tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerOneDefaultMat;
            }
        }
        else if (gameManager.publicCurrentViewer == "PlayerOne" && SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            foreach (GameObject obj in tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerTwoDefaultMat;
            }
            foreach (GameObject obj in p2tiles)
            {
                obj.GetComponent<MeshRenderer>().material = PlayerOneDefaultMat;
            }
        }
        
    }


    public bool mouseDownOnHighlight()
    {
        string parent = GetParentOfBlocks();

        return setupManager.CreateShip(GameObject.Find($"{parent}/{MakeCast()}"), currentCursorShip);
    }

    private string GetParentOfBlocks()
    {
        // Honestly, I have no idea what this does or what it is used for
        string parent;
        if (gameManager.publicCurrentViewer == "PlayerOne" && SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            parent = "FloorBlocks";
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo" && SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            parent = "p2FloorBlocks";
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo" && SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            parent = "FloorBlocks";
        }
        else if (gameManager.publicCurrentViewer == "PlayerOne" && SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            parent = "p2FloorBlocks";
        }
        else { parent = "FloorBlocks"; }
        return parent;
    }

    private List<GameObject> GetListToUse()
    {
        List<GameObject> tilesToUse;
        // we want to determine which tileset to use
        if (gameManager.publicCurrentViewer == "PlayerOne" && SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            tilesToUse = tiles;
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo" && SceneManager.GetActiveScene().name == "PlayerOneSelection")
        {
            tilesToUse = p2tiles;
        }
        else if (gameManager.publicCurrentViewer == "PlayerTwo" && SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            tilesToUse = tiles;
        }
        else if (gameManager.publicCurrentViewer == "PlayerOne" && SceneManager.GetActiveScene().name == "PlayerTwoSelection")
        {
            tilesToUse = p2tiles;
        }
        else { tilesToUse = tiles; }
        return tilesToUse;
    }

    public string MakeCast()
    {
        // Makes a raycast from the camera to determine which tile the user is selecting
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
        obj.GetComponent<MeshRenderer>().material = GetDefaultMat();
    }
}
