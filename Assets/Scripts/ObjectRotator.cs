using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public GameObject CamRotator;

    void Update()
    {
        float yRotation = CamRotator.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
