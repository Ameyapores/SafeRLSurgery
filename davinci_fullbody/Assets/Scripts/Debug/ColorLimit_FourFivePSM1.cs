using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLimit_FourFivePSM1 : MonoBehaviour
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
        if (!Four_PSM1Limit.collision_4_PSM1 || !Five_PSM1Limit.collision_5_PSM1)
        {
            rend.material = ifTrue;
        }
        else
        {
            rend.material = ifFalse;
        }
    }
}
