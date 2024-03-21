using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Pinwheel.Griffin.SplineTool;
using UnityEngine.Splines;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System.Linq;
using System;

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
        GSplineAnchor[] anchors = griffinSpline.Spline.Anchors.ToArray();

        Spline unitySpline = unitySplineContainer.AddSpline();
        unitySpline.Resize(anchors.Length);

        for (int i = 0; i < anchors.Length; i++)
        {
            BezierKnot knot = new BezierKnot();
            knot.Position = anchors[i].Position + offset;
            knot.Rotation = anchors[i].Rotation;

            unitySpline.RemoveAt(i); 
            unitySpline.Insert(i, knot);
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
