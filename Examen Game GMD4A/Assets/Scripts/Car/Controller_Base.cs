using JetBrains.Annotations;
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

    public virtual void NextCheckpoint(Transform checkpoint)
    {
        if (checkpoint == checkpointToReach)
        {
            if (checkpointToReach == track.checkpoints.Last())
            {
                RacerManager.instance.ParticipantFinished(this);
            }
            else
            {
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
            car.transform.position = new Vector3(respawnCheckpoint.position.x, respawnCheckpoint.position.y + 1, respawnCheckpoint.position.z);
            car.transform.rotation = respawnCheckpoint.rotation;
        }
    }
}
