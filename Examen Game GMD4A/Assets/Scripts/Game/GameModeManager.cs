using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeManager : MonoBehaviour
{
    public GameObject NpcObject;

    public float startCountdown;
    float timer;

    private void Start()
    {
        SetupGame();
        StartGame();
    }

    public abstract void SetupGame();
    public abstract void StartGame();
}
