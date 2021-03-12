using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLimit_FourFivePSM2 : MonoBehaviour
{
    [SerializeField] Material ifTrue;
    [SerializeField] Material ifFalse;

    //Save rander for optimization
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Four_PSM2Limit.collision_4_PSM2 || !Five_PSM2Limit.collision_5_PSM2)
        {
            rend.material = ifTrue;
        }
        else
        {
            rend.material = ifFalse;
        }
    }
}