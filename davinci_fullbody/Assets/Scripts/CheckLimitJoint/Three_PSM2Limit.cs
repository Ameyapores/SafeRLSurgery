using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************
* Check Joint limit with trigger
* *******************************************************/

public class Three_PSM2Limit : MonoBehaviour
{
    public static bool collision_3_PSM2 = false;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("PSM2) 3->2: " + collision_3_PSM2);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "2_PSM2")
        {
            collision_3_PSM2 = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "2_PSM2")
        {
            collision_3_PSM2 = false;
        }
    }
}
