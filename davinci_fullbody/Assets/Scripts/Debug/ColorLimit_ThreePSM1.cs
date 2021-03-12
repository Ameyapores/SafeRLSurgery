using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLimit_ThreePSM1 : MonoBehaviour
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
        if (!Three_PSM1Limit.collision_3_PSM1)
        {
            rend.material = ifTrue;
        }
        else
        {
            rend.material = ifFalse;
        }
    }
}
