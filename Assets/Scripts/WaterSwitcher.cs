using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSwitcher : MonoBehaviour
{
    // Deprecated, used for debugging, do not ship

    public GameObject water;
    public GameObject fakeWater;

    // Start is called before the first frame update
    void Start()
    {
        water.SetActive(true);
        fakeWater.SetActive(false);
    }
}
