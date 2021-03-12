using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DVRK;

public class IK : MonoBehaviour
{

    // lista dei vari giunti
    public List<URDFJoint> independentJoints = new List<URDFJoint>();
    public URDFJoint jaw = null;

    // oggetto verso il quale DVRK si muove
    public GameObject target;

    // posizione del target
    private Vector3 tarPos;

    // !!! MODFICARE PER CAMBIARE DIREZIONE !!!
    // direzione dell'end effector 
    [SerializeField]
    private Vector3 direction = new Vector3(1, 0, 1);
    // !!! MODFICARE PER CAMBIARE DIREZIONE !!!



    /// <summary>
    /// Peremette di inizializzare i giunti del robot e settarli alla loro
    /// posizione predefinita
    ///
    /// Inoltre viene normalizzato il vettore che indica la direzione
    /// dell'end effector
    /// </summary>
    void Start()
    {
        float red = .155f / Vector3.Magnitude(direction);
        direction = Vector3.Scale(direction, new Vector3(red, red, red));
        // all the joints, to setup linkage
        foreach (URDFJoint joint in GetComponentsInChildren<URDFJoint>())
        {
            joint.SetupRobotJoint();
        }
        foreach (URDFJoint joint in independentJoints)
        {
            joint.SetJointValueDefault();
        }
        if (jaw != null)
        {
            jaw.SetJointValueDefault();
        }
        tarPos = target.transform.position;

        Debug.Log("Here's to you, Nicola and Bart");

    }

    /// <summary>
    /// Ad ogni step viene hiamata la funzione per muovere il robot 
    /// e viene poi aggiornata la posizione del target
    ///
    /// Si stampa inoltre se il punto è raggiungibile dal braccio tale calcolo 
    /// non tiene conto di una eventuale orientazione sbagliata della pinza
    /// </summary>
    void Update()
    {
        moveTo(tarPos, direction);
        tarPos = target.transform.position;
        //log();
        Debug.Log(ValidSpawnTarget(tarPos));
    }

    /// <summary>
    /// setta i giunti del robot
    /// </summary>
    private void moveTo(Vector3 tarPos, Vector3 unit_vector)
    {
        // calcolo il punto del polso del di dvrk
        Vector3 tarPos_joint = tarPos - new Vector3(Mathf.Abs(unit_vector.x), unit_vector.y, Mathf.Abs(unit_vector.z));

        // setto inclinazione, torsione ed estensione del braccio
        independentJoints[0].SetJointValue(thetaAngle(tarPos_joint));
        independentJoints[1].SetJointValue(omegaAngle(tarPos_joint));
        independentJoints[2].SetJointValue(extendValue(tarPos_joint));

        independentJoints[3].SetJointValue(alfaAngle(tarPos_joint, unit_vector));//rotazione braccio
        independentJoints[4].SetJointValue(trustMeAngle(tarPos_joint, unit_vector));// inclinazione pinza
    }


    private float thetaAngle(Vector3 target)
    {
        return -Mathf.Atan(target.x / target.y) / (Mathf.PI * 2) * 360;
    }

    private float omegaAngle(Vector3 target)
    {
        return Mathf.Acos(target.z / Vector3.Distance(new Vector3(0, 0, 0), target)) / (Mathf.PI * 2) * 360 - 90;
    }

    private float alfaAngle(Vector3 tarPos_joint, Vector3 unit_vector)
    {
        float angle_adjust;

        float angle_offset = Mathf.Atan2(unit_vector.x, unit_vector.z) * 360 / (Mathf.PI * 2);
        if (tarPos_joint.z > 0)
        {
            angle_adjust = Vector3.Angle(Vector3.forward, new Vector3(tarPos_joint.x, 0, tarPos_joint.z));
            if (tarPos_joint.x < 0) angle_adjust *= -1;
        }
        else
        {
            angle_adjust = Vector3.Angle(Vector3.back, new Vector3(tarPos_joint.x, 0, tarPos_joint.z)) % 90;
            if (tarPos_joint.x > 0) angle_adjust *= -1;
        }
        Debug.Log(angle_adjust + "   " + angle_offset);
        return angle_adjust - (angle_adjust - angle_offset);

    }

    private float trustMeAngle(Vector3 tarPos_joint, Vector3 unit_vector)
    {
        float angle_adjust;
        angle_adjust = Vector3.Angle(unit_vector, tarPos_joint);
        //Debug.Log(angle_adjust+"   "+tarPos_joint);
        if (tarPos_joint.z > 0)
            return -angle_adjust;
        else
            return -angle_adjust;
    }

    private float extendValue(Vector3 target)
    {
        return (0.24f * (Vector3.Distance(new Vector3(0, 0, 0), target) + 0.155f)) / 2.4f;
    }

    private void log()
    {
        string toPrint = "[";
        int i = 0;
        foreach (URDFJoint joint in independentJoints)
        {
            toPrint += "Joint" + i + " : " + joint.currentJointValue + " , ";
            i++;
        }
        Debug.Log(toPrint + "]");
        return;
    }

    private bool ValidSpawnTarget(Vector3 pos)
    {
        return (pos.y <= 0
            && (Mathf.Sqrt(Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2) + Mathf.Pow(pos.z, 2)) < 2.5f)
            && (Mathf.Sqrt(Mathf.Pow(pos.x, 2) + Mathf.Pow(pos.y, 2)) > Mathf.Abs(pos.z))
        );
    }
}