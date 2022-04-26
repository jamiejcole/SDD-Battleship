using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashCursor : MonoBehaviour
{
    public Material highlightMat;
    public Material defaultMat;

    private GameObject[] gameObjs;

    void Update()
    {
        // whole thing is dodgy, errors when deleting obj, all ships remain highlighted
        // could try using list instead of array

        gameObjs = GameObject.FindGameObjectsWithTag("Ship");

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
            Destroy(hitInfo.transform.gameObject);
            Destroy(gameObject);
        }
        if (Input.GetMouseButton(1))
        {
            flushHighlight();
            Destroy(gameObject);
        }
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
