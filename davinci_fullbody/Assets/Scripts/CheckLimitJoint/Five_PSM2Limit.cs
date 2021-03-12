using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************
* Check Joint limit with trigger
* *******************************************************/

public class Five_PSM2Limit : MonoBehaviour
{
    public static bool collision_5_PSM2 = false;

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("PSM2) 5->3: " + collision_5_PSM2);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "3_PSM2")
        {
            collision_5_PSM2 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "3_PSM2")
        {
            collision_5_PSM2 = false;
        }
    }
}