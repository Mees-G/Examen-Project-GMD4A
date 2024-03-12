using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UILevelPathHandler : MaskableGraphic
{
    [Header("Car")]
    public Transform car;
    public AnimationCurve pathingAnimation;

    [Header("UI")]
    public TMP_Text missionOutOfText;
    public TMP_Text moneyText;

    [Header("Map")]
    public Color unlockedColor;
    public float thickness = 10f;
    public bool curved = false;
    public int segments = 5;
    public float timer;
    public AnimationCurve carHummingAnimation;
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public bool shouldDoAnimation;


    protected override void Start()
    {
        if (Application.isPlaying)
        {
            LevelManager.INSTANCE.onChangeLevelIndex += OnChangeLevelIndex;
            LevelManager.INSTANCE.UpdateCurrentLevelIndex();
            Vector2 startPoint = gameObject.transform.GetChild(LevelManager.INSTANCE.currentLevelIndex == 0 ? 0 : LevelManager.INSTANCE.currentLevelIndex - 1).localPosition;

            car.transform.localPosition = startPoint;

            missionOutOfText.text = (LevelManager.INSTANCE.currentLevelIndex) + "/" + LevelManager.INSTANCE.levels.Count;
            moneyText.text = CurrencyManager.SYMBOL + CurrencyManager.INSTANCE.amount;
            SetAllDirty();
        }
    }

    protected override void OnDestroy()
    {
        LevelManager.INSTANCE.onChangeLevelIndex -= OnChangeLevelIndex;
    }

    private void Update()
    {
        if (Application.isPlaying && shouldDoAnimation)
        {
            Vector2 previousPosition = car.transform.position;
            Vector2 startPoint = gameObject.transform.GetChild(LevelManager.INSTANCE.currentLevelIndex == 0 ? 0 : LevelManager.INSTANCE.currentLevelIndex - 1).localPosition;
            Vector2 endPoint = gameObject.transform.GetChild(LevelManager.INSTANCE.currentLevelIndex).localPosition;

            //car pathing
            timer = Mathf.Min(timer + Time.deltaTime * 0.15f, 1);


            if (LevelManager.INSTANCE.currentLevelIndex != 0)
            {
                if (timer != 1)
                {
                    Vector2[] controlPoints = CalculateControlPoints(startPoint, endPoint);
                    Vector2 point = CalculateCubicSplinePoint(controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], pathingAnimation.Evaluate(timer));

                    car.transform.localPosition = point;
                    car.transform.rotation = Quaternion.Lerp(car.transform.rotation, Quaternion.Euler(0, 0, RotatePointTowards(previousPosition, car.transform.position) - 90), Time.deltaTime * 5);
                    float scale = carHummingAnimation.Evaluate(Time.time % 1);
                    car.localScale = new Vector3(scale, scale, scale);
                    Canvas.ForceUpdateCanvases();

                    /*contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                            - (Vector2)scrollRect.transform.InverseTransformPoint(car.transform.position), Time.deltaTime * 5);*/
                }
                else
                {
                    shouldDoAnimation = false;
                }
            }
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        timer = 0;
        Debug.Log("EHYEFWVFEWBIUJFW ");
        for (int i = 0; i < gameObject.transform.childCount - 1; i++)
        {
            Vector2 startPoint = gameObject.transform.GetChild(i).localPosition;
            Vector2 endPoint = gameObject.transform.GetChild(i + 1).localPosition;

            Vector2[] controlPoints = CalculateControlPoints(startPoint, endPoint);
            Vector2[] curvePoints = CubicSpline(controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3]);

            for (int j = 0; j < curvePoints.Length - 1; j++)
            {
                //lines instead of 1 line
                if (j % 2 == 0)
                {
                    Vector2 point1 = curvePoints[j];
                    Vector2 point2 = curvePoints[j + 1];

                    CreateLineSegment(point1, point2, vh, LevelManager.INSTANCE.currentLevelIndex * segments > i * segments ? unlockedColor : color);
                }
            }
        }
    }

    private void OnChangeLevelIndex(int index)
    {
        for (int i = 0; i < gameObject.transform.childCount - 1; i++)
        {
            Button button = gameObject.transform.GetChild(i).GetComponent<Button>();
            button.enabled = i <= index;
        }
    }



    private Vector2[] CalculateControlPoints(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 middlePoint1 = new Vector2(startPoint.x + (curved ? -(endPoint.x - startPoint.x) / 3f : (endPoint.x - startPoint.x) / 3f), startPoint.y);
        Vector2 middlePoint2 = new Vector2(startPoint.x + (curved ? -2f * (endPoint.x - startPoint.x) / 3f : 2f * (endPoint.x - startPoint.x) / 3f), endPoint.y);

        return new Vector2[] { startPoint, middlePoint1, middlePoint2, endPoint };
    }

    private Vector2[] CubicSpline(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3)
    {
        List<Vector2> splinePoints = new List<Vector2>();

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            Vector2 point = Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(P0, P1, t), Vector2.Lerp(P1, P2, t), t), Vector2.Lerp(Vector2.Lerp(P1, P2, t), Vector2.Lerp(P2, P3, t), t), t);
            splinePoints.Add(point);
        }

        return splinePoints.ToArray();
    }

    private Vector2 CalculateCubicSplinePoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        return Vector2.Lerp(Vector2.Lerp(Vector2.Lerp(p0, p1, t), Vector2.Lerp(p1, p2, t), t), Vector2.Lerp(Vector2.Lerp(p1, p2, t), Vector2.Lerp(p2, p3, t), t), t);
    }

    private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh, Color color)
    {
        Vector3 offset = Vector2.zero;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Quaternion point1Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point1, point2) + 90);
        Vector3 p1 = point1Rotation * new Vector3(-thickness / 2, 0) + point1 - offset;
        Vector3 p2 = point1Rotation * new Vector3(thickness / 2, 0) + point1 - offset;

        Quaternion point2Rotation = Quaternion.Euler(0, 0, RotatePointTowards(point2, point1) - 90);
        Vector3 p3 = point2Rotation * new Vector3(-thickness / 2, 0) + point2 - offset;
        Vector3 p4 = point2Rotation * new Vector3(thickness / 2, 0) + point2 - offset;

        // Add the vertices
        vh.AddVert(p1, color, Vector2.zero);
        vh.AddVert(p2, color, Vector2.zero);
        vh.AddVert(p3, color, Vector2.zero);
        vh.AddVert(p4, color, Vector2.zero);

        // Add the triangles
        int vertIndex = vh.currentVertCount - 4;
        vh.AddTriangle(vertIndex, vertIndex + 1, vertIndex + 2);
        vh.AddTriangle(vertIndex + 1, vertIndex + 3, vertIndex + 2);
    }

    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }
}
