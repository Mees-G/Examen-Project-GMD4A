using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public bool NPC;
    public abstract void NextCheckpoint(Transform checkpoint);
}
