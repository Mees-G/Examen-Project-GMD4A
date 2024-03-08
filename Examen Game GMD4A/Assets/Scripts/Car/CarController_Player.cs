using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController_Player : Controller_Base
{
    public static CarController_Player instance;
    public PlayerInput playerInput;

    private void Awake()
    {
        instance = this;
        playerInput.DeactivateInput();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ActivateControl();
    }

    private void Update()
    {
        if(car.cameraControl != null)
        {
            car.cameraControl.enabled = true;
        }
    }

    public override void ActivateControl()
    {
        playerInput.ActivateInput();
    }

    //Controls
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();

        car.throttleInput = inputVector.y;
        car.steeringDirectionInput = inputVector.x;
    }
    public void OnHandBrake(InputValue value)
    {
        if(!car.handBrake) car.handBrake = value.isPressed;
        else car.handBrake = false;
    }
}