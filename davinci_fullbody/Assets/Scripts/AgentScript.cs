using DVRK;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Reflection;
using NVIDIA.Flex;
//using UnityEditor.Experimental.GraphView;
using Random = System.Random;
using Unity.MLAgents.Sensors;
public class AgentScript : Agent
{
    private greenpixels greenChecker;
    /// Point _ee start position
    [SerializeField] private test_collision shadow;
    /// Point _ee start position
    [FormerlySerializedAs("startPosRot")] [SerializeField] private GameObject ee;
    /// Left claw of the grippe 
    [SerializeField] private URDFJoint leftGripper;
    /// Right claw of the gripper
    [SerializeField] private URDFJoint rightGripper;
    /// Academy's agent 
    [SerializeField] private DvrkAcademy dvrkAcademy;
    /// Initial position of the ee in order to reset the gripper position   
    private Vector3 _resetPosition;
    /// Initial rotation of the ee in order to reset the gripper rotation   
    private Quaternion _resetRotation;
    /// Multiplicative constant for a correct movement of the clamp's position
    private float _deltaMovement;
    /// Multiplicative constant for a correct movement of the clamp's rotation
    private float _deltaRotation;
    /// Boolean for enable scene limits
    private bool _enableSceneLmits;
    /// Max limit for pegs and rings plane 
    private GameObject _limitMax;
    /// Min limit for pegs and rings plane 
    private GameObject _limitMin;
    /// Array containing all rings in the scene
    //[SerializeField] private AttachToroid_Trigger[] _rings;
    private GameObject _gettoppoint;
    /// Array containing all pegs in the scene
    private GameObject[] _pegs;

    /// Actual ee position and rotation
    private GameObject[] _fat;
    
    private Transform _eePositionRotation;
    /// Boolean for when a ring exit from the platform
    //private bool _aRingHasFallen;
    
    /// Backup Rotation of EE
    private Vector3 _beeRotation;
    /// Backup Position of EE used for the shadow
    private Vector3 _beePosition;
    ///Backup Backup Rotation of EE used for the shadow
    private Vector3 _bbeeRotation;
    ///Backup Backup Position of EE used for the shadow
    private Vector3 _bbeePosition;
    
    /// Index which represents the ring that the agent has to take
    //private int _ringToTake;
    /// Boolean that represents if the agent is hitting a pole
    /// Boolean that represents if the agent has already completed the ring
    /// It's used to give the rewards to the agent 
    private bool[] _alreadyCompleted;

    private int step;
    
    private bool isGrippin = false;
    private Vector3 Kidney_reset;
    
    private Vector3 fat_reset_position;

    private Quaternion fat_reset_rotation;
    private Vector3 ee_reset_offset;
    void Start() 
    {
        _alreadyCompleted = new bool[1];
        _resetPosition = ee.transform.position;
        _resetRotation = ee.transform.rotation;
        
        _beeRotation = _beePosition = Vector3.zero;
        
        _deltaMovement = dvrkAcademy.deltaMovement;
        _deltaRotation = dvrkAcademy.deltaRotation;
        _limitMax = dvrkAcademy.GetLiminitMax();
        _limitMin = dvrkAcademy.GetLiminitMin();
        _gettoppoint = dvrkAcademy.Gettoppoint();
        _enableSceneLmits = dvrkAcademy.GetEnableSceneLimits();
        _pegs = dvrkAcademy.GetPegs();
        _fat = dvrkAcademy.Getfat();
        _eePositionRotation = ee.transform;
        step = 0;
        Kidney_reset = GameObject.Find("Lesion").transform.position;
        fat_reset_position = _fat[0].transform.position;
        //fat_reset_position = GameObject.Find("Fat").transform.position;
        //fat_reset_rotation = GameObject.Find("Fat").transform.rotation;
        fat_reset_rotation = _fat[0].transform.rotation;
        greenChecker = GameObject.Find("securitycamera").GetComponent<greenpixels>();
        RandomSpawn();
        
    }

    
    private void RandomSpawn()
    {
        Random random = new Random();  
        float x = random.Next(-50 , 50)/100f;
        float z = random.Next(-50, 50)/100f;
        ee_reset_offset=new Vector3(x,0,z);

        //egs_reset_offset=new Vector3(0,0,0);
        Debug.Log("random");
    }
    // public override void CollectObservations(VectorSensor sensor)
    // {
    //     //AddVectorObs(dvrkAcademy.GetObservation());
    //     sensor.AddObservation(dvrkAcademy.GetObservation());
    // }

    
    public override void OnActionReceived(float[] action)
    {
        
        step++;

        CalculateRewards();
        for(int i = 0; i<3;i++) 
            action[i] = action[i] == 2 ? -1 : action[i];

        Vector3 position = new Vector3(action[0]*_deltaMovement,action[1]*_deltaMovement,action[2]*_deltaMovement);
        Vector3 rotation =new Vector3( 0,0,0);
        //float nipperValue = 0;
        
        // If the previous move was legit
        if (dvrkAcademy.GetPreventCollision())
        {
            PreventCollision( position, rotation);
        }
        else
        {
            SetAgentPosition(position, rotation);
        }

        if (AmIDone())
        {
            OnEpisodeBegin();
            //AgentOnDone()
            Debug.Log("I am done");
            EndEpisode();
        }

        
    }

