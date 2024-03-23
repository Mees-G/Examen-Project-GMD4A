using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Controller_Base : MonoBehaviour
{
    public GameModeBase modeManager;

    [HideInInspector]public bool NPC;
    protected Vector3 driveTarget;

    [SerializeField]
    [Tooltip("What car does this control")]
    private Car _car;

    public Car car
    {
        get {
            return _car;
        } 
        set
        {
            _car = value;
            this.OnSetCar();
        }
    }

    public Track track;
    public Transform checkpointToReach;
    public int lapIndex;

    bool carRespawning;

    public LevelType gameMode;

    public virtual void NextCheckpoint(Transform checkpoint)
    {
        if (checkpoint == checkpointToReach)
        {
            if (this is CarController_Player)
                checkpointToReach.GetChild(0).gameObject.SetActive(false);

            if (checkpoint == track.checkpoints.Last())
            {
                checkpointToReach = track.checkpoints.First();
                lapIndex++;
                if (lapIndex >= RacerManager.instance.laps)
                {
                    RacerManager.instance.ParticipantFinished(this);
                }
            }
            else
            {
                checkpointToReach = track.checkpoints[track.checkpoints.IndexOf(checkpoint) + 1];
            }

            if (this is CarController_Player && checkpointToReach)
                checkpointToReach.GetChild(0).gameObject.SetActive(true);

            driveTarget = checkpointToReach.position;
        }

        modeManager.UpdatePlacement(checkpoint, this);
    }
    public abstract void SwitchControl(bool activate);
    public virtual void RespawnCar()
    {
        Transform respawnCheckpoint;

        if (checkpointToReach == track.checkpoints.First())
        {
            respawnCheckpoint = track.checkpoints.Last();
        }
        else
        {
            respawnCheckpoint = track.checkpoints[track.checkpoints.IndexOf(checkpointToReach) - 1];

            if (gameMode == LevelType.CHASER)
            {
                if(this is CarController_NPC)
                {
                    respawnCheckpoint = track.checkpoints[track.checkpoints.IndexOf(CarController_Player.instance.checkpointToReach) - 2];
                    checkpointToReach = track.checkpoints[track.checkpoints.IndexOf(respawnCheckpoint) + 1];
                }
            }
        }

        if (respawnCheckpoint != null )
        {
            StartCoroutine(CollisionControl());

            car.collisionComponent.gameObject.layer = LayerMask.NameToLayer("Car_Body_NoCollision");
            car.rb.velocity = new Vector3(0, 0, 0);
            foreach (WheelCollider wheel in car.backWheels) { wheel.rotationSpeed = 0f; }
            foreach (WheelCollider wheel in car.frontWheels) { wheel.rotationSpeed = 0f; }
            car.rb.MovePosition(new Vector3(respawnCheckpoint.position.x, respawnCheckpoint.position.y + 1, respawnCheckpoint.position.z));
            car.rb.MoveRotation(respawnCheckpoint.rotation);
            carRespawning = true;
        }
    }

    public virtual void FixedUpdate()
    {
        if (!carRespawning)
            return;

        Vector3 box = car.collisionComponent.GetComponent<Collider>().bounds.center;
        Collider[] hitColliders = Physics.OverlapBox(car.transform.position, box, car.transform.rotation, LayerMask.NameToLayer("Car_Body"));
        
        int i = hitColliders.Length;
        
        if (i == 0)
        {
            carRespawning = false;
        }
    }


    public virtual void OnSetCar() { }


    IEnumerator CollisionControl()
    {
        yield return new WaitForSecondsRealtime(5);

        yield return new WaitUntil(()=> !carRespawning);

        car.collisionComponent.gameObject.layer = LayerMask.NameToLayer("Car_Body");
    }
}
