using System;
using UnityEngine;
using Unity.MLAgents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.PlayerLoop;
using Random = System.Random;
public class CompleteTaskLogic : MonoBehaviour//Decision
{
    /*
    /// Array of actions to return 
    protected float[] Actions = new float[5];
    /// Have i to initialize the variables?  
    protected bool FirstTime = true;
    /// Array of rings 
    protected AttachToroid_Trigger[] Rings;
    /// Array of pegs 
    protected GameObject[] Pegs;
    /// Academy 
    protected DvrkAcademy DvrkAcademy;
    /// Position where i want to move my gripper 
    protected Vector3 DesiredPosition;

    protected Vector3 DesiredRotation;
    /// angle at wich grip the ring
    protected float Angle;
    /// Variable for smooth the movement 
    protected float DeltaMovement;
    /// Variable for smooth the movement 
    protected float DeltaRotation;
    /// Possible actions that i can do 
    protected PossibleActions ActionToDo;
    /// Counter used when it's open the gripper 
    protected int Counter;
    /// Number of ring to catch 
    protected int I;
    /// All true means that the task is completed 
    protected int[] RingComplete;
    /// The End effector to be moved
    protected GameObject Ee;
    
    /// The y values of the top of the pegs
    /// Steps where the agent has to stay an do nothings 
    protected readonly int MaxCounter = 60;

    protected float Rnx;
    protected float Rnz;

    private Vector3 offset;
    
    /// Possible actions 
    protected enum PossibleActions
    {
        Slideuptoring,
        Slideuptopeg,
        Godown,
        Goup,
        Opengripper,
        Closegripper,
        Release,
        Done
    }


    protected void Start()
    {
        Ee = GameObject.Find(DvrkAcademy.GetPreventCollision() ? "ShadowpointEE2" : "pointEE2");
    }

    public override float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        // Initialized variables
        Begin(done);

        //ImagePosition of the endefector
        Vector3 currentPosition = Ee.transform.position;
        
        ChoosePosition();
        for (int i = 0; i <= 2; i++)
        {
            var delta = DesiredPosition[i] - currentPosition[i];
            if (delta > DeltaMovement/3)
                Actions[i] = 1f;
            else if (delta < -DeltaMovement / 3)
                Actions[i] = 2f;
            else
                Actions[i] = 0f;
            }
            */
        /*
        Vector3 current_direction = Ee.transform.rotation.eulerAngles;
        
        var tmp = DesiredRotation.y - current_direction.y;
        tmp=(Mathf.Clamp(tmp <- 180 ? tmp+360 :tmp > 180 ?tmp-360: tmp , -DeltaRotation, DeltaRotation)/DeltaRotation);
        if (tmp > DeltaMovement/3)
            Actions[3] = 1f;
        else if (tmp < -DeltaMovement / 3)
            Actions[3] = 2f;
        else
            Actions[3] = 0f;*/
        /*
        return Actions;
    }
        
        
        public override List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
        {
            return new List<float>();
        }




        protected bool IsTookByMe()
        {
            return Rings[I].TookByPSM2();
        }

        protected void UpdateTaskStatus()
        {
            
                Vector3 ringPos = Rings[0].transform.position;

                if (Vector3.Distance(ringPos, Pegs[0].transform.position) > .089f || ringPos.y > .02f)
                    RingComplete[0] = 0;
            
        }

        protected void ChooseRing()
        {
            I = -0;
        }

        protected bool PositionValid(Vector3 pos)
        {
            Vector3 center = new Vector3(0,0,0);
            float radius = .44f;
            return(Vector3.Distance(pos, center) < radius);
        }
        
        protected Vector3 PickUpPoint()
        {
            Vector3 toReturn;
            float c = Rings[I].transform.eulerAngles.x;
            double x = -Mathf.Cos(Angle/180*Mathf.PI)*.066f;
            double z = -Mathf.Sin(Angle/180*Mathf.PI)*.066f;
            
            toReturn.x = Convert.ToSingle(x);
            toReturn.y = Rings[I].transform.position.y-.007f;
            toReturn.z = Convert.ToSingle(z);

            return new Vector3(0,0,0);
        }


        public void SetAngle()
        {
            Random rn= new Random();
            do
            {
                Angle = 180;// rn.Next(0, 359);
                offset = PickUpPoint();
                DesiredPosition=offset + Rings[I].transform.position;
            } while (!PositionValid(DesiredPosition));
            Rnx = 90 - rn.Next(0, 20);
            Rnz = 360  - rn.Next(0, 20);
            DesiredRotation=new Vector3(Rnx,-Angle,Rnz);
            Debug.Log("Set Angle");
        }

        protected void Begin(bool done)
        {
            if (FirstTime)
            {
                DvrkAcademy = FindObjectOfType<DvrkAcademy>();
                Rings = DvrkAcademy.GetRings();
                Pegs = DvrkAcademy.GetPegs();
                DeltaMovement = DvrkAcademy.deltaMovement;
                DeltaRotation = DvrkAcademy.deltaRotation;
                Start();
            }

            if (!done && !FirstTime) return;
            ActionToDo = PossibleActions.Slideuptoring;
            Counter = 0;
            RingComplete = new [] {0,0,0,0};
            ChooseRing();
            SetAngle();
            FirstTime = false;

        }
    

        private void ChoosePosition()
        {
            Vector3 currentPosition = Ee.transform.position;    
            switch (ActionToDo)
            {
                case PossibleActions.Done:
                    if (Counter == MaxCounter)
                    {
                        UpdateTaskStatus();
                        if (RingComplete.Sum() != 4)
                        {
                            ChooseRing();
                            ActionToDo = PossibleActions.Slideuptoring;
                            Counter = 0;
                        }
                    }
                    else
                    {
                        Counter++; 
                    }
                    break;
                
                // Slide to the top of peg that has the ring _i
                case PossibleActions.Slideuptoring:
                    DesiredPosition=PickUpPoint()+ Rings[I].transform.position;
                    DesiredPosition.y = .14f;
                    if (Vector3.Distance( currentPosition,DesiredPosition) < .009f)
                                        
                        if (Counter < MaxCounter)
                            Counter += 1;
                        else
                        {
                            ActionToDo = PossibleActions.Opengripper;
                            Counter = 0;
                        }
                        
                    break;
                
                
                
                case PossibleActions.Opengripper:
                    Actions[3] = 1f;
                    ActionToDo = PossibleActions.Godown;
                    break;*/
                /*
                 * Open Gripper , this action changes according to the boolean variable have ring
                 * - If have ring is true the agent has to open the gripper to release the ring
                 * - If have ring is false the agent has to open the gripper and goes down to take the ring
                 * It used a counter to block the agent position for some steps 
                 */
                /*
                case PossibleActions.Release:
                    DesiredPosition = currentPosition;
                
                    if (Counter < MaxCounter)
                        Counter += 1;
                    else
                    {
                        Actions[3] = 1f;
                        RingComplete[I] = 1;
                        if (RingComplete.Sum() == 4)
                        {
                            ActionToDo = PossibleActions.Done;
                            Counter = 0;
                        }
                        else
                        {
                            if (Counter == MaxCounter * 2)
                            {
                                ChooseRing();
                                SetAngle();
                                ActionToDo = PossibleActions.Slideuptoring;
                                Counter = 0;
                            }
                            else
                            {
                                Counter++;
                            }
                        }
                    }

                    break;
                
                // Go down to take the ring inside his peg 
                case PossibleActions.Godown :
                    DesiredPosition=PickUpPoint()+ Rings[I].transform.position;
                    if (Vector3.Distance( currentPosition,DesiredPosition) < .009f)
                        ActionToDo = PossibleActions.Closegripper;
                    break;
               */
                /**
                 * Close gripper to take the ring.
                 * It used a counter to block the agent position for some steps 
                 */ 
                /*
                case PossibleActions.Closegripper:
                    Actions[3] = 0f;

                    if (Counter < MaxCounter)
                        Counter += 1;
                    else
                    {
                        ActionToDo = PossibleActions.Goup;
                        Counter = 0;
                    }
                    break;
                
                // Go up on top of the peg with an y = peg.y * 2
                case PossibleActions.Goup:
                    DesiredPosition = new Vector3(currentPosition.x,.14f , currentPosition.z);

                    if (Vector3.Distance( currentPosition,DesiredPosition) < .009f)
                        if (IsTookByMe())
                        {
                            ActionToDo = PossibleActions.Slideuptopeg;
                        }
                        else
                        {
                            ActionToDo = PossibleActions.Opengripper;
                        }
                    break;
                
                // The agent has got the ring and has to bring it to the correct peg
                case PossibleActions.Slideuptopeg:
                    UpdateTaskStatus();
                    var posEe = Ee.transform.position;
                    var posRi = Rings[I].transform.position;
                    var posPe = Pegs[I].transform.position;
                    DesiredPosition = new Vector3(posPe.x, .14f, posPe.z )+offset;
                    
                    if( Vector3.Distance(currentPosition ,DesiredPosition)< .005f)
                        ActionToDo = PossibleActions.Release;
                    Counter = 0;
                    break;
                    
            }
        }
*/
                
}

