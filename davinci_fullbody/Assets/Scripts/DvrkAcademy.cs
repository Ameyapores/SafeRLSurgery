using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;
using NVIDIA.Flex;
using Unity.MLAgents.Sensors;
public class DvrkAcademy : MonoBehaviour
{
    /// Array of pegs
    // [SerializeField] private GameObject[] _pegs;
    // /// Limits used to calculate when a ring falls off the platform
    // [SerializeField] private GameObject[] _fat;
    // /// Array of rings
    // [SerializeField] private GameObject _ee;
    
    // [SerializeField] private GameObject _limitmax,_limitmin, _toppoint;

    // /// Boolean variable to enable scene limits
    // [SerializeField] private bool enableSceneLimits;
    
    // [SerializeField] private bool preventCollision;
    
    // [SerializeField] private bool _createDataSet;

    // // Variables to write the csv file
    // private readonly string _filePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\DVRK_dataset";
    // private readonly char _delimiter = ';';
    // private StringBuilder _stringBuilder;
    // private bool _firstLine = true;

    // /// Variable used to smooth the movement
    // public float deltaMovement;
    // /// Variable used to smooth the rotation
    // public float deltaRotation;
    // /// List of observations
    // private List<float> _obs;

    // private int psm2Gripping; 


    // public void AcademyReset()
    // {
    //     // write data on csv
    //     // if (!_firstLine && _createDataSet )
    //     // {
    //     //     var path = _filePath  + @"\" + DateTime.Now.Ticks + ".csv";
    //     //     File.WriteAllText(path,_stringBuilder.ToString());
            
    //     //     _stringBuilder = new StringBuilder();
    //     //     _firstLine = true;
    //     // }
    //     Debug.Log("Agent Reset");

    // }
    
    // // public override void InitializeAcademy()
    // // {

    // //     CollectObservation();


    // //     // if (_createDataSet)
    // //     // {
    // //     //     _stringBuilder = new StringBuilder();
    // //     //     if (!Directory.Exists(_filePath))
    // //     //         Directory.CreateDirectory(_filePath);
    // //     // }
        
    // // }

    // void Awake() 
    // {
    //     //CollectObservation(sensor);
    //     Academy.Instance.OnEnvironmentReset += AcademyReset;
    // }

    // // public override void AcademyStep()
    // // {
    // //     // add to the observation the position and rotation of the rings
    // //     CollectObservation();

    // //     if (GetStepCount()%5==0 && _createDataSet)
    // //     {
    // //         WriteOnCsv();   
    // //     }
    // // }

    // // void FixedUpdate() 
    // // {
    // //     // add to the observation the position and rotation of the rings
    // //     CollectObservation();

    // //     // if (GetStepCount()%5==0 && _createDataSet)
    // //     // {
    // //     //     WriteOnCsv();   
    // //     // }
    // // }

    // // private void WriteOnCsv()
    // // {
        
    // //     var line = "";

    // //     if (_firstLine)
    // //     {

    // //             line = line + _ee.name + "position.x" + _delimiter + _ee.name + "position.y" + _delimiter +
    // //                    _ee.name + "position.z" + _delimiter + _ee.name + "rotation.x" + _delimiter
    // //                    + _ee.name + "rotation.y" + _delimiter + _ee.name + "rotation.z" + _delimiter + _ee.name + "rotation.w" + _delimiter;
                
    // //         _firstLine = false;
    // //     }
    // //     else
    // //     {
    // //         Vector3 position;
    // //         Quaternion rotation;
            
            
    // //             position = _ee.transform.position;
    // //             rotation = _ee.transform.rotation;
                
    // //             line = line + position.x + _delimiter + position.y + _delimiter +
    // //                    position.z + _delimiter + rotation.x + _delimiter
    // //                    + rotation.y + _delimiter + rotation.z + _delimiter + rotation.w + _delimiter;
                
    // //     }
        
    // //     _stringBuilder.AppendLine(line);
    // // }

    // public GameObject[] Getfat()
    // {
    //     return _fat;
    // }

    // public GameObject[] GetPegs()
    // {
    //     return _pegs;
    // }

    // public float[] GetObservation()
    // {

    //     return _obs.ToArray();
    // }

    

    // public GameObject GetLiminitMax()
    // {
    //     return _limitmax;
    // }
    
    // public GameObject GetLiminitMin()
    // {
    //     return _limitmin;
    // }

    // public GameObject Gettoppoint()
    // {
    //     return _toppoint;
    // }

    // public bool GetEnableSceneLimits()
    // {
    //     return enableSceneLimits;
    // }

    // public bool GetPreventCollision()
    // {
    //     return preventCollision;
    // }

    // // private float normalizedDistance(Vector3 cord1,Vector3 cord2)
    // // {
    // //     float maxDistance = Vector3.Distance(_limitmin.transform.position, _limitmax.transform.position);
    // //     return Vector3.Distance(cord1,cord2)/(maxDistance);
    // // }

    // // public void SetPsm2IsGripping(bool isGripping)
    // // {
    // //     if (isGripping)
    // //         psm2Gripping = 0;
    // //     else
    // //         psm2Gripping = 1;
    // // }

    // // private void CollectObservation(VectorSensor sensor)
    // // {
    // //    //_obs = new List<float>();
    // //     Vector3 position;
    // //     float maxFar = .5f;
    // //     position = _toppoint.transform.position;

    // //     // 0 if it is grasping, 1 otherwise
    // //     sensor.AddObservation(FlexActor.activeGrab ? 0 : 1);
        
    // //     sensor.AddObservation(_ee.transform.position.x);
    // //     sensor.AddObservation(_ee.transform.position.y);
    // //     sensor.AddObservation(_ee.transform.position.z);
        
    // //     if (FlexActor.activeGrab){
    // //         // if it has gripped the fat, we do not care about the old pegs position
    // //         sensor.AddObservation(position.x);
    // //         sensor.AddObservation(position.y);
    // //         sensor.AddObservation(position.z);
    // //         sensor.AddObservation(Vector3.Distance(position,_ee.transform.position));
    // //     } else {
    // //         sensor.AddObservation(_pegs[0].transform.position.x);
    // //         sensor.AddObservation(_pegs[0].transform.position.y);
    // //         sensor.AddObservation(_pegs[0].transform.position.z);
    // //         sensor.AddObservation(Vector3.Distance(_pegs[0].transform.position, _ee.transform.position));
    // //     }        
    // // }
}
