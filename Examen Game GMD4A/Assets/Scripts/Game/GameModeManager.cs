using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GameModeManager : MonoBehaviour
{
    public GameObject NpcObject;

    public float startCountdown;
    public bool started = false;
    public float countDownTime = 10;

    public bool dontSpawnPlayer;

    private void Start()
    {
        SetupGame();
    }

    public void Update()
    {
        countDownTime -= Time.deltaTime;
        GameUI.instance.countDownTime.text = Mathf.Floor(countDownTime).ToString();
        if(!started && countDownTime <= 0)
        {
            GameUI.instance.countDownTime.gameObject.SetActive(false);
            StartGame();
            started = true;
        }
    }

    public abstract void SetupGame();
    public abstract void StartGame();
}
