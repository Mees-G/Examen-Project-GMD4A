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
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;
        GameUI.instance.countDownTime.text = Mathf.Floor(timer).ToString();
        if(timer < 0)
        {
            StartGame();
        }
    }

    public abstract void SetupGame();
    public abstract void StartGame();
}
