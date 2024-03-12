using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCarPhysics

{
    public static float CornerRadius(Transform car, Transform currentCheckpoint, Transform nextCheckpoint, float wheelbase)
    {
        // Calculate vectors representing the direction of the car before and after the turn
        Vector3 beforeTurnDir = (currentCheckpoint.position - car.position).normalized;
        Vector3 afterTurnDir = (nextCheckpoint.position - currentCheckpoint.position).normalized;

        Debug.DrawLine(car.transform.position, car.transform.position + beforeTurnDir * 15f, Color.blue);
        Debug.DrawLine(car.transform.position, car.transform.position + afterTurnDir * 15f, Color.red);

        // Calculate the angle in radians using the cross product and the arctangent function
        float angleRad = Mathf.Atan2(Vector3.Cross(beforeTurnDir, afterTurnDir).magnitude, Vector3.Dot(beforeTurnDir, afterTurnDir));

        return wheelbase / Mathf.Tan(angleRad / 2f);
    }



    public static float CalculateNormalForce(float carMass)
    {
        // You may need to adjust this based on your specific car and suspension setup
        // For simplicity, assuming a uniform weight distribution here
        return carMass * 9.81f / 4f; // 9.81 m/s^2 is the acceleration due to gravity
    }
}
