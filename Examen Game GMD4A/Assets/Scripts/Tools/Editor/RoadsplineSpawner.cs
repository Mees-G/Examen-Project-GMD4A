using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Pinwheel.Griffin.SplineTool;
using UnityEngine.Splines;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System.Linq;
using System;
using static UnityEngine.Rendering.HableCurve;

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

    public void InstantiateSplineA()
    {
        //List<GSplineAnchor> anchors = griffinSpline.Spline.Anchors.ToList();
        //List<GSplineSegment> segments = griffinSpline.Spline.Segments;

        //for (int i = 0; i < anchors.Count; i++)
        //{
        //    int[] segmentsIndices = griffinSpline.Spline.SmoothTangents(i);

        //    Spline unitySpline = unitySplineContainer.AddSpline();

        //    for (int x = 0; x < segmentsIndices.Length; x++)
        //    {
        //        BezierKnot knot = new BezierKnot();

        //        knot.Position = anchors.IndexOf(x).Position + offset;
        //        knot.Rotation = segmentsIndices[x].Rotation;

        //        unitySpline.Add(knot);

        //    }

        //    unitySpline.SetTangentMode(TangentMode.AutoSmooth);
        //}
    }
    public void InstantiateSpline()
    {
        List<GSplineAnchor> anchors = griffinSpline.Spline.Anchors.ToList();
        List<GSplineSegment> segments = griffinSpline.Spline.Segments;

        int[] anchorRanks = new int[anchors.Count];
        Vector3[] directions = new Vector3[anchors.Count];
        float[] segmentLengths = new float[segments.Count];

        Spline unitySpline = unitySplineContainer.AddSpline();

        for (int i = 0; i < segments.Count; ++i)
        {
            GSplineSegment s = segments[i];
            anchorRanks[s.StartIndex] += 1;
            anchorRanks[s.EndIndex] += 1;

            GSplineAnchor aStart = anchors[s.StartIndex];
            GSplineAnchor aEnd = anchors[s.EndIndex];

            //Vector3 startToEnd = aEnd.Position - aStart.Position;
            //Vector3 d = Vector3.Normalize(startToEnd);
            //directions[s.StartIndex] += d;
            //directions[s.EndIndex] += d;

            //segmentLengths[i] = startToEnd.magnitude;

            BezierKnot knot = new BezierKnot();

            knot.Position = anchors[anchors.IndexOf(aStart)].Position + offset;
            knot.Rotation = anchors[anchors.IndexOf(aStart)].Rotation;

            unitySpline.Add(knot);

            

            //for (int x = 0; x < segmentLengths.Length; x++)
            //{
                
            //}

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
