using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController_NPC : MonoBehaviour
{
    public Car car;
    public float detectionDistance;
    [Range(0, 100)] public float agressiveness;
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
}
