using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : MonoBehaviour
{


    private Upgrade _upgrade;
    public Upgrade upgrade
    {
        get
        {
            return _upgrade;
        }
        set
        {
            _upgrade = value;
            this.UpdateUI();
        }
    }

    public TMP_Text upgradeNameText;
    public TMP_Text upgradeLevelOutofLevelText;
    public TMP_Text upgradePriceText;

    public Image upgradeImage;
    public Button upgradeButton;
    public HorizontalLayoutGroup[] horizontalLayoutGroups;

    public void OnClickButton()
    {
       
        int price = upgrade.CalculateCurrentPrice();
        CurrencyManager.INSTANCE.amount -= price;
        upgrade.currentValue += 1;
        this.UpdateUI();

    }

    private void UpdateUI()
    {
        upgradeNameText.text = upgrade.name;
        upgradeLevelOutofLevelText.text = upgrade.currentValue + "/" + (int)upgrade.upgradeValueMax;

        int price = upgrade.CalculateCurrentPrice();
        upgradePriceText.text = price.ToString();

        if (CurrencyManager.INSTANCE.amount < price || _upgrade.currentValue == _upgrade.upgradeValueMax)
        {
            upgradeButton.interactable = false;
        }

        //bug fix text center
        foreach (HorizontalLayoutGroup horizontalLayoutGroup in horizontalLayoutGroups)
        {
            horizontalLayoutGroup.CalculateLayoutInputHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup.transform as RectTransform);
        }
    }


}
