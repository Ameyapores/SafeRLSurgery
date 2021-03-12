using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPosB : MonoBehaviour
{
    // Start is called before the first frame update
    public static Matrix4x4 BToWorldMat;
    public static Matrix4x4 BToWorldMatInv;

    private Vector3 pB_True;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BToWorldMat = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        BToWorldMatInv = BToWorldMat.inverse;

        pB_True = new Vector3(BToWorldMat.m03, BToWorldMat.m13, BToWorldMat.m23);

        //Debug.Log("B: \n" + BToWorldMat);
        //Debug.Log("pB true: " + pB_True.ToString("F6"));
    }
}
