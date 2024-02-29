using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public Car car;

    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();

        car.throttle = inputVector.y;
        car.steeringDirection = inputVector.x;
    }
}