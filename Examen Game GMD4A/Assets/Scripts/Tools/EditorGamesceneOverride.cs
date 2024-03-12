using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorGamesceneOverride : MonoBehaviour
{
    public void StartRace()
    {
        RacerManager.instance.dontSpawnPlayer = true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EditorGamesceneOverride))]
public class GamesceneoverrideButtons : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGamesceneOverride yourScript = (EditorGamesceneOverride)target;

        //buttons
        if (GUILayout.Button("Startrace"))
        {
            yourScript.StartRace();
        }
    }
}
#endif
