using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopManager : MonoBehaviour
{

    public RectTransform buyableCarsParent;
    public UIBuyableCar uiBuyableCarPrefab;
    public Buyable[] buyableCars;
    public UIBuyableCar[] uiBuyableCar;
    public CarCameraData[] cameraCarData; 

    public GameObject confirmPanel;
    public ScrollRect scrollRect;
    public AnimationCurve scrollAnimation;

    public Button buyButton;


    public GameObject confirmMapPanel;
    public Transform mapCameraTransform;


    public TMP_Text coinAmount;
    public TMP_Text buyPriceText;

    private Action onFinishMapAnimation = delegate { };


    private int _currentEquiped = -1;
    public int currentEquiped
    {
        set {
            if(_currentEquiped >= 0)
            uiBuyableCar[_currentEquiped].border.SetActive(false);
            _currentEquiped = value;
            uiBuyableCar[_currentEquiped].border.SetActive(true);
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
            if (_index != value) {
                confirmMapPanel.SetActive(false);
                confirmPanel.SetActive(false);
                if (!doingAnimation) {
                    _index = value % buyableCarsParent.transform.childCount;
                    if (_index < 0) _index = buyableCarsParent.transform.childCount - 1;
                    StartCoroutine(SmoothScrollToObject(_index));
                    Camera.main.SmoothToTransform(this, cameraCarData[_index].cameraTransform, scrollAnimation);
                }
            } else
            {
                if (!buyableCars[_index].unlocked) {
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
            float add = Time.deltaTime * animationSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
            factor += add;
        }
        doingAnimation = false;
    }

    private void Awake()
    {
        this.InitializeUI();
    }

    public void InitializeUI()
    {
        coinAmount.SetValue(this, CurrencyManager.INSTANCE.amount, 1);
        uiBuyableCar = new UIBuyableCar[buyableCars.Length];
        for (int i = 0; i < buyableCars.Length; i++)
        {
            Buyable buyable = buyableCars[i];
            UIBuyableCar carBuyUI = Instantiate(uiBuyableCarPrefab, buyableCarsParent);
            uiBuyableCar[i] = carBuyUI;
            carBuyUI.buyable = buyable;
            carBuyUI.UpdateUI();

            carBuyUI.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                OnClickButton(carBuyUI);
            });
        }

        //uiBuyableCar[_currentEquiped].border.SetActive(true);
    }

    public void OnClickButton(UIBuyableCar uiCar)
    {
        index = uiCar.transform.GetSiblingIndex();
    }

    public void BuyCurrentSelected()
    {
        CurrencyManager.INSTANCE.amount -= buyableCars[_index].price;
        coinAmount.SetValue(this, CurrencyManager.INSTANCE.amount, 1);
        buyableCars[_index].unlocked = true;
        uiBuyableCar[_index].UpdateUI();
        confirmPanel.SetActive(false);
    }

    public void OnClickButtonMap()
    {
        Camera.main.SmoothToTransform(this, mapCameraTransform, scrollAnimation, 1, onFinishMapAnimation);
        confirmMapPanel.SetActive(false);
        onFinishMapAnimation += OnLoadedMap;
       
    }

    public void OnLoadedMap()
    {
        Debug.Log("jAWEL");
        gameObject.SetActive(false);
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
