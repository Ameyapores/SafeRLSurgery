using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
///This script is used for connect a toroid with PSM gripper
/// with FixedJoint and trigger for check collision
/// </summary>
public class AttachToroid_Trigger : MonoBehaviour
{
    /// Rigid body of the ee
    [SerializeField] private Rigidbody conBody2;

    // Represents if the toroid is token by the PSM2
    private bool _tookByPsm2;

    /// Bool if there is a FixedJoint enable from toroid and PSM2
    public static bool JointEnablePsm2;

    /// Bool check if toroid collide with PSM2 trigger
    private bool _triggerPsm2;

    /// Bool for not create double FixedJoint
    private bool _semJoint2 = true;

    /// Bool if psm2 is gripping 
    private bool _psm2IsGripping;

    /// Fixed joint used to connect ee to the toroid
    private FixedJoint _jointPsm2;

    private bool agenthitting = false;

    [SerializeField] private GameObject ee;

    void Update()
    {
        //------------- START PSM2---------------//
        if (Vector3.Distance(transform.position, ee.transform.position) <= 0.03f && _semJoint2)
        {
            _jointPsm2 = gameObject.AddComponent<FixedJoint>();
            _jointPsm2.connectedBody = conBody2;
            _jointPsm2.breakForce = 1000000;

            JointEnablePsm2 = true;
            _semJoint2 = false;
            _tookByPsm2 = true;

        }
        //------------- END PSM2---------------//

    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger_PSM2"))
        {
            agenthitting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger_PSM2"))
        {
            agenthitting = false;
        }
    }

    public bool getAgenthitting()
    {
        return agenthitting;
    }

    /// <summary>
    /// Getter method to return TookByPSM2 
    /// </summary>
    /// <returns>TookByPSM2 a boolean that represents if the toroid is token by PSM2</returns>
    public bool TookByPSM2()
    {
        return _tookByPsm2;
    }
    
    /// <summary>
    /// Setter method used to change _psm2IsGripping variable  
    /// </summary>
    /// <param name="value">True if the psm is gripping</param>
    public void SetPSM2_is_gripping(bool value)
    {
        _psm2IsGripping = value;
    }
    
    /// <summary>
    /// Function used to destroy the joint between the psm and the toroid 
    /// </summary>
    public void DestroyPsm2FixedJoint()
    {
        Destroy(_jointPsm2);
        _tookByPsm2 = false;
        JointEnablePsm2 = false;
        _semJoint2 = true;
    }   
    
}
