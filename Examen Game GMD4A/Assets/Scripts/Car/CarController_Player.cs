using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController_Player : Controller_Base
{
    public static CarController_Player instance;
    public PlayerInput playerInput;

    float smoothVelocity = 0.1f;

    private CarDamage carDamage;
    private float targetSteeringInput;
    private void Awake()
    {
        instance = this;
        //playerInput.DeactivateInput();
    }

    public override void OnSetCar()
    {
        carDamage = car.GetComponent<CarDamage>();
        carDamage.OnDeath += OnDeath;
    }

    private void OnDeath(Car car)
    {
        modeManager.EndGame();
    }

    private void Update()
    {
        if(car && car.cameraControl != null)
        {
            car.cameraControl.enabled = true;
        }
    }

    public override void FixedUpdate()
    {
        if (car == null || car.rb == null)
            return;

        base.FixedUpdate();

        if (car.throttleInput >= 0.9f && car.forwardSpeed < 0f)
        {
            GameUI.instance.StuckNotification(true);
        }
        GameUI.instance.StuckNotification(false);

        car.steeringDirectionInput = Mathf.Clamp(Mathf.Lerp(car.steeringDirectionInput, targetSteeringInput, 0.2f), -1f, 1f);
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
            playerInput.DeactivateInput();
        }
    }

    //Controls
    public void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();

        car.throttleInput = inputVector.y;
        targetSteeringInput = inputVector.x;

    }
    public void OnHandBrake(InputValue value)
    {
        if(!car.handBrake) car.handBrake = value.isPressed;
        else car.handBrake = false;
    }
    public void OnLights()
    {
        car.lights.SetActive(!car.lights.activeInHierarchy);
    }
    public void OnRespawn()
    {
        RespawnCar();
    }
    public void OnLookBack()
    {
        //Debug.Log("Looking back");
        //CinemachineTransposer.BindingMode originalBindingmode = car.cameraControl.m_BindingMode;
        //car.cameraControl.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        //car.cameraControl.m_XAxis.Value = 180;
    }
}