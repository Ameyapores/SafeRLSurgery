using DVRK;
using System.Collections.Generic;
using NVIDIA.Flex;
using UnityEngine;

public class MatrixOperationPSM : MonoBehaviour
{
    public List<URDFJoint> independentJoints = new List<URDFJoint>();

    [SerializeField] GameObject EE;
    Matrix4x4 tipToWorldMat;
    [SerializeField] bool activeIK;


    Matrix4x4 baseMat_To_tipToWorld;    //Base to Tip(C) 
    Vector3 nC;                         //unit vector of C
    Vector3 nEE;                        //unit vector of EE
    Vector3 t_Tc;                       //t of BaseToTip
    Vector3 p;                          //t of BaseToTip normalized
    Vector3 nB;                         //nC x p
    Vector3 vB;                         //unit vector from B to C   
    Vector3 pC;                         //traslation vector of C
    Vector3 pB;                         //traslation vector of B
    Vector3 pO;                         //traslation vector of Base
    Vector3 pEE;

    Vector3 nX;
    Vector3 nY;
    Vector3 nZ;

    Vector3 C_To_B;
    Vector3 B_To_Base;
    Vector3 Base_To_C;
    Vector3 B_To_C;
    Vector3 C_To_EE;
    Vector3 Base_To_X;
    Vector3 Base_To_B;

    float joint6_yaw;
    float joint5_pitch;
    float joint4_roll;
    float joint3_prismatic;
    float joint2_pitch;
    float joint1_yaw;

    float offsetBC = 0.09099992f;             //offset B to C
    float offsetCEE = 0.1059f;
    float offsetPrismatic = 0.01560001f;

    private void Start()
    {
        nX = Vector3.right;
        nY = Vector3.up;
        nZ = Vector3.forward;
        FlexActor.relGrasp = false;
        FlexActor.activeGrab = false;
        FlexActor.resFatPos = false;
    }
    void Update()
    {
        tipToWorldMat = Matrix4x4.TRS(new Vector3(EE.transform.position.x, EE.transform.position.y + 0.01059f, EE.transform.position.z), EE.transform.rotation, new Vector3(1, 1, 1));

        baseMat_To_tipToWorld = tipToWorldMat * RotPosBase.baseMatInv; //RotPosTip.tipToWorldMatInv * RotPosBase.baseMat
        if(!activeIK)
            baseMat_To_tipToWorld = RotPosTip.tipToWorldMat * RotPosBase.baseMatInv;

        nC = - new Vector3 (baseMat_To_tipToWorld.m02, baseMat_To_tipToWorld.m12, baseMat_To_tipToWorld.m22).normalized;
        nEE = - new Vector3(baseMat_To_tipToWorld.m00, baseMat_To_tipToWorld.m10, baseMat_To_tipToWorld.m20).normalized;

        if (!activeIK)
        {
            //----THIS
            pC = new Vector3(RotPosTip.tipToWorldMat.m03, RotPosTip.tipToWorldMat.m13, RotPosTip.tipToWorldMat.m23);
            pEE = pC + (offsetCEE * nEE);
            
        }
        //----OR THIS
        pEE = new Vector3(EE.transform.position.x, EE.transform.position.y, EE.transform.position.z);
        pC = pEE - (offsetCEE * nEE);

        Base_To_C = pC - pO;

        p = Base_To_C.normalized;

        nB = - Vector3.Cross(p, nC).normalized;
        vB =  Vector3.Cross(nC, nB).normalized;
        pO = new Vector3(RotPosBase.baseMat.m03, RotPosBase.baseMat.m13, RotPosBase.baseMat.m23);

        pB = pC + (offsetBC * vB);

        //Debug.Log("pB: " + pB.ToString("F6"));

        C_To_B = pB - pC;
        B_To_Base = pO - pB;
        Base_To_C = pC - pO;
        B_To_C = pC - pB;
        C_To_EE = pEE - pC;
        Base_To_B = pB - pO;

        
        //Debug.DrawRay(pB, B_To_Base.normalized, Color.green);
        //Debug.DrawRay(pC, B_To_C.normalized, Color.red);
        

        /*
        Debug.DrawLine(pC, pB, Color.yellow,0,false);
        Debug.DrawLine(pC, pEE, Color.green,0,false);
        Debug.DrawLine(pB, pO, Color.red,0,false);
        Debug.DrawLine(pO, pC, Color.black, 0, false);
        */


        joint1_yaw = Quaternion.FromToRotation(Base_To_B, nY).eulerAngles.z -180;
        if (joint1_yaw < 360 && joint1_yaw > 150)
            joint1_yaw = joint1_yaw - 360;
        joint1_yaw = -joint1_yaw;
        //Debug.Log("Value joint1_yaw: " + joint1_yaw);

        joint2_pitch = Quaternion.FromToRotation(Base_To_B, nY).eulerAngles.x;
        if (joint2_pitch < 360 && joint2_pitch > 150)
            joint2_pitch = joint2_pitch - 360;
        //Debug.Log("Value joint2_pitch: " + joint2_pitch);

        joint3_prismatic = Vector3.Distance(pB, pO) / 10+ offsetPrismatic;
        //Debug.Log("joint3_prismatic: " + (joint3_prismatic));

        joint4_roll = Vector3.Angle(nB, Vector3.Cross(nZ, Base_To_B)) - 180;
        if (Vector3.Dot(B_To_Base, Vector3.Cross(Vector3.Cross(nZ, Base_To_B), nB)) <=0 ) 
            joint4_roll = -joint4_roll;
        //Debug.Log("joint4_roll: " + (joint4_roll));

        joint5_pitch = 180 - Vector3.Angle(B_To_C, B_To_Base);
        if (Vector3.Dot(nB , (Vector3.Cross(B_To_C, B_To_Base)))>=0)
            joint5_pitch = -joint5_pitch;
        //Debug.Log("joint5_pitch: " + (joint5_pitch));

        joint6_yaw = 180 -  Vector3.Angle(C_To_B, C_To_EE);
        if (Vector3.Dot(nC, (Vector3.Cross(C_To_B, C_To_EE))) <= 0 )
            joint6_yaw = -joint6_yaw;
        //Debug.Log("joint6_yaw: " + ( joint6_yaw));

        if (activeIK)
        {
            independentJoints[0].SetJointValue(joint1_yaw);
            independentJoints[1].SetJointValue(joint2_pitch);
            independentJoints[2].SetJointValue(joint3_prismatic);
            independentJoints[3].SetJointValue(joint4_roll);
            independentJoints[4].SetJointValue(joint5_pitch);
            independentJoints[5].SetJointValue(joint6_yaw);
        }
    }

    private Vector3 MyCrossProd(Vector3 A, Vector3 B)
    {
        Vector3 C;
        C.x = (A.y * B.z) - (A.z * B.y);
        C.y = (A.z * B.x) - (A.x * B.z);
        C.z = (A.x * B.y) - (A.y * B.x);
        return C;
    }

}
