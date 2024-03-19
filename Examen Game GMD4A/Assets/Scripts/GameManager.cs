using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
    }

    public Buyable currentCar;
    public Level currentLevel;

    //for car anim
    public Vector3 latestLevelPosition;
}
