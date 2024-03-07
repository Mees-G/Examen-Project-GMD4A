using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager
{

    private static CurrencyManager _INSTANCE;
    public static CurrencyManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null) {
                _INSTANCE = new CurrencyManager();
            }
            return _INSTANCE;
        } 
        set
        {
            _INSTANCE = value;
        }
    }

    private int _amount = 500;
    public int amount
    {
        get { return _amount; } 
        set
        {
            _amount = value;
            OnChangeAmount.Invoke(amount);
        }
    }

    public const double multiplier = 1.5;
    public Action<int> OnChangeAmount;

    public int ConvertAlcoholToMoney(double liters)
    {
        return (int) (liters * multiplier);
       
    }

}
