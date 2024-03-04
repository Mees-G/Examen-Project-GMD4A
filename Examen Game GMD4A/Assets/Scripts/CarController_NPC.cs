using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Splines;

public class CarController_NPC : MonoBehaviour
{
    public Car car;
    public float detectionDistance;
    [Range(0, 100)] public float agressiveness;

    SplineContainer route;

    void Start()
    {
        
    }

    void Update()
    {
        Detection();
    }

    void Detection()
    {
        
    }

    void Driving()
    {
        car.throttle = 1;
    }
    
}
