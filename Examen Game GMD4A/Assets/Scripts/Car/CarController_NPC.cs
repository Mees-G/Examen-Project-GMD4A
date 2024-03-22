using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class CarController_NPC : Controller_Base
{
    public float detectionDistance;
    [Range(0, 100)] public float agressiveness;
    public float respawnTime;

    [Header("NPC Driving Values")]
    public float brakeDistance = 20f;

    float smoothVelocity = 0.1f;
    [HideInInspector] public bool NpcActivated;
    bool canMakeTurn;
    bool isFlipped;

    [Header("NPC driving variables")]
    public float angleThreshold = 20f;
    public float angleBuffer = 2f; // Introduce a buffer to avoid rapid switching

    public LevelType gameMode;

    void Awake()
    {
        NPC = true;
    }

    public override void SwitchControl(bool activate)
    {

        if (gameMode == LevelType.RACER)
        {
            if (activate)
            {
                driveTarget = checkpointToReach.position;
            }
            NpcActivated = activate;
        }

    }

    public override void FixedUpdate()
    {

        // Debug.Log("Fixed updating " + controlType);
        switch (gameMode)
        {
            case LevelType.RACER:


                break;

            case LevelType.CHASER:
                //Debug.Log("chase - " + NpcActivated);
                if (!NpcActivated)
                {
                    Debug.Log(Vector3.Distance(car.transform.position, CarController_Player.instance.car.transform.position));
                    if (NpcActivated = (Vector3.Distance(car.transform.position, CarController_Player.instance.car.transform.position) < 40))
                    {
                        Debug.Log("Actuvardd!");
                        this.NextCheckpoint(track.checkpoints[track.checkpoints.IndexOf(CarController_Player.instance.checkpointToReach)]);

                        car.handBrake = false;
                    }
                }

                break;
        }

        base.FixedUpdate();
        Detection();
        Driving();
    }

    //public override void NextCheckpoint(Transform checkpoint)

    //{
    //    this.checkpointToReach = checkpoint;
    //    driveTarget = checkpointToReach.position;

    //    if (checkpoint == checkpointToReach)
    //    {
    //        if (checkpointToReach != track.checkpoints.Last())
    //        {
    //            if (track.checkpoints.IndexOf(checkpoint) == -1)
    //            {
    //                Debug.Log("jatoch! wtkk ouwe - " + checkpoint.gameObject + " - " + checkpoint.gameObject.transform.parent.name);
    //            }
    //            checkpointToReach = track.checkpoints[track.checkpoints.IndexOf(checkpoint) + 1];

    //            if (NPC)
    //            {
    //                driveTarget = checkpointToReach.position;
    //            }
    //        }
    //    }


    //}

    void Detection()
    {

    }
    void Driving()
    {
        if (!NpcActivated)
            return;

     //   Debug.Log("ay shit driving biiihhh");
        // Calculate the angle between the car and the target position
        Vector3 targetDirection = (driveTarget - car.transform.position).normalized;
        Vector3 forward = car.transform.forward.normalized;
        float angle = Mathf.DeltaAngle(Vector3.SignedAngle(targetDirection, forward, Vector3.up), 0);

        // Compute the steering wheel input
        float targetSteeringInput = angle / car.currentSteerRange;

        // Clamp the steering input to ensure it's within the range of -1 to 1
        targetSteeringInput = Mathf.Clamp(targetSteeringInput, -1f, 1f);

        // Throttle Control
        float targetThrottle = 1f;

        // Calculate the angle between the car, the current checkpoint to reach, and the next checkpoint
        Vector3 nextCheckpoint;

       // Debug.Log(track.checkpoints.IndexOf(checkpointToReach) + " da index - " + track.checkpoints.Count + " - " + driveTarget);
        if (checkpointToReach == track.checkpoints.Last())
        {
            nextCheckpoint = track.checkpoints.First().position;
        }
        else
        {
            nextCheckpoint = track.checkpoints[track.checkpoints.IndexOf(checkpointToReach) + 1].position;
        }

        if (gameMode == LevelType.CHASER && checkpointToReach == CarController_Player.instance.checkpointToReach)
        {
            driveTarget = CarController_Player.instance.car.transform.position;
        }

        float cornerRadius = CalculateCarPhysics.CornerRadius(car.transform, checkpointToReach, nextCheckpoint, car.wheelBase);

        float centripetalForce = (car.rb.mass * (car.forwardSpeed * car.forwardSpeed / 4)) / cornerRadius;
        float frictionForce = car.frontWheels[0].forwardFriction.stiffness * CalculateCarPhysics.CalculateNormalForce(car.rb.mass);

        // Calculate if the car can make the turn without overshooting
        bool canMakeTurn = centripetalForce <= frictionForce && Mathf.Abs(targetSteeringInput) <= 1f;

        // Overshoot Prevention
        if (!canMakeTurn)
        {
            // The car might not be able to make the turn at the current speed
            targetThrottle = 0;
        }

        // Introduce hysteresis to prevent rapid switching
        car.throttleInput = Mathf.Clamp(Mathf.MoveTowards(car.throttleInput, targetThrottle, Time.deltaTime * 1.5f), -1f, 1f);
        car.steeringDirectionInput = Mathf.Clamp(Mathf.SmoothDamp(car.steeringDirectionInput, targetSteeringInput, ref smoothVelocity, 0.1f), -1f, 1f);

        //Debug.Log($"Corner Radius: {cornerRadius}, Throttle: {car.throttleInput}, Can Make Turn: {canMakeTurn}");

        if (isFlipped)
            return;

        // Despawn Detection
        if (gameMode == LevelType.RACER && car.throttleInput >= 0.9f && car.forwardSpeed < 0f)
        {
            isFlipped = true;
            StartCoroutine(FlipDetection());
        }
    }

    IEnumerator FlipDetection()
    {
        yield return new WaitForSeconds(3);
        if (car.throttleInput >= 0.9f && car.forwardSpeed < 0f)
        {
            //NpcActivated = false;
            RespawnCar();
        }
        else
        {
            isFlipped = false;
            //   NpcActivated = true;
        }
    }

    public override void RespawnCar()
    {
        base.RespawnCar();
        isFlipped = false;
        //NpcActivated = true;
    }
    private void OnDrawGizmos()
    {
        if (car == null)
            return;

        if (canMakeTurn)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawSphere(new Vector3(car.transform.position.x, car.transform.position.y + 4, car.transform.position.z), 0.5f);

    }
}
