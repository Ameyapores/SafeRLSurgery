using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to change the color of gripper when it can catch the ring
/// </summary>
public class GripperLight : MonoBehaviour
{
    /// Link to gripper left
    [SerializeField]
    private GameObject grasper4;
    /// Link to gripper Right
    [SerializeField]
    private GameObject grasper5;
    /// Save rander for optimization
    private Renderer _rend4;
    /// Save rander for optimization
    private Renderer _rend5;
    /// Backup start material
    private Material _bckMaterial;
    /// Color to assign to the gripper
    [SerializeField]
    
    Material colorTriggerEnter;

    void Start()
    {
        //save the component
        _rend4 = grasper4.GetComponent<Renderer>();
        _rend5 = grasper5.GetComponent<Renderer>();
        //save backup material
        _bckMaterial = _rend4.material;
    }

    private void OnTriggerStay(Collider other)
    {
        //If collide with a toroid then change the color 
        if (other.gameObject.CompareTag("Tor1") || other.gameObject.CompareTag("Tor2") || other.gameObject.CompareTag("Tor3") || other.gameObject.CompareTag("Tor4")) 
        {
            _rend4.material = colorTriggerEnter;
            _rend5.material = colorTriggerEnter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            _rend4.material = _bckMaterial;
            _rend5.material = _bckMaterial;
    }

}
