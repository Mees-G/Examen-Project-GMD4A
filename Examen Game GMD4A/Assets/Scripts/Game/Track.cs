using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public List<Transform> checkpoints;
    public StartPosition[] startPositions;
    public GameObject trackObjects;

    [Serializable]
    public class StartPosition
    {
        public Transform startTransform;
        public bool occupied;
    }
}
