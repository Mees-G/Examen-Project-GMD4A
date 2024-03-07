using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller_Base : MonoBehaviour
{
    [HideInInspector]public bool NPC;
    [Tooltip("What car does this control")] public Car car;

    public Track track;

    private void Start()
    {
        car.currentCarController = this;
    }
    public virtual void NextCheckpoint(Transform checkpoint)
    {

    }
    public abstract void ActivateControl();
}
