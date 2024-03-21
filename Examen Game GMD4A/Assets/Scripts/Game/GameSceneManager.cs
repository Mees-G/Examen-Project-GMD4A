using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    public Track[] tracks;

    public GameObject raceManager;
    public GameObject chaseManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        raceManager.gameObject.SetActive(GameManager.INSTANCE.currentLevel.levelType == LevelType.RACER);
        chaseManager.gameObject.SetActive(GameManager.INSTANCE.currentLevel.levelType == LevelType.CHASER);
    }
}
