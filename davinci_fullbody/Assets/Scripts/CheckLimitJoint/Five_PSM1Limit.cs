using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************
* Check Joint limit with trigger
* *******************************************************/

public class Five_PSM1Limit : MonoBehaviour
{
    public static bool collision_5_PSM1 = false;

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("PSM1) 5->3: " + collision_5_PSM1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "3_PSM1")
        {
            collision_5_PSM1 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "3_PSM1")
        {
            collision_5_PSM1 = false;
        }
    }
}
