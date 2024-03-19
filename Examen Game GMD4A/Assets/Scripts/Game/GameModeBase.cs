using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public abstract class GameModeBase : MonoBehaviour
{
    [Header("GameMode Manager")]
    public GameObject NpcObject;
    public Track currentTrack;
    public Track[] raceTracks;
    [HideInInspector] public bool started = false;
    public Level currentLevel;

    [Header("Timer")]
    public TMP_Text timer;
    public float currentTime;
    public float countDownTime = 10;

    [Header("Debugging")]
    public bool dontSpawnPlayer;
    public List<Controller_Base> participants;
    protected GameMode gameMode;
    public enum GameMode { racer, chaser }

    private void OnEnable()
    {
        CarController_Player.instance.modeManager = this;

        currentLevel = GameManager.INSTANCE.currentLevel;
        currentTrack = raceTracks[GameManager.INSTANCE.currentLevel.trackIndex];

        if (currentTrack)
            SetupGame();
    }

    public virtual void Update()
    {
        countDownTime -= Time.deltaTime;
        GameUI.instance.countDownTime.text = Mathf.Floor(countDownTime).ToString();
        if (!started && countDownTime <= 0)
        {
            GameUI.instance.countDownTime.gameObject.SetActive(false);
            StartGame();
            started = true;
        }
    }
    public void UpdatePlacement(Transform checkPoint, Controller_Base participant)
    {
        //Update game UI
        switch (gameMode)
        {
            case GameMode.racer:
                int newPosition = participants.IndexOf(participant);

                for (int i = 0; i < participants.Count; i++)
                {
                    if (currentTrack.checkpoints.IndexOf(participants[i].checkpointToReach) <
                        currentTrack.checkpoints.IndexOf(participant.checkpointToReach))
                    {
                        if (participants.IndexOf(participant) > participants.IndexOf(participants[i]))
                        {
                            newPosition = participants.IndexOf(participants[i]);
                        }
                    }
                }

                participants.RemoveAt(participants.IndexOf(participant));
                participants.Insert(newPosition, participant);

                GameUI.instance.UpdateLeaderBoard(participant, newPosition);
                break;

            case GameMode.chaser:
                //TODO: change music ofzo
                break;
        }
    }
    public abstract void SetupGame();
    public abstract void StartGame();
}