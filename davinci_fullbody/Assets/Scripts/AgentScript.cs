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
    [FormerlySerializedAs("startPosRot")] [SerializeField] private GameObject ee;
    [SerializeField] private URDFJoint leftGripper;
    [SerializeField] private URDFJoint rightGripper; 
    private Vector3 _resetPosition;
    private Quaternion _resetRotation;
    public float _deltaMovement;
    private GameObject _limitMax;
    private GameObject _limitMin;
    private GameObject _gettoppoint;
    private GameObject _pegs;
    private GameObject _fat;
    private bool[] _alreadyCompleted;
    private int step;
    private Vector3 fat_reset_position;
    private Quaternion fat_reset_rotation;
    private Vector3 ee_reset_offset;


    int pos_x = 0;
    int pos_y = 0;
    int pos_z = 0;

    void Start() 
    {
        _alreadyCompleted = new bool[1];
        _resetPosition = ee.transform.position;
        _resetRotation = ee.transform.rotation;
        
        _limitMax = GameObject.Find("Maximum");
        _limitMin = GameObject.Find("Minimum");
        _gettoppoint = GameObject.Find("toppoint1");
        _pegs = GameObject.Find("Lesion");
        _fat = GameObject.Find("Fat");
        step = 0;
        fat_reset_position = _fat.transform.position;
        fat_reset_rotation = _fat.transform.rotation;
        greenChecker = GameObject.Find("securitycamera").GetComponent<greenpixels>();
        // RandomSpawn();
        
    }

    
    // private void RandomSpawn()
    // {
    //     Random random = new Random();  
    //     float x = random.Next(-50 , 50)/100f;
    //     float z = random.Next(-50, 50)/100f;
    //     ee_reset_offset=new Vector3(x,0,z);

    //     //egs_reset_offset=new Vector3(0,0,0);
    //     // Debug.Log("random");
    // }

    public override void CollectObservations(VectorSensor sensor)
    {
        
        // 0 if it is grasping, 1 otherwise
        sensor.AddObservation(FlexActor.activeGrab ? 0 : 1);
        sensor.AddObservation(ee.transform.position);
        // Debug.Log("adadasdsadsasa");  
        if (FlexActor.activeGrab){
            // if it has gripped the fat, we do not care about the old pegs position
            sensor.AddObservation(_gettoppoint.transform.position);
            // sensor.AddObservation(position.y);
            // sensor.AddObservation(position.z);
            sensor.AddObservation(Vector3.Distance(_gettoppoint.transform.position,ee.transform.position));
        } else {
            sensor.AddObservation(_pegs.transform.position);
            // sensor.AddObservation(_pegs[0].transform.position.y);
            // sensor.AddObservation(_pegs[0].transform.position.z);
            sensor.AddObservation(Vector3.Distance(_pegs.transform.position, ee.transform.position));
        }      
        
    }
    
    public override void OnActionReceived(float[] action)
    {
        // Debug.Log("action"+ action[0]);
        step++;
        int Pos_y = Mathf.FloorToInt(action[0]);
        int Pos_z = Mathf.FloorToInt(action[1]);
        int Pos_x = Mathf.FloorToInt(action[2]);

        if (Pos_y == 0) {  pos_y = -1; }
        if (Pos_y == 1) {  pos_y = 0; }
        if (Pos_y == 2) {  pos_y = 1; }

        if (Pos_z == 0) {  pos_z = -1; }
        if (Pos_z == 1) {  pos_z = 0; }
        if (Pos_z == 2) {  pos_z = 1; }

        if (Pos_x == 0) {  pos_x = -1; }
        if (Pos_x == 1) {  pos_x = 0; }
        if (Pos_x == 2) {  pos_x = 1; }
        CalculateRewards();
        // for(int i = 0; i<3;i++) 
        //     action[i] = action[i] == 2 ? -1 : action[i];
        // pos_x = 1;
        // Pos_y = -1;

        Vector3 position = new Vector3(pos_x*_deltaMovement,pos_y*_deltaMovement,pos_z*_deltaMovement);
        Vector3 rotation =new Vector3(0,0,0);
        
        SetAgentPosition(position, rotation);

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

        if (FlexActor.activeGrab == true)
            return (Vector3.Distance(_gettoppoint.transform.position, ee.transform.position) < 0.2);
        
        return false;
    }

    // public Vector3[] GridOfPoints(float startX, float endX, float y, float startZ, float endZ)
    // {
    //     float rangeX = endX - startX;
    //     float rangeZ = endZ - startZ;
    //     float helpX = startX;
    //     float helpZ = startZ;
    //     float stepX = rangeX / 9;
    //     float stepZ = rangeZ / 9;
    //     int cnt = 0;

    //     Vector3[] grid = new Vector3[100];

    //     for (float x = startX; x <= endX; x += stepX)
    //     {
    //         for (float z = startZ; z <= endZ; z += stepZ)
    //         {
    //             //Debug.Log(cnt);
    //             grid[cnt] = new Vector3(x, y, z);
    //             cnt++;
    //         }
    //     }
    //     return grid;
    // }

    // Vector3[] grid = new Vector3[100];
    // bool sem = true;
    // int count = 0;
    public override void OnEpisodeBegin()
    {
        // if (sem)
        // {
        //     grid = GridOfPoints(-0.42f,0.378f, 0f,0.046f,0.841f);
        //     sem = false;
        // }
        // RandomSpawn();
        if (step == 0)
            FlexActor.relGrasp = false;
        else
            FlexActor.relGrasp = true;
                
        FlexActor.resFatPos = true;
        // ee.transform.position = _resetPosition + grid[count];
        ee.transform.position = _resetPosition + ee_reset_offset ;
        _alreadyCompleted = new bool[1];
    }

    private void SetAgentPosition(Vector3 pos, Vector3 rotation)
    {
        Vector3 desired = ee.transform.position + pos;
        if (desired.x > _limitMax.transform.position.x || desired.x < _limitMin.transform.position.x)
            pos.x = 0f;
        if (desired.y > _limitMax.transform.position.y || desired.y < _limitMin.transform.position.y)
            pos.y = 0f;
        if (desired.z > _limitMax.transform.position.z || desired.z < _limitMin.transform.position.z)
            pos.z = 0f;

        ee.transform.position += pos;

    }

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
            reward = (Vector3.Distance(_pegs.transform.position, ee.transform.position) * -0.5f/maxDistance) - 0.5f;
        
        // Debug.Log("reward: "+reward);
        AddReward(reward);
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 1;
        actionsOut[1] = 1;
        actionsOut[2] = 1;
        if (Input.GetKey(KeyCode.W))
        {
        actionsOut[0] = -1;
        // Debug.Log("dhaflafas");
        }
        if (Input.GetKey(KeyCode.S))
        {
        actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
        actionsOut[1] = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
        actionsOut[1] = 1;
        }
    }
}
