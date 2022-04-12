using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindToCursor : MonoBehaviour
{
    SelectionManager selectionManager;
    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
    }
    void Update()
    {
        transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            selectionManager.mouseDownOnHighlight();
            Destroy(gameObject);
        }
    }
}
