using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    public Track[] tracks;

    public GameObject raceManager;
    public GameObject chaseManager;

    public Light sun;

    [Header("Daytime")]
    public Material dayBox;
    public float fogDensityDay;
    public float environmentIntensityDay;
    public float environmentReflectionIntensityDay;
    public Color fogColorDay;

    [Header("Nighttime")]
    public Material nightBox;
    public float fogDensityNight;
    public float environmentIntensityNight;
    public float environmentReflectionIntensityNight;
    public Color fogColorNight;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        raceManager.gameObject.SetActive(GameManager.INSTANCE.currentLevel.levelType == LevelType.RACER);
        chaseManager.gameObject.SetActive(GameManager.INSTANCE.currentLevel.levelType == LevelType.CHASER);
        SetTimeOfDay();
    }

    public void SetTimeOfDay()
    {
        if(GameManager.INSTANCE.currentLevel.timeOfDay == TimeOfDay.DAY)
        {
            sun.intensity = 1;
            RenderSettings.skybox = dayBox;
            RenderSettings.fogDensity = fogDensityDay;
            RenderSettings.fogColor = fogColorDay;
            RenderSettings.ambientIntensity = environmentIntensityDay;
            RenderSettings.reflectionIntensity = environmentReflectionIntensityDay;
        }
        if (GameManager.INSTANCE.currentLevel.timeOfDay == TimeOfDay.NIGHT)
        {
            sun.intensity = 0;
            RenderSettings.skybox = nightBox;
            RenderSettings.fogDensity = fogDensityNight;
            RenderSettings.fogColor = fogColorNight;
            RenderSettings.ambientIntensity = environmentIntensityNight;
            RenderSettings.reflectionIntensity = environmentReflectionIntensityNight;

        }
    }
}
