using Pinwheel.Griffin.SplineTool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

[CustomEditor(typeof(InstantiateCheckpoints))]
public class YourScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InstantiateCheckpoints yourScript = (InstantiateCheckpoints)target;

        if (GUILayout.Button("Instantiate Things"))
        {
            yourScript.InstantiateItems();
        }
    }
}

[ExecuteInEditMode]
public class InstantiateCheckpoints : MonoBehaviour
{
    public SplineContainer splineContainer;
    public Transform itemToInstantiate;

    public void InstantiateItems()
    {
        RaceTrack thisTrack = GetComponent<RaceTrack>();

        if (splineContainer != null)
        {
            if(thisTrack.checkpoints.Count > 0)
            {
                foreach (Transform checkpoint in thisTrack.checkpoints)
                {
                    DestroyImmediate(checkpoint.gameObject);
                }
            }
            
            thisTrack.checkpoints.Clear();

            for (int i = 0; i < splineContainer.Spline.Count; i++)
            {
                BezierKnot knotToSpawnAt = splineContainer.Spline[i];
                Vector3 spawnPos = transform.position + new Vector3(knotToSpawnAt.Position.x, knotToSpawnAt.Position.y, knotToSpawnAt.Position.z);

                Transform checkpoint = Instantiate(itemToInstantiate, spawnPos, knotToSpawnAt.Rotation, transform.GetChild(0));
                thisTrack.checkpoints.Add(checkpoint);
            }
        }
    }
}