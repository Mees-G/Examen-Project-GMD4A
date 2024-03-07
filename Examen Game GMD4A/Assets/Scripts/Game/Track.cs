using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public List<Transform> checkpoints;
    public StartPosition[] startPositions;

    [Serializable]
    public class StartPosition
    {
        public Transform startTransform;
        public bool occupied;
    }
}