using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Controller_Base currentCarController;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public CinemachineFreeLook cameraControl;

    public Transform collisionComponent;

    [Header("Wheels and Input")]
    public WheelCollider[] frontWheels;
    public WheelCollider[] backWheels;

    public float throttleInput;
    public float steeringDirectionInput;
    public bool handBrake;

    public GameObject lights;
    public float health;
    public float maxLoad;
    [HideInInspector] public float currentLoad;

    [Header("Engine")]
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float topSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;

    [Header("Car Audio")]
    public AudioSource engineAudio;
    public AudioClip engineIdle;

    private float minEnginePitch;

    [HideInInspector] public float forwardSpeed;
    [HideInInspector] public float currentSteerRange;
    [HideInInspector] public float wheelBase;

    public GameObject nameTag;
    public TMP_Text nameTagText;
    private void Start()
    {
        cameraControl = GetComponentInChildren<CinemachineFreeLook>();
        minEnginePitch = engineAudio.pitch;
        rb = GetComponent<Rigidbody>();

        wheelBase = Vector3.Distance(frontWheels[0].transform.position, backWheels[0].transform.position);
    }
    private void FixedUpdate()
    {
        //if(currentCarController != null)
        //{
        //    handBrake = true;
        //    return;
        //}

        //calculate current speed in relation to the forward direction of the car
        forwardSpeed = Vector3.Dot(transform.forward, rb.velocity);
        
        //calculate how close the car is to top speed
        float speedFactor = Mathf.InverseLerp(0, topSpeed, forwardSpeed);

        //calculate how much torque is available 
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        //calculate how much to steer 
        currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check if the users input is in the same direction as the cars velocity
        bool isAccelerating = Mathf.Sign(throttleInput) == Mathf.Sign(forwardSpeed);

        foreach (WheelCollider wheel in frontWheels)
        {
            // Apply steering to Wheel colliders
            wheel.steerAngle = steeringDirectionInput * currentSteerRange;

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders
                wheel.brakeTorque = 0;
            }
            else
            {
                // apply brakes to all wheels when throttle is negative
                wheel.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
            }
        }
        foreach (WheelCollider wheel in backWheels)
        {
            if (handBrake)
            {
                wheel.brakeTorque = brakeTorque;
                return;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders
                wheel.motorTorque = throttleInput * currentMotorTorque;
                wheel.brakeTorque = 0;
            }
            else
            {
                // apply brakes to all wheels when throttle is negative
                wheel.brakeTorque = Mathf.Abs(throttleInput) * brakeTorque;
                wheel.motorTorque = 0;
            }
        }
     

    }

    private void Update()
    {
        //sound and pitch of the engine
        engineAudio.pitch = Mathf.Min(Mathf.Lerp(engineAudio.pitch, minEnginePitch + forwardSpeed, Time.deltaTime / 3), 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        {
            if (currentCarController != null)
            {
                currentCarController.NextCheckpoint(other.transform);
            }
        }
    }
}
