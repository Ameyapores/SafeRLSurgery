using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPosBase : MonoBehaviour
{
    // Start is called before the first frame update
    public static Matrix4x4 baseMat;
    public static Matrix4x4 baseMatInv;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        baseMat = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        baseMatInv = baseMat.inverse;

        //Debug.Log("Base: \n"+ baseMat);
        

    }
}
