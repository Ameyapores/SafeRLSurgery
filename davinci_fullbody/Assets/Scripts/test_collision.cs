using System;
using Unity.MLAgents;
using UnityEngine;

public class test_collision : MonoBehaviour
{
    /// Academy's agent 
    // [SerializeField] private DvrkAcademy dvrkAcademy;

    
    // /// Max limit for pegs and rings plane 
    // private GameObject _limitMax;
    // /// Min limit for pegs and rings plane 
    // private GameObject _limitMin;
    
    // [SerializeField] private GameObject _shadowEE;
    // [SerializeField] private GameObject _realEE;
    // private int hit;
    // // Start is called before the first frame update
    // public void Start()
    // {
    //     hit = 0;

    //      _shadowEE.transform.position = _realEE.transform.position;
    //     _shadowEE.transform.eulerAngles= _realEE.transform.eulerAngles;
    //     _limitMax = dvrkAcademy.GetLiminitMax();
    //     _limitMin = dvrkAcademy.GetLiminitMin();
        
    // }

    // public bool MoveTo(Vector3 position,Vector3 rotation)
    // {
    //     if (hit != 0)
    //     {
    //         _shadowEE.transform.position = _realEE.transform.position;
    //         _shadowEE.transform.eulerAngles = _realEE.transform.eulerAngles;

    //         Vector3 desired = _shadowEE.transform.position + position;
    //         if (_shadowEE.name == "ShadowpointEE2")
    //         {
    //             if (desired.x > _limitMax.transform.position.x || desired.x < _limitMin.transform.position.x)
    //                 position.x = 0;
    //         }
    //         else
    //         {
    //             if (desired.x > _limitMax.transform.position.x || desired.x < _limitMin.transform.position.x)
    //                 position.x = 0;
    //         }

    //         if (desired.y > _limitMax.transform.position.y || desired.y < _limitMin.transform.position.y)
    //             position.y = 0;
    //         if (desired.z > _limitMax.transform.position.z || desired.z < _limitMin.transform.position.z)
    //             position.z = 0;
    //         _shadowEE.transform.position +=position;
    //         setRotation(rotation);
    //         return false;
    //     }
    //     else
    //     {
    //         _shadowEE.transform.rotation = _realEE.transform.rotation;
    //         Vector3 desired = _shadowEE.transform.position + position;
    //         if (_shadowEE.name == "ShadowpointEE2")
    //         {
    //             if (desired.x > _limitMax.transform.position.x || desired.x < _limitMin.transform.position.x)
    //                 position.x = 0;
    //         }
    //         else
    //         {
    //             if (desired.x > _limitMax.transform.position.x || desired.x < _limitMin.transform.position.x)
    //                 position.x = 0;
    //         }

    //         if (desired.y > _limitMax.transform.position.y || desired.y < _limitMin.transform.position.y)
    //             position.y = 0;
    //         if (desired.z > _limitMax.transform.position.z || desired.z < _limitMin.transform.position.z)
    //             position.z = 0;
            
    //         //setRotation(rotation);
    //         _shadowEE.transform.position += position;

    //         var tmp = _shadowEE.transform.eulerAngles.x + rotation.x;
    //         rotation.x=tmp > 360 ? tmp - 360 : tmp;
    //         tmp = _shadowEE.transform.eulerAngles.y + rotation.y;
    //         rotation.y=tmp > 360 ? tmp - 360 : tmp;
    //         tmp = _shadowEE.transform.eulerAngles.z + rotation.z;
    //         rotation.z = tmp > 360 ? tmp - 360 : tmp;
    //         _shadowEE.transform.eulerAngles = rotation;
            
            
            
            
 
    //         return true;
    //     }

        
        
    // }

    // // Update is called once per frame
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.tag.Equals("Pole"))
    //     {
    //         hit++;
    //         Debug.Log("hit");
    //     }
        
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.tag.Equals("Pole"))
    //     {
    //         hit=hit>=1?hit-1:0;
    //     }
    // }


    // private void setRotation(Vector3 rotation)
    // {
    //     var tmp = _shadowEE.transform.transform.eulerAngles.x + rotation.x;
    //     rotation.x=tmp > 360 ? tmp - 360 : tmp;
    //     tmp = _shadowEE.transform.transform.eulerAngles.y + rotation.y;
    //     rotation.y=tmp > 360 ? tmp - 360 : tmp;
    //     tmp = _shadowEE.transform.transform.eulerAngles.z + rotation.z;
    //     rotation.z = tmp > 360 ? tmp - 360 : tmp;
    //     _shadowEE.transform.transform.eulerAngles = rotation;


    // }


}