using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

/// <summary>
/// Heuristic script to use ml_agent with hydra system.
/// Must be attached to an heuristic brain
/// </summary>
public abstract class HeuristicLogic : MonoBehaviour// Decision
{
    /*
    /// Variable used to smooth the movement
    protected float _deltaMovement;
    /// Variable used to smooth the rotation
    protected float _deltaRotation;
    /// Psm to move
    protected GameObject _psm;

    protected GameObject ee;
    
    /// Variable for clutch to reset position and position
    protected Vector3 _helpStartHydraPos;
    /// Variable for clutch to reset position and rotation
    protected Quaternion _helpStartHydraRot;

    /// Offset position Hydra to end effector
    protected Vector3 _offsetHydraPos;
    //Offset rotation Hydra to end effector
    protected Quaternion _offsetHydraRot;
    
    /// Bool for stop motion 
    protected bool _semPos;

    /// Desired position where move end effector 
    protected Vector3 _desiredPosition;
    /// Desired direction where move end effector 
    protected Quaternion _desiredDirection;

    /// Agent's academy
    protected DvrkAcademy _dvrkAcademy;
    /// Number of the controller
    protected int Controller_number;
    
    private int _firsttime = 0;

    protected test_collision shadow;
    
    public override float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        if (_firsttime == 0 || _dvrkAcademy.GetStepCount() == 0)
        {
            _dvrkAcademy = FindObjectOfType<DvrkAcademy>();
            _deltaMovement = _dvrkAcademy.deltaMovement;
            _deltaRotation = _dvrkAcademy.deltaRotation;
            _semPos = false;
            Controller_number = 1;
            _psm = GameObject.Find("PSM2");
            if(_dvrkAcademy.GetPreventCollision())
                ee = GameObject.Find("ShadowpointEE2");
            else
                ee = GameObject.Find("pointEE2");
            _desiredDirection = ee.transform.rotation;
            _desiredPosition = ee.transform.position;
            test_collision [] shadows = FindObjectsOfType<test_collision>();
            //shadow = shadows[0].name == "ShadowPSM1" ? shadows[1] : shadows[0];
            //shadow.Start();
            _firsttime = 1;
            Debug.Log("Reset Heuristic Logic");
        }
        

        // Bumper pressed 
        if (SixenseInput.Controllers[Controller_number].GetButtonDown(SixenseButtons.BUMPER))
        {
            _semPos = false;
        }

        //Reset position and rotation of end effector
        if (SixenseInput.Controllers[Controller_number].GetButtonUp(SixenseButtons.BUMPER))
        {
            
            _helpStartHydraPos = SixenseInput.Controllers[Controller_number].Position / 500;
            _helpStartHydraRot = SixenseInput.Controllers[Controller_number].Rotation;

            _offsetHydraPos = ee.transform.position - _helpStartHydraPos;
            _offsetHydraRot = Quaternion.Euler(_helpStartHydraRot.eulerAngles);
            
            _semPos = true;
            
        }

        if (_semPos)
        {
            
            // position deisred based on the position of the hydra controller
            _desiredPosition =  (SixenseInput.Controllers[Controller_number].Position / 500 + _offsetHydraPos);
            _desiredDirection = Quaternion.Euler(SixenseInput.Controllers[Controller_number].Rotation.eulerAngles);
            _desiredDirection *= Quaternion.Euler(0,  0, 0);
        }
        //Debug.Log("desired position:"+_desiredPosition.x+" "+_desiredPosition.y+" "+_desiredPosition.z);


        float[] Actions = new float[4];
        
        //ImagePosition of the endefector
        Vector3 currentPosition = ee.transform.position;
        Vector3 DesiredPosition = _desiredPosition;
        for (int i = 0; i <= 2; i++)
        {
            var delta = DesiredPosition[i] - ee.transform.position[i];
            if (delta > _deltaMovement/3)
                Actions[i] = 1f;
            else if (delta < -_deltaMovement / 3)
                Actions[i] = 2f;
            else
                Actions[i] = 0f;
        }

        return Actions;
    }

    public override List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return new List<float>();
    }

    protected abstract void Start();*/
}