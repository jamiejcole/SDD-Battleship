using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BindToCursor : MonoBehaviour
{
    SelectionManager selectionManager;
    Button actualButton;

    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        selectionManager.isHighlighting = true;

        GameObject buttonObj = EventSystem.current.currentSelectedGameObject;
        actualButton = buttonObj.GetComponent<Button>();
        
    }
    void Update()
    {
        transform.position = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            bool legal = selectionManager.mouseDownOnHighlight();
            if (legal) { 
                selectionManager.isHighlighting = false;
                selectionManager.cleanTiles();
                //actualButton.interactable = false;
                Destroy(gameObject);
            }
        }
        if (Input.GetMouseButton(1))
        {
            selectionManager.isHighlighting = false;
            selectionManager.cleanTiles();
            Destroy(gameObject);
        }
    }
}
