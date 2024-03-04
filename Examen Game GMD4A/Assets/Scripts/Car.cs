using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Car : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Wheels and Input")]
    public WheelCollider[] frontWheels;
    public WheelCollider[] backWheels;

    public float throttle;
    public float steeringDirection;

    [Header("Engine")]
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;

    [Header("Car Audio")]
    public AudioSource engineAudio;
    public AudioClip engineIdle;
    private float minEnginePitch;

    private void Start()
    {
        minEnginePitch = engineAudio.pitch;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //calculate current speed in relation to the forward direction of the car
        float forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);

        //calculate how close the car is to top speed
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        //calculate how much torque is available 
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        
        //calculate how much to steer 
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check if the users input is in the same direction as the cars velocity
        bool isAccelerating = Mathf.Sign(throttle) == Mathf.Sign(forwardSpeed);

        foreach (WheelCollider wheel in frontWheels)
        {
            // Apply steering to Wheel colliders
            wheel.steerAngle = steeringDirection * currentSteerRange;
        }
        foreach (WheelCollider wheel in backWheels)
        {
            if (isAccelerating)
            {
                // Apply torque to Wheel colliders
                wheel.motorTorque = throttle * currentMotorTorque;
                wheel.brakeTorque = 0;
            }
            else
            {
                // apply brakes to all wheels when throttle is negative
                wheel.brakeTorque = Mathf.Abs(throttle) * brakeTorque;
                wheel.motorTorque = 0;
            }
        }

        //sound and pitch of the engine
        engineAudio.pitch = Mathf.Min(Mathf.Lerp(engineAudio.pitch, minEnginePitch + forwardSpeed, Time.deltaTime / 3), 2);
    }
}
