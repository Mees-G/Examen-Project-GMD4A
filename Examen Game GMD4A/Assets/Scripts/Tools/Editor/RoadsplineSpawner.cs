using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Pinwheel.Griffin.SplineTool;
using UnityEngine.Splines;

public class RoadsplineSpawner : EditorWindow
{
    [MenuItem("Ruben/SplineConverter")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<RoadsplineSpawner>();
    }

    public GSplineCreator griffinSpline;
    public SplineContainer unitySplineContainer;
    public Vector3 offset;
    public void InstantiateSpline()
    {
        Spline unitySpline = unitySplineContainer.AddSpline();
        for (int i = 0; i < griffinSpline.Spline.Anchors.Count; i++)
        {
            BezierKnot knot = new BezierKnot();
            knot.Position = griffinSpline.Spline.Anchors[i].Position + offset;
            knot.Rotation = griffinSpline.Spline.Anchors[i].Rotation;
            unitySpline.Add(knot);
        }
        unitySpline.SetTangentMode(TangentMode.AutoSmooth);
    }

    public void OnGUI()
    {
        griffinSpline = EditorGUILayout.ObjectField("Griffin Spline", griffinSpline, typeof(GSplineCreator), true) as GSplineCreator;
        unitySplineContainer = EditorGUILayout.ObjectField("Unity Spline Container", unitySplineContainer, typeof(SplineContainer), true) as SplineContainer;
        offset = EditorGUILayout.Vector3Field("Offset", offset);
        if (GUILayout.Button("Instantiate Spline"))
        {
             InstantiateSpline();
        }
    }
}
