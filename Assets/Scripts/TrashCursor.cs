using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashCursor : MonoBehaviour
{
    public Material highlightMat;
    public Material defaultMat;

    ShipButtonManager shipButtonManager;
    SetupManager setupManager;

    private GameObject[] gameObjs;

    private void Start()
    {
        shipButtonManager = GameObject.Find("ShipButtonManager").GetComponent<ShipButtonManager>();
        setupManager = GameObject.Find("SetupManager").GetComponent<SetupManager>();
    }

    void Update()
    {
        // whole thing is dodgy, errors when deleting obj, all ships remain highlighted
        // could try using list instead of array

        //gameObjs = GameObject.FindGameObjectsWithTag("Ship");
        gameObjs = FindGameObjectsWithTags(new string[] { "PlayerOneVisible", "PlayerTwoVisible" });

        flushHighlight();
        transform.position = Input.mousePosition;

        RaycastHit hitInfo = new RaycastHit();
        int layerMask = LayerMask.GetMask("Ship");
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000, layerMask))
        {
            GameObject hitObj = hitInfo.transform.gameObject;
            hitObj.transform.GetComponent<MeshRenderer>().material = highlightMat;
            
        }

        if (Input.GetMouseButton(0))
        {
            flushHighlight();
            string shipName;

            // We try to get the 2x parent of the hit ship's mesh, if it doesn't exist, we didn't hit a ship, so return.
            try
            {
                shipName = hitInfo.transform.parent.parent.gameObject.name;
            }
            catch (Exception)
            {
                return;
            }

            Destroy(hitInfo.transform.parent.parent.gameObject);

            shipButtonManager.ToggleButton(shipName, true);

            // Add logic for removing the ship's occupied tile nums from the occupiedTiles list in SetupManager.cs
            // add a list in setupmanager.cs that keeps hold of each ships start tile positions 
            // from the corresponding ship pos or ship name, so that it can be called when
            // trying to determine the start tile index of a ships placement on the board
            string newShipName = shipName.Remove(0, 1).Remove(9, 7);
            (int, bool) shipStartData;

            setupManager.shipStartPositions.TryGetValue(newShipName, out shipStartData);

            setupManager.RemoveShipFromOccupied(setupManager.FindLengthOfShip(newShipName), shipStartData.Item1, shipStartData.Item2);
            // remove the item from the dict once remFromOccu is called
            setupManager.shipStartPositions.Remove(newShipName);

            Destroy(gameObject);
        }
        if (Input.GetMouseButton(1))
        {
            flushHighlight();
            Destroy(gameObject);
        }
    }

    GameObject[] FindGameObjectsWithTags(params string[] tags)
    {
        var all = new List<GameObject>();

        foreach (string tag in tags)
        {
            all.AddRange(GameObject.FindGameObjectsWithTag(tag).ToList());
        }

        return all.ToArray();
    }

    private void flushHighlight()
    {
        foreach (GameObject ship in gameObjs)
        {
            try
            {
                ship.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = defaultMat;

            }
            catch (System.Exception)
            {
                return;
            }
        }
    }
}
