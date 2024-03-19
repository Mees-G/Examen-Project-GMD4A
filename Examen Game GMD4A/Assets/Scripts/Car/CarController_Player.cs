using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController_Player : Controller_Base
{
    public static CarController_Player instance;
    public PlayerInput playerInput;

    public Vector3 previousVelocity;

    private void Awake()
    {
        instance = this;
        playerInput.DeactivateInput();
    }

    private void Update()
    {
        //if(car.cameraControl != null)
        //{
        //    car.cameraControl.enabled = true;
        //}
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();

        float difference = Vector3.Distance(car.rb.velocity, previousVelocity);
        float factor = Mathf.Min(difference / (car.topSpeed * Time.deltaTime), 1);
        // if (Vector3.Distance(rb.velocity, previousVelocity) > topSpeed / 5)
        {
            Debug.Log(factor);
        }
        previousVelocity = car.rb.velocity;
    }

    public override void SwitchControl(bool activate)
    {
        if (activate)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.ActivateInput();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
          //  playerInput.DeactivateInput();
        }
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
    public void OnRespawn()
    {
        RespawnCar();
    }
}