using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UILevelPathHandler : MonoBehaviour
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
    public ScrollRect mapHolderScrollRect;
    public RectTransform mapRectTransform;
    public RectTransform contentPanel;
    public Transform controllerCursor;
    public bool isMapFocused;
    public GraphicRaycaster graphicRaycaster;
    private List<RaycastResult> graphicRaycastResults;

    private Input playerInput;

    private Vector2 startAnimationPosition;
    private int UI_LAYER;


    [SerializeField]
    private bool _shouldDoAnimation;
    public bool shouldDoAnimation
    {
        get
        {
            return _shouldDoAnimation;
        }
        set
        {
            _shouldDoAnimation = value;
            if (_shouldDoAnimation)
            {
                timer = 0;
                startAnimationPosition = car.transform.localPosition;
            }
        }
    }


    private void OnEnable()
    {
        playerInput = new Input();
        playerInput.UI.Enable();
    }

    public void OnClickMap(InputAction.CallbackContext context)
    {
        if (isMapFocused)
        {
            if (context.performed)
            {
                Debug.Log("Aaahh");
                for (int i = 0; i < transform.childCount; i++)
                {
                    RectTransform buttonRect = transform.GetChild(i).GetComponent<RectTransform>();
                    if (buttonRect.rect.Contains(buttonRect.InverseTransformPoint(controllerCursor.position)))
                    {
                        buttonRect.GetComponent<Button>().Select();
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        playerInput.UI.Disable();
        playerInput.UI.Submit.performed -= OnClickMap;
    }

    public void Start()
    {
        Vector2 startPoint = GameManager.INSTANCE.latestLevelPosition;

        car.transform.localPosition = startPoint;

        missionOutOfText.text = (LevelManager.INSTANCE.completedLevelCount) + "/" + gameObject.transform.childCount;

        if (GameManager.INSTANCE.latestLevelPosition != Vector3.zero)
        {
            car.transform.position = GameManager.INSTANCE.latestLevelPosition;
        }
        UpdateLevelButtons();
    }

    private void Update()
    {
        moneyText.text = CurrencyManager.SYMBOL + CurrencyManager.INSTANCE.amount;
        if (shouldDoAnimation)
        {
            Vector2 previousPosition = car.transform.position;
            Vector2 startPoint = startAnimationPosition;
            Vector2 endPoint = GameManager.INSTANCE.latestLevelPosition;
            Debug.Log(startPoint + " - " + endPoint);

            //car pathing
            timer = Mathf.Min(timer + Time.deltaTime * 0.15f, 1);


            if (Vector3.Distance(startPoint, endPoint) != 0)
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




        if (isMapFocused)
        {
         

            graphicRaycastResults = new List<RaycastResult>();
            PointerEventData ed = new PointerEventData(EventSystem.current);
            ed.position = (playerInput.UI.Point.ReadValue<Vector2>());
            graphicRaycaster.Raycast(ed, graphicRaycastResults);
            foreach(RaycastResult result in graphicRaycastResults)
            {   
                controllerCursor.position = result.worldPosition;
            }

            Vector2 inputVector = playerInput.UI.Move.ReadValue<Vector2>();
            Vector3 added = inputVector * Time.deltaTime * 500;
            controllerCursor.localPosition += added;

            if (Mathf.Abs(controllerCursor.localPosition.x) >= mapRectTransform.rect.width / 2 || Mathf.Abs(controllerCursor.localPosition.y) >= mapRectTransform.rect.height / 2)
            {
                /*contentPanel.anchoredPosition = (Vector2)mapHolderScrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)mapHolderScrollRect.transform.InverseTransformPoint(controllerCursor.transform.position);
                controllerCursor.transform.localPosition = Vector2.zero;*/
                contentPanel.localPosition -= added;
                //undo pos
                controllerCursor.localPosition -= added;
            }


        }

    }

    private void UpdateLevelButtons()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            UILevelSelectButton levelSelectButton = gameObject.transform.GetChild(i).GetComponent<UILevelSelectButton>();
            Button button = gameObject.transform.GetChild(i).GetComponent<Button>();
            LevelType indexedLevelType = levelSelectButton.level.levelType;
            levelSelectButton.completedImage.gameObject.SetActive(levelSelectButton.level.completed);
            levelSelectButton.pinTop.color = indexedLevelType == LevelType.RACER ? Color.red : Color.blue;
            button.interactable = (levelSelectButton.level.parentLevel == null || levelSelectButton.level.parentLevel.completed)/* && indexedLevelType == currentLevelType*/;
            Debug.Log(i + " - " + button.interactable);

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

    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }

    public void SetMapFocused()
    {
        isMapFocused = true;
        Debug.Log("AAAA");
        playerInput.UI.Submit.performed += OnClickMap;
    }
}
