using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RacerManager : GameModeBase
{
    public static RacerManager instance;

    [Header("Racer variables")]
    public Transform banner;
    public int laps = 1;

    private void Awake()
    {
        instance = this;
        gameMode = LevelType.RACER;
    }

    public override void SetupGame()
    {
        SpawnParticipants();
        GameUI.instance.SetupLeaderBoard();
    }

    public override void Update()
    {
        base.Update();
        if (started && !finished)
        {
            currentTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int remainingSeconds = Mathf.FloorToInt(currentTime % 60);
            string timeFormatted = string.Format("{0}:{1:00}", minutes, remainingSeconds);
            timer.text = timeFormatted;
        }
    }

    public void SpawnParticipants()
    {
        //Spawn Player
        if (!dontSpawnPlayer)
        {
            //Select one of the track's start positions at random
            int spawnPosIndex = UnityEngine.Random.Range(0, currentTrack.startPositions.Length);
            Transform spawnPoint = currentTrack.startPositions[spawnPosIndex].startTransform;

            //Mark it as occupied
            currentTrack.startPositions[spawnPosIndex].occupied = true;

            //Instantiate the player's car at the selected start position
            Car plyrCar = CarSpawner.instance.InstantiateCar(GameManager.INSTANCE.currentCar.car, spawnPoint, CarController_Player.instance);

            //Assign the track and car to the player component
            CarController_Player.instance.car = plyrCar;
            CarController_Player.instance.track = currentTrack;
            CarController_Player.instance.checkpointToReach = currentTrack.checkpoints[0];
            participants.Add(CarController_Player.instance);
        }

        //Spawn NPC Participants

        for (int i = 0; i < currentTrack.startPositions.Length; i++)
        {
            if (!currentTrack.startPositions[i].occupied)
            {
                //Spawn a opponent at every free start position
                Transform spawnPos = currentTrack.startPositions[i].startTransform;
                Controller_Base npc = Instantiate(NpcObject, transform.parent.GetChild(0)).GetComponent<Controller_Base>();
                Car npcCar = CarSpawner.instance.InstantiateCar(currentLevel.NPC_Cars[UnityEngine.Random.Range(0, currentLevel.NPC_Cars.Count)], spawnPos, npc);

                participants.Add(npc);

                npc.modeManager = this;
                npc.car = npcCar;
                npc.track = currentTrack;
                npc.checkpointToReach = currentTrack.checkpoints[0];
            }
        }
    }

    public override void StartGame()
    {
        foreach (Controller_Base controller in participants)
        {
            controller.car.handBrake = false;
            controller.SwitchControl(true);
        }
    }

    public void ParticipantFinished(Controller_Base participant)
    {
        //if multiple laps
        participant.SwitchControl(false);
        if (!participant.NPC)
        {
            int placent = participants.IndexOf(CarController_Player.instance);

            //  float time = ;
            int score = (int)((participants.Count - placent) * ((60 * 8) - currentTime));

            //  finishUI.ShowFinishUI();
            //GameUI.FinishUI. TODO

            GameUI.instance.finishUI.ShowFinishUI(placent.ToString(), score, timer.text);
            finished = true;
        }
    }
}
