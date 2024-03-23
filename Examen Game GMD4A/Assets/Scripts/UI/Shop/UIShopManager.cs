using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UIShopManager : MonoBehaviour
{

    [Header("Parents")]
    public RectTransform buyableCarsParent;

    [Header("Prefabs")]
    public UIBuyableCar uiBuyableCarPrefab;
    public UIUpgrade uiUpgradePrefab;

    [Header("Car Data & Camera Data")]
    public Buyable[] buyableCars;
    public CarCameraData[] cameraCarData;


    [Header("Referenced Panels & Objects")]
    public GameObject confirmPanel;
    public ScrollRect scrollRect;
    public AnimationCurve scrollAnimation;
    public GameObject confirmMapPanel;
    public UILevelPathHandler uiLevelPathHandler;

    public Transform mapCameraTransform;
    public GameObject upgradePanel;

    //for start press any key thing
    public GameObject pressAnyKeyPanel;
    public VolumeProfile profile;

    [Header("Buttons")]
    public Button buyButton;
    public Button buttonLeft;
    public Button buttonRight;

    [Header("Text")]
    public TMP_Text coinAmount;
    public TMP_Text buyPriceText;


    //Instansiated car UI buttons
    private UIBuyableCar[] uiBuyableCars;

    //press any key
    private CanvasGroup canvasGroup;
    private bool hasPressedAny;
    private bool initializedAnimation;
    private Input input;


    private Action onFinishMapAnimation = delegate { };

    private int _currentEquiped = 0;
    public int currentEquiped
    {
        set
        {
            if (_currentEquiped >= 0)
                uiBuyableCars[_currentEquiped].border.SetActive(false);
            _currentEquiped = value;
            uiBuyableCars[_currentEquiped].border.SetActive(true);
            GameManager.INSTANCE.currentCar = buyableCars[_currentEquiped];

        }
        get { return _currentEquiped; }
    }

    private int _index = -1;
    public int index
    {
        get
        {
            return _index;
        }
        set
        {
            if (_index != value)
            {
                confirmMapPanel.SetActive(false);
                confirmPanel.SetActive(false);
                //if (!doingAnimation)
                {
                    _index = value % buyableCarsParent.transform.childCount;
                    if (_index < 0) _index = buyableCarsParent.transform.childCount - 1;
                    StopAllCoroutines();
                    StartCoroutine(SmoothScrollToObject(_index));
                    Camera.main.SmoothToTransform(this, cameraCarData[_index].cameraTransform, scrollAnimation);
                }

                if (upgradePanel.transform.childCount != 0)
                {
                    for (int i = 0; i < upgradePanel.transform.childCount; i++)
                    {
                        Transform child = upgradePanel.transform.GetChild(i);
                        Destroy(child.gameObject);
                    }
                }

                //populate upgrades panel
                upgradePanel.SetActive(buyableCars[_index].unlocked);
                if (buyableCars[_index].unlocked)
                {
                    // Button previousButton = null;
                    foreach (Upgrade upgrade in buyableCars[_index].upgrades)
                    {
                        UIUpgrade upgradeUI = Instantiate(uiUpgradePrefab, upgradePanel.transform);
                        upgradeUI.upgrade = upgrade;

                        Button upgradeButton = upgradeUI.upgradeButton;
                        /* Navigation navigation = upgradeButton.navigation;
                         navigation.mode = Navigation.Mode.Explicit;
                         navigation.selectOnLeft = previousButton;
                         navigation.selectOnDown = uiBuyableCars[_index].button;

                         //set previous
                         if (previousButton != null)
                         {
                             Navigation previousNavigation = previousButton.navigation;
                             previousNavigation.mode = Navigation.Mode.Explicit;
                             previousNavigation.selectOnRight = upgradeButton;
                             previousButton.navigation = previousNavigation;
                         }

                         upgradeButton.navigation = navigation;
                         previousButton = upgradeButton;*/
                    }
                }


            }
            else
            {
                if (!buyableCars[_index].unlocked)
                {
                    Debug.Log("Buy/Select/ETC");
                    buyButton.interactable = buyableCars[_index].price <= CurrencyManager.INSTANCE.amount;
                    buyPriceText.text = buyableCars[_index].price.ToString();
                    confirmPanel.SetActive(true);
                    confirmPanel.GetComponentInChildren<Button>().Select();
                    HorizontalLayoutGroup layout = buyPriceText.transform.parent.GetComponent<HorizontalLayoutGroup>();

                    //bug fix
                    layout.CalculateLayoutInputHorizontal();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(buyPriceText.transform.parent as RectTransform);
                }
                else
                {
                    currentEquiped = _index;
                    confirmMapPanel.SetActive(true);
                    confirmMapPanel.GetComponentInChildren<Button>().Select();
                }
            }
        }
    }


    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        pressAnyKeyPanel.SetActive(GameManager.INSTANCE.isFirstLaunch);
        canvasGroup.alpha = GameManager.INSTANCE.isFirstLaunch ? 0 : 1;
        if (profile.TryGet(out DepthOfField depth))
        {
            depth.active = GameManager.INSTANCE.isFirstLaunch;
        }
        this.InitializeUI();
    }

    private void OnDestroy()
    {
        CurrencyManager.INSTANCE.OnChangeAmount -= OnChangeMoneyAmount;
        SelectedButtonOutlineManager.INSTANCE.input.UI.Any.performed -= OnAnyPressed;
        SelectedButtonOutlineManager.INSTANCE.input.UI.Disable();
    }

    private void Awake()
    {
        SelectedButtonOutlineManager.INSTANCE.input.UI.Any.performed += OnAnyPressed;
        SelectedButtonOutlineManager.INSTANCE.input.UI.Enable();

        CurrencyManager.INSTANCE.OnChangeAmount += OnChangeMoneyAmount;
    }

    public void InitializeUI()
    {

        coinAmount.SetValue(this, CurrencyManager.INSTANCE.amount, 1);

        uiBuyableCars = new UIBuyableCar[buyableCars.Length];
        for (int i = 0; i < buyableCars.Length; i++)
        {
            Buyable buyable = buyableCars[i];
            UIBuyableCar carBuyUI = Instantiate(uiBuyableCarPrefab, buyableCarsParent);
            uiBuyableCars[i] = carBuyUI;
            carBuyUI.buyable = buyable;
            carBuyUI.UpdateUI();

            carBuyUI.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                OnClickButton(carBuyUI);
            });
        }

        //uiBuyableCar[_currentEquiped].border.SetActive(true);
    }

    private void Update()
    {
       
        //Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (hasPressedAny)
        {
            if (!initializedAnimation)
            {
                if (profile.TryGet(out DepthOfField depth))
                {
                    depth.active = false;
                }
                pressAnyKeyPanel.SetActive(false);
                index = 0;
                this.UpdateSelectionButtons();
                this.SelectCurrentCarButton();
                initializedAnimation = true;
            }

            if (canvasGroup.alpha != 1)
                canvasGroup.alpha += Time.deltaTime * 2;
        }
    }

    private void OnAnyPressed(InputAction.CallbackContext context)
    {
        //Debug.Log(context.control.device is Mouse);
        //Debug.Log(context.control.device is Gamepad);
        //Debug.Log(context.control.device is Keyboard);

        hasPressedAny = true;
    }

    private IEnumerator SmoothScrollToObject(int index)
    {
        float factor = 0;
        float animationSpeed = 2.0F;
        Canvas.ForceUpdateCanvases();
        Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;

        Vector2 childLocalPosition = buyableCarsParent.GetChild(index).localPosition;
        Vector2 result = new Vector2(
            0 - (viewportLocalPosition.x + childLocalPosition.x),
            0 - (viewportLocalPosition.y + childLocalPosition.y)
        );
        Vector2 startPosition = scrollRect.content.localPosition;
        Vector2 endPosition = result;
        while (factor < 1)
        {
            scrollRect.content.localPosition = Vector2.LerpUnclamped(startPosition, endPosition, scrollAnimation.Evaluate(factor));

            //200 = spacing
            float contentPosition = scrollRect.content.localPosition.x;
            float spaceBetweenItems = 960;
            float itemSize = 300;
            float half = itemSize / 2;


            float f = -(contentPosition + half) / (spaceBetweenItems + itemSize);

            int firstPoint = Mathf.CeilToInt(f);
            int secondPoint = Mathf.FloorToInt(f);

            float extraScale = 0.2F;
            float centeredScale = (f % 1) * extraScale;
            float otherScale = (1 - (f % 1)) * extraScale;


            // uiBuyableCars[affectedIndex2].transform.localScale = new Vector3(1 + scale2, 1 + scale2, 1 + scale2);
            uiBuyableCars[firstPoint].transform.localScale = new Vector3(1 + centeredScale, 1 + centeredScale, 1 + centeredScale);
            uiBuyableCars[secondPoint].transform.localScale = new Vector3(1 + otherScale, 1 + otherScale, 1 + otherScale);

            float add = Time.deltaTime * animationSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
            factor += add;
        }
    }

    private void OnChangeMoneyAmount(int amount)
    {
        coinAmount.SetValue(this, amount, 1);
    }


    public void OnClickButton(UIBuyableCar uiCar)
    {
        index = uiCar.transform.GetSiblingIndex();
    }

    public void BuyCurrentSelected()
    {
        CurrencyManager.INSTANCE.amount -= buyableCars[_index].price;
        buyableCars[_index].unlocked = true;
        uiBuyableCars[_index].UpdateUI();
        confirmPanel.SetActive(false);
    }

    public void OnClickButtonMap(MonoBehaviour monoBehaviour)
    {
        onFinishMapAnimation += uiLevelPathHandler.SetMapFocused;
        Camera.main.SmoothToTransform(monoBehaviour, mapCameraTransform, scrollAnimation, 1, onFinishMapAnimation);
        confirmMapPanel.SetActive(false);
        gameObject.SetActive(false);
        uiLevelPathHandler.GetComponent<CanvasGroup>().interactable = true;


    }

    public void OnClickButtonSkin()
    {

        Buyable buyable = buyableCars[_index];
        SkinManager skinManager = cameraCarData[_index].carObj.GetComponent<SkinManager>();
        int skindex = buyable.meshSkins.IndexOf(skinManager.meshFilter.sharedMesh);
        Debug.Log(skindex);
        skinManager.SetSkin(buyable.meshSkins[(skindex + 1) % buyable.meshSkins.Count]);
    }

    public void SelectCurrentCarButton()
    {
        uiBuyableCars[_index].button.Select();
    }

    public void IncreaseIndex(Button button)
    {
        index++;
        this.UpdateSelectionButtons();
    }

    public void DecreaseIndex(Button button)
    {
        index--;
        this.UpdateSelectionButtons();
    }

    /* sets the buttons left & right from the car selection so they can only target the car button */
    private void UpdateSelectionButtons()
    {
        Navigation navigationL = buttonLeft.navigation;
        navigationL.mode = Navigation.Mode.Explicit;
        navigationL.selectOnRight = uiBuyableCars[_index].button;
        buttonLeft.navigation = navigationL;

        Navigation navigationR = buttonRight.navigation;
        navigationR.mode = Navigation.Mode.Explicit;
        navigationR.selectOnLeft = uiBuyableCars[_index].button;
        buttonRight.navigation = navigationR;
    }

    [Serializable]
    public class CarCameraData
    {
        public Buyable car;
        public GameObject carObj;
        public Transform cameraTransform;
    }


}
