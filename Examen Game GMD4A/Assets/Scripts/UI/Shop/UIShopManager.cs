using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIShopManager : MonoBehaviour
{

    public RectTransform buyableCarsParent;
    public UIBuyableCar uiBuyableCarPrefab;
    public UIUpgrade uiUpgradePrefab;

    public UIBuyableCar[] uiBuyableCars;
    public Buyable[] buyableCars;
    public CarCameraData[] cameraCarData;

    public GameObject confirmPanel;
    public ScrollRect scrollRect;
    public AnimationCurve scrollAnimation;

    public UnityEngine.UI.Button buyButton;

    public GameObject confirmMapPanel;
    public Transform mapCameraTransform;

    public GameObject upgradePanel;


    public TMP_Text coinAmount;
    public TMP_Text buyPriceText;

    public UILevelPathHandler uiLevelPathHandler;

    private Coroutine lastRoutine;

    private Action onFinishMapAnimation = delegate { };

    private int _currentEquiped = -1;
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

    private bool doingAnimation = false;

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
                    lastRoutine = Camera.main.SmoothToTransform(this, cameraCarData[_index].cameraTransform, scrollAnimation);
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
                    foreach (Upgrade upgrade in buyableCars[_index].upgrades)
                    {
                        UIUpgrade upgradeUI = Instantiate(uiUpgradePrefab, upgradePanel.transform);
                        upgradeUI.upgrade = upgrade;
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
                    HorizontalLayoutGroup layout = buyPriceText.transform.parent.GetComponent<HorizontalLayoutGroup>();

                    //bug fix
                    layout.CalculateLayoutInputHorizontal();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(buyPriceText.transform.parent as RectTransform);
                }
                else
                {
                    currentEquiped = _index;
                    confirmMapPanel.SetActive(true);
                }
            }
        }
    }

    private void Awake()
    {
        this.InitializeUI();
    }

    public void InitializeUI()
    {
        CurrencyManager.INSTANCE.OnChangeAmount += OnChangeMoneyAmount;
        coinAmount.SetValue(this, CurrencyManager.INSTANCE.amount, 1);

        uiBuyableCars = new UIBuyableCar[buyableCars.Length];
        for (int i = 0; i < buyableCars.Length; i++)
        {
            Buyable buyable = buyableCars[i];
            UIBuyableCar carBuyUI = Instantiate(uiBuyableCarPrefab, buyableCarsParent);
            uiBuyableCars[i] = carBuyUI;
            carBuyUI.buyable = buyable;
            carBuyUI.UpdateUI();

            carBuyUI.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate ()
            {
                OnClickButton(carBuyUI);
            });
        }

        //uiBuyableCar[_currentEquiped].border.SetActive(true);
    }

    private IEnumerator SmoothScrollToObject(int index)
    {
        doingAnimation = true;
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
        doingAnimation = false;
    }

    private void OnChangeMoneyAmount(int amount)
    {
        coinAmount.SetValue(this, amount, 1);
        Debug.Log("jaja " + amount);
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
        onFinishMapAnimation += OnLoadedMap;
        Camera.main.SmoothToTransform(monoBehaviour, mapCameraTransform, scrollAnimation, 1, onFinishMapAnimation);
        confirmMapPanel.SetActive(false);
        gameObject.SetActive(false);

    }

    public void OnLoadedMap()
    {
        //do map animation
        uiLevelPathHandler.shouldDoAnimation = true;
    }

    public void IncreaseIndex()
    {
        index++;
    }

    public void DecreaseIndex()
    {
        index--;
    }

    [Serializable]
    public class CarCameraData
    {
        public Buyable car;
        public Transform cameraTransform;
    }


}
