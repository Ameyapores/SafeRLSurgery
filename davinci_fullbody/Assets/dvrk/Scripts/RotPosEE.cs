using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPosEE : MonoBehaviour
{
    public static Matrix4x4 EEMat;
    public static Matrix4x4 EEMatInv;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EEMat = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        EEMatInv = EEMat.inverse;

//        Debug.Log("Base: \n" + EEMat);


    }
}