    private bool AmIDone()
    {

        // if (FlexActor.activeGrab == true)
        //     return (Vector3.Distance(_gettoppoint.transform.position, ee.transform.position) < 0.2);
        
        return false;
    }

    public Vector3[] GridOfPoints(float startX, float endX, float y, float startZ, float endZ)
    {
        float rangeX = endX - startX;
        float rangeZ = endZ - startZ;
        float helpX = startX;
        float helpZ = startZ;
        float stepX = rangeX / 9;
        float stepZ = rangeZ / 9;
        int cnt = 0;

        Vector3[] grid = new Vector3[100];

        for (float x = startX; x <= endX; x += stepX)
        {
            for (float z = startZ; z <= endZ; z += stepZ)
            {
                //Debug.Log(cnt);
                grid[cnt] = new Vector3(x, y, z);
                cnt++;
            }
        }
        return grid;
    }

    Vector3[] grid = new Vector3[100];
    bool sem = true;
    int count = 0;
    public override void OnEpisodeBegin()
    {
        if (sem)
        {
            grid = GridOfPoints(-0.42f,0.378f, 0f,0.046f,0.841f);
            sem = false;
        }
        RandomSpawn();
        if (step == 0)
            FlexActor.relGrasp = false;
        else
            FlexActor.relGrasp = true;
                
        FlexActor.resFatPos = true;
        ee.transform.position = _resetPosition + grid[count];
        //ee.transform.position = _resetPosition + ee_reset_offset ;
        ee.transform.rotation = _resetRotation;
        _beePosition=new Vector3(0,0,0);
        _beeRotation=new Vector3(0,0,0);
        _alreadyCompleted = new bool[1];
        
        shadow.Start();
    }

    /// <summary>
    /// Function used to set the agent position in the environment 
    /// </summary>
    /// <param name="pos"> Desired position </param>
    /// <param name="rotation"> Desired direction </param>
    private void SetAgentPosition(Vector3 pos, Vector3 rotation)
    {
        var tmp = _eePositionRotation.transform.transform.eulerAngles.x + rotation.x;
        rotation.x = tmp > 360 ? tmp - 360 : tmp;
        tmp = _eePositionRotation.transform.transform.eulerAngles.y + rotation.y;
        rotation.y = tmp > 360 ? tmp - 360 : tmp;
        tmp = _eePositionRotation.transform.transform.eulerAngles.z + rotation.z;
        rotation.z = tmp > 360 ? tmp - 360 : tmp;
        _eePositionRotation.transform.transform.eulerAngles = rotation;

        //Controls of the limits in X Y Z
        if (_enableSceneLmits)
        {
            Vector3 desired = ee.transform.position + pos;
            if (desired.x > _limitMax.transform.position.x || desired.x < _limitMin.transform.position.x)
                pos.x = 0f;
            if (desired.y > _limitMax.transform.position.y || desired.y < _limitMin.transform.position.y)
                pos.y = 0f;
            if (desired.z > _limitMax.transform.position.z || desired.z < _limitMin.transform.position.z)
                pos.z = 0f;
        }

        ee.transform.position += pos;

    }

