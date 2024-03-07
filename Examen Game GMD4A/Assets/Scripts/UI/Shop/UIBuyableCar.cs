using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBuyableCar : MonoBehaviour
{
    public Buyable buyable;

    public TMP_Text carNameText;
    public TMP_Text carPriceText;
    public Image carDisplay;

    public Transform lockedTransform;

    public GameObject border;

    public void UpdateUI()
    {
        if (buyable != null)
        {
            carNameText.text = buyable._name;
            carPriceText.transform.parent.gameObject.SetActive(!buyable.unlocked);
            if (!buyable.unlocked) {
                carPriceText.text = buyable.price.ToString();
            }
            lockedTransform.gameObject.SetActive(!buyable.unlocked);
            //carDisplay.sprite = buyable.icon;
        }
    }

}
