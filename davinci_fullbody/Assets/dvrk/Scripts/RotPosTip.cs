using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPosTip : MonoBehaviour
{
    // Start is called before the first frame update
    public static Matrix4x4 tipToWorldMat;
    public static Matrix4x4 tipToWorldMatInv;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tipToWorldMat = Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1));
        tipToWorldMatInv = tipToWorldMat.inverse;

        //Debug.Log("Tip: \n"+ Matrix4x4.TRS(transform.position, transform.rotation, new Vector3(1, 1, 1)));
    }
}
