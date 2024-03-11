using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.Rendering.DebugUI.Table;

public class CarController_NPC : Controller_Base
{
    public float detectionDistance;
    [Range(0, 100)] public float agressiveness;
    public float respawnTime;

    [Header("NPC Driving Values")]
    public float brakeDistance = 20f;


    float smoothVelocity = 0.1f;
    bool NpcActivated;
    bool canMakeTurn;

    [Header("NPC driving variables")]
    public float angleThreshold = 20f;
    public float angleBuffer = 2f; // Introduce a buffer to avoid rapid switching

    void Start()
    {
        NPC = true;
    }

    public override void SwitchControl(bool activate)
    {
        if (activate) 
        {
            driveTarget = checkpointToReach.position;
        }
        NpcActivated = activate;
    }

    void FixedUpdate()
    {
        if (car.throttleInput >= 0.9f && car.forwardSpeed < 0f)
        {
            StartCoroutine(FlipDetection());
        }
        
        Detection();
        Driving();
    }

    IEnumerator FlipDetection()
    {
        yield return new WaitForSeconds(3);
        RespawnCar();
    }

    void Detection()
    {
        
    }
    void Driving()
    {
        if (!NpcActivated)
            return;

        // Calculate the angle between the car and the target position
        Vector3 targetDirection = (driveTarget - car.transform.position).normalized;
        Vector3 forward = car.transform.forward.normalized;
        float angle = Mathf.DeltaAngle(Vector3.SignedAngle(targetDirection, forward, Vector3.up), 0);

        float targetSteeringInput;

        // Steer left or right depending on the angle
        if (angle < -10)
        {
            // Steer left and keep the wheel at the right steeringanlge when turning out the corner

            targetSteeringInput = -1;
        }
        else if (angle > 10)
        {
            // Steer right
            targetSteeringInput = 1;
        }
        else
        {
            // Keep straight
            targetSteeringInput = 0;
        }

        // Throttle Control
        float targetThrottle = 1f;

        // Calculate the angle between the car, the current checkpoint to reach, and the next checkpoint
        float cornerRadius = CalculateCarPhysics.CornerRadius(car.transform, checkpointToReach, track.checkpoints[track.checkpoints.IndexOf(checkpointToReach) + 1], car.wheelBase);

        float centripetalForce = (car.rb.mass * car.forwardSpeed * (car.forwardSpeed / 2)) / cornerRadius;
        float frictionForce = car.frontWheels[0].forwardFriction.stiffness * CalculateCarPhysics.CalculateNormalForce(car.rb.mass);

        canMakeTurn = centripetalForce <= frictionForce;

        // Overshooting Prevention
        if (!canMakeTurn)
        {
            // The car might not be able to make the turn at the current speed
            targetThrottle = 0;
        }

        // Introduce hysteresis to prevent rapid switching
        car.throttleInput = Mathf.Clamp(Mathf.MoveTowards(car.throttleInput, targetThrottle, Time.deltaTime * 0.5f), -1f, 1f);
        car.steeringDirectionInput = Mathf.Clamp(Mathf.SmoothDamp(car.steeringDirectionInput, targetSteeringInput, ref smoothVelocity, 0.2f), -1f, 1f);

        //Debug.Log($"Corner Radius: {cornerRadius}, Throttle: {car.throttleInput}, Can Make Turn: {canMakeTurn}");
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
