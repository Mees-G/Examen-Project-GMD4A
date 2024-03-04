using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    private static GameManager _INSTANCE;
    public static GameManager INSTANCE
    {
        get
        {
            if (_INSTANCE == null)
            {
                _INSTANCE = new GameManager();
            }
            return _INSTANCE;
        }
    }

    public GameObject currentCar;

}
