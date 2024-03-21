using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorGamesceneOverride : MonoBehaviour
{
    public float timeScale = 1;

    public void SpeedTime()
    {
        Time.timeScale++;
        timeScale= Time.timeScale;
    }
    public void SlowTime()
    {
        Time.timeScale--;
        timeScale= Time.timeScale;
    }
    public void RecalculateNormals()
    {
        //NormalSolver.RecalculateNormals();
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

        if (GUILayout.Button("Speed Time + 1"))
        {
            yourScript.SpeedTime();
        }
        if (GUILayout.Button("Slow Time - 1"))
        {
            yourScript.SlowTime();
        }
    }
}
#endif
