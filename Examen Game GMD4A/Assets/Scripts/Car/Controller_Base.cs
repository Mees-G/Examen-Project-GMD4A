using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Controller_Base : MonoBehaviour
{
    [HideInInspector]public bool NPC;
    protected Vector3 driveTarget;
    [Tooltip("What car does this control")] public Car car;

    public RaceTrack track;
    public Transform checkpointToReach;

    bool carRespawning;
    public virtual void NextCheckpoint(Transform checkpoint)
    {
        Debug.Log("checkpoint Triggered");

        if (checkpoint == checkpointToReach)
        {
            Debug.Log("checkpoint reached");

            if (checkpointToReach == track.checkpoints.Last())
            {
                Debug.Log("last checkpoint reached (finish)");
                RacerManager.instance.ParticipantFinished(this);
            }
            else
            {
                Debug.Log(track.checkpoints.IndexOf(checkpoint) + 1);
                checkpointToReach = track.checkpoints[track.checkpoints.IndexOf(checkpoint) + 1];
                
                if(NPC)
                {
                    driveTarget = checkpointToReach.position;
                }
            }
        }

        RacerManager.instance.UpdatePlacement(checkpoint, this);
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
        }

        if(respawnCheckpoint != null )
        {
            StartCoroutine(CollisionControl());

            car.collisionComponent.gameObject.layer = LayerMask.NameToLayer("Car_Body_NoCollision");
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
        int i = 0;

        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Increase the number of Colliders in the array
            i++;
        }

        if (i == 0)
        {
            carRespawning = false;
        }
    }

    IEnumerator CollisionControl()
    {
        yield return new WaitForSecondsRealtime(5);

        yield return new WaitUntil(()=> !carRespawning);

        car.collisionComponent.gameObject.layer = LayerMask.NameToLayer("Car_Body");
    }
}
