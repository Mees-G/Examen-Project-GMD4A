using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Splines;

public class CarController_NPC : Controller
{
    public Car car;
    public float detectionDistance;
    [Range(0, 100)] public float agressiveness;

    public Track track;
    public Vector3 driveTarget;
    public Transform checkpointToReach;

    [Header("NPC Driving Values")]
    public float brakeDistance = 20f;
    void Start()
    {
        car.currentCarController = this;

        StartNPC();
    }

    void StartNPC()
    {
        checkpointToReach = track.checkpoints[0];
        driveTarget = checkpointToReach.position;
    }

    void FixedUpdate()
    {
        Detection();
        Driving();
    }

    void Detection()
    {
        
    }

    void Driving()
    {
        //Calculate the angle between the car and the target position
        Vector3 targetDirection = (driveTarget - car.transform.position).normalized;
        Vector3 forward = car.transform.forward.normalized;
        float angle = Mathf.DeltaAngle(Vector3.SignedAngle(targetDirection, forward, Vector3.up), 0);

        Debug.Log(angle);
        //Steer left or right depending on the angle
        if(angle < -10)
        {
            //steer left
            car.steeringDirectionInput = -1;
        }
        else if(angle > 10)
        {
            //steer right
            car.steeringDirectionInput = 1;
        }
        else
        {
            //keep straight
            car.steeringDirectionInput = 0;
        }

        //throttle Control
        car.throttleInput = 1f;

        //Decrease throttle when the angle to the target is above a certain angle
        if (angle < -25 || angle > 25)
        {
            car.throttleInput = 0.5f;
        }
        if (angle < -30 || angle > 30)
        {
            car.throttleInput = 0;
        }

        //prevent overshooting

        //Calculate the angle between the car, the currentcheckpointtoreach and the next checkpoint
        
        //If the angle is sharp, the car should brake well before it reaches the corner
    }

    public override void NextCheckpoint(Transform checkpoint)
    {
        if(checkpoint == checkpointToReach)
        {
            checkpointToReach = track.checkpoints[track.checkpoints.IndexOf(checkpoint) + 1];
            driveTarget = checkpointToReach.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(driveTarget, 1f);
    }
}
