using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Buyable currentCar;
    public Level currentLevel;
    public Mesh currentSkin;

    //for car anim
    public Vector3 latestLevelPosition;

    //voor pressed zooi
    public bool isFirstLaunch = true;

}
