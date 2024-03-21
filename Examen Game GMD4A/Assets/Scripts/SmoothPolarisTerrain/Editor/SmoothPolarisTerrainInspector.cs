using UnityEditor;
using UnityEngine;
using Pinwheel.Griffin;

[CustomEditor(typeof(SmoothPolarisTerrain))]
public class SmoothPolarisTerrainInspector : Editor
{
    private SmoothPolarisTerrain instance;

    private void OnEnable()
    {
        instance = target as SmoothPolarisTerrain;
    }

    public override void OnInspectorGUI()
    {        
        DrawAction();
    }

    private void DrawAction()
    {
        string label = "Action";
        string id = "action" + instance.ToString();

        GEditorCommon.Foldout(label, true, id, () =>
        {
            instance.applySmoothing = EditorGUILayout.Toggle("Auto Apply", instance.applySmoothing);
            instance.angle = EditorGUILayout.FloatField("Angle", instance.angle);

            if (GUILayout.Button("Smooth Terrain"))
            {
                ProcessSmoothing();
            }
        });
    }

    void ProcessSmoothing()
    {
        instance.ProcessSmoothing();
    }
}