    /// <summary>
    /// Function used to add the reward.
    /// It's called each step 
    /// </summary>
    private void CalculateRewards()
    {
        float reward;
        float maxDistance = Vector3.Distance(_limitMax.transform.position, _limitMin.transform.position);
        if (FlexActor.activeGrab == true)
        {    
            if (Vector3.Distance(_gettoppoint.transform.position, ee.transform.position) < 0.15f) {
                reward = 1;
                EndEpisode();
            } else {
                reward = ((Vector3.Distance(_gettoppoint.transform.position, ee.transform.position)) * -0.5f / maxDistance)-0.25f;
            }
        }
        else                    
            reward = (Vector3.Distance(_pegs[0].transform.position, ee.transform.position) * -0.5f/maxDistance) - 0.5f;
        
        //Debug.Log("maxdistance: "+maxDistance);
        // if (FlexActor.activeGrab == true)
        // {

        //     //reward = (Vector3.Distance(_gettoppoint.transform.position, ee.transform.position) * -0.5f / maxDistance);
        //     //if (Vector3.Distance(_gettoppoint.transform.position, ee.transform.position) > 0.51f)
        //         //reward = -0.7f;
        //     //else
        //     //{
        //     reward = (Vector3.Distance(_gettoppoint.transform.position, ee.transform.position) * -0.5f / maxDistance);
        //         //reward = 0.5f - Vector3.Distance(_gettoppoint.transform.position, ee.transform.position);
        //     //}
        //     //reward = (ee.transform.position.y - 0.515f) - 0.5f; ;
        //     //Debug.Log("reward: " + reward);
        // }
        // else
        // {
        //     reward = ((float)greenChecker.getGreenPixels())*1f/100;
        //     //reward = (Vector3.Distance(_pegs[0].transform.position, ee.transform.position) * -0.5f/maxDistance) - 0.5f;
        //     //Debug.Log("reward: " + reward);
        //     //reward =  (Vector3.Distance(ee.transform.position, _rings[0].transform.position)*-0.3956f)-.5f;
        // }
        Debug.Log("reward: "+reward);
        AddReward(reward);
        
    }


    private void PreventCollision(Vector3 position,Vector3 rotation)
    {
        if (shadow.MoveTo(position, rotation))
        {
            SetAgentPosition(_bbeePosition, _bbeeRotation); //move ..
            _bbeeRotation = _beeRotation;
            _bbeePosition = _beePosition;
            _beePosition = position; //..and then save the possible next one move
            _beeRotation = rotation;
        }
        else
        {
            _bbeeRotation = _beePosition = _beeRotation = _bbeePosition = new Vector3(0f, 0f, 0f);
            SetAgentPosition(_bbeePosition, _bbeeRotation); //move
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetKey(KeyCode.D) ? 1.0f : 0.0f;
        actionsOut[1] = Input.GetKey(KeyCode.S) ? 1.0f : 0.0f;
        actionsOut[2] = Input.GetKey(KeyCode.A) ? 1.0f : 0.0f;
        actionsOut[3] = Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;
        actionsOut[4] = Input.GetKey(KeyCode.E) ? 1.0f : 0.0f;
        actionsOut[5] = Input.GetKey(KeyCode.Q) ? 1.0f : 0.0f;
        
        // if (Input.GetKey(KeyCode.D))
        // {
            
        //     actionsOut[0] = {-1, 0, 0};
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     return new float[] {1, 0, 0};
        // }

        // if (Input.GetKey(KeyCode.S))
        // {
        //     return new float[] {0, -1, 0};
        // }
        // if (Input.GetKey(KeyCode.W))
        // {
        //     return new float[] {0, 1, 0};
        // }

        // if (Input.GetKey(KeyCode.E))
        // {
        //     return new float[] {0, 0, -1};
        // }
        // if (Input.GetKey(KeyCode.Q))
        // {
        //     return new float[] {0, 0, 1};
        // }
        //return new float[] { 0,0,0 };
    }
}
