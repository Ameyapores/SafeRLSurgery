using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*********************************************************
* Check Joint limit with trigger
* *******************************************************/

public class Three_PSM1Limit : MonoBehaviour
{
    
    public static bool collision_3_PSM1 = false;

    void Update()
    {
        //Debug.Log("PSM1) 3->2: " + collision_3_PSM1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "2_PSM1")
        {
            collision_3_PSM1 = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "2_PSM1")
        {
            collision_3_PSM1 = false;
        }
    }
}
