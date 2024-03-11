using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeManager : MonoBehaviour
{
    public GameObject NpcObject;

    public float startCountdown;
    float countDownTime = 10;

    private void Start()
    {
        SetupGame();
    }

    public virtual void Update()
    {
        countDownTime -= Time.deltaTime;
        GameUI.instance.countDownTime.text = Mathf.Floor(countDownTime).ToString();
        if(countDownTime <= 0)
        {
            GameUI.instance.countDownTime.gameObject.SetActive(false);
            StartGame();
        }
    }

    public abstract void SetupGame();
    public abstract void StartGame();
}
