using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BindToCursor : MonoBehaviour
{
    // This script is attached to the 2D Ship Icon on Instiantiation, and 'binds'
    // the 2D Ship Icon GameObject to the cursor by updating it's position to the
    // cursor each frame.

    SelectionManager selectionManager;
    GameManager gameManager;

    Button actualButton;

    // Assigning types
    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        selectionManager.isHighlighting = true;

        GameObject buttonObj = EventSystem.current.currentSelectedGameObject;
        actualButton = buttonObj.GetComponent<Button>();
        
    }
    void Update()
    {
        // Updating the position
        transform.position = Input.mousePosition;

        // If the user clicks the left mouse button
        if (Input.GetMouseButton(0))
        {
            if (gameObject.name == "BombSelector(Clone)")
            {
                string hitName = selectionManager.MakeCast();
                if (hitName != "false")
                {
                    // instantiate missile object at y20 above the tile
                    gameManager.LaunchMissile(gameManager.GetTileNumFromName(hitName));

                    selectionManager.isHighlighting = false;
                    selectionManager.cleanTiles();

                    Destroy(gameObject);
                }
            }
            else
            {
                bool legal = selectionManager.mouseDownOnHighlight();
                if (legal)
                {
                    selectionManager.isHighlighting = false;
                    selectionManager.cleanTiles();
                    //actualButton.interactable = false;
                    Destroy(gameObject);
                }
            }
        }

        // If the user clicks the right mouse button
        if (Input.GetMouseButton(1))
        {
            selectionManager.isHighlighting = false;
            selectionManager.cleanTiles();
            Destroy(gameObject);
        }
    }
}
