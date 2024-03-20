using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Car_Engine : MonoBehaviour
{
    [Header("Engine Variables")]
    public float maxEngineTorque;

    private Car car;
    public float maxRpm = 2000;
    public float minRpm = 600;
    public AnimationCurve torqueCurve;

    public float[] gearRatios = { 0, 3.5f, 2.5f, 1.8f, 1.3f, 1.0f, -3.0f };

    [Header("Runtime Variables")]
    public int currentGear;
    public float currentRPM;
    public float torqueOutput;

    public float throttleInput;

    [Header("Sounds")]
    public AudioSource engine;
    //public AudioSource engineFx;
    public AudioClip engineIdleAudio;
    public AudioClip engineRevvingAudio;

    private void Start()
    {
        car = GetComponentInParent<Car>();
    }
    private void FixedUpdate()
    {
        // Calculate wheel RPM by averaging the RPMs of all driven wheels
        float totalWheelRPM = 0f;
        foreach (WheelCollider wheel in car.backWheels)
        {
            totalWheelRPM += wheel.rpm;
        }
        float averageWheelRPM = totalWheelRPM / car.backWheels.Length;

        // Convert wheel RPM to engine RPM using gear ratio and differential ratio
        float differentialRatio = 3.42f; // Example differential ratio (adjust as needed)
        float gearRatio = gearRatios[currentGear];
        float engineRPM = averageWheelRPM * gearRatio * differentialRatio;

        // Clamp engine RPM within the allowed range
        engineRPM = Mathf.Clamp(engineRPM, minRpm, maxRpm);

        // Update current RPM
        currentRPM = engineRPM;

        // Calculate throttle-adjusted torque
        float normalizedRPM = currentRPM / maxRpm;
        float torqueMultiplier = throttleInput * torqueCurve.Evaluate(normalizedRPM);
        torqueOutput = maxEngineTorque * torqueMultiplier;

        // Shifting gears
        ShiftingGears();
    }

    private void ShiftingGears()
    {
        float shiftUpThreshold = maxRpm * 0.9f; // RPM threshold for shifting up
        float shiftDownThreshold = maxRpm * 0.6f; // RPM threshold for shifting down

        if (currentGear == 0) // Neutral gear
        {
            if (throttleInput > 0 && currentRPM >= shiftUpThreshold) // Shift to first gear if throttle is applied
            {
                currentGear = 1;
            }
            else if (throttleInput < 0 && currentRPM <= -shiftDownThreshold) // Shift to reverse gear if throttle is applied in opposite direction
            {
                currentGear = gearRatios.Length - 1;
            }
        }
        else if (currentGear == gearRatios.Length - 1) // Reverse gear
        {
            if (throttleInput >= 0) // Shift to neutral if throttle is released or applied in forward direction
            {
                currentGear = 0;
            }
        }
        else // Forward gears
        {
            if (throttleInput > 0 && currentRPM >= shiftUpThreshold) // Shift up if RPM is too high
            {
                currentGear++;
            }
            else if (throttleInput < 0 && currentRPM <= shiftDownThreshold) // Shift down if RPM is too low (for reverse gear)
            {
                currentGear--;
            }
        }
    }

    private void EngineAudio()
    {
        //sound and pitch of the engine
        //engineAudio.pitch = Mathf.Min(Mathf.Lerp(engineAudio.pitch, minEnginePitch + forwardSpeed, Time.deltaTime / 3), 2);
    }
}
