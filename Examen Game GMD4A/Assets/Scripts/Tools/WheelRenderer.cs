using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRenderer : MonoBehaviour
{
    private WheelCollider wheelCollider;
    
    private void Start()
    {
        wheelCollider = GetComponentInParent<WheelCollider>();
    }

    private void FixedUpdate()
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);

        transform.position = pos;
        transform.rotation = rot;
    }
}
