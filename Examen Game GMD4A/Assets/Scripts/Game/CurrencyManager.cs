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

    public int amount;
    public const double multiplier = 1.5;

    public int ConvertAlcoholToMoney(double liters)
    {
        return (int) (liters * multiplier);
       
    }

}
