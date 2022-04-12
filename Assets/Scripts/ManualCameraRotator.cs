using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCameraRotator : MonoBehaviour
{
    public float rotateSpeed = 50f;

    public SelectionManager selectionManager;
    private bool prevIsFacingDefault = true;

    void Update()
    {
        prevIsFacingDefault = selectionManager.isFacingDefault;
        float yRotation = transform.rotation.eulerAngles.y;
        if (yRotation > 103) { yRotation = yRotation - 360; }

        if (Input.GetKey("left shift")) { rotateSpeed = 100f; }
        else { rotateSpeed = 50f; }

        if ((Input.GetKey("d") || Input.GetKey("right")) && yRotation >= -12)
        {
            transform.Rotate(0, -rotateSpeed * Time.deltaTime, 0);
        }
        
        if ((Input.GetKey("a") || Input.GetKey("left")) && yRotation <= 102)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }

        if (yRotation > 45)
        {
            selectionManager.isFacingDefault = false;
        }

        else if (yRotation <= 45)
        {
            selectionManager.isFacingDefault = true;
        }

        if (prevIsFacingDefault != selectionManager.isFacingDefault)
        {
            selectionManager.cleanTiles();
            prevIsFacingDefault = selectionManager.isFacingDefault;
        }
    }
}
