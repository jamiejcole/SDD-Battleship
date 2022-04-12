using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    public GameObject Ship_2_01;
    public GameObject Ship_3_01;
    public GameObject Ship_3_02;
    public GameObject Ship_4_01;
    public GameObject Ship_5_01;

    public void CreateShip(GameObject tile, string type)
    {
        GameObject spawnObj;
        Debug.Log(type);

        if (type == "Ship_2_01") { spawnObj = Ship_2_01; }
        else if (type == "Ship_3_01") { spawnObj = Ship_3_01; }
        else if (type == "Ship_3_02") { spawnObj = Ship_3_02; }
        else if (type == "Ship_4_01") { spawnObj = Ship_4_01; }
        else if (type == "Ship_5_01") { spawnObj = Ship_5_01; }
        else { return; }

        Vector3 originalPos = tile.transform.position;
        Vector3 spawnPos = new Vector3(originalPos.x + 0.5f, originalPos.y + 1, originalPos.z + 0.5f);

        Instantiate(spawnObj, spawnPos, new Quaternion());
    }
}
