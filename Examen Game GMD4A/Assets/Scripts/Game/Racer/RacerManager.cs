using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RacerManager : GameModeManager
{
    public static RacerManager instance;

    public List<Controller_Base> participants;
    public RaceTrack raceTrack;

    public GameObject[] NPCcars;

    public Transform banner;

    public TMP_Text timer;
    public float currentTime;

    public bool finished;

    public int laps = 1;

    private void Awake()
    {
        instance = this;
    }

    public override void SetupGame()
    {
        SpawnParticipants();
        GameUI.instance.SetupLeaderBoard();
    }

    private void Update()
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
            int spawnPosIndex = UnityEngine.Random.Range(0, raceTrack.startPositions.Length);
            Transform spawnPoint = raceTrack.startPositions[spawnPosIndex].startTransform;

            //Mark it as occupied
            raceTrack.startPositions[spawnPosIndex].occupied = true;

            //Instantiate the player's car at the selected start position
            Debug.Log(GameManager.INSTANCE.currentCar.car);
            Debug.Log(CarController_Player.instance);
            Debug.Log(CarSpawner.instance);
            Car plyrCar = CarSpawner.instance.InstantiateCar(GameManager.INSTANCE.currentCar.car, spawnPoint, CarController_Player.instance);

            //Assign the track and car to the player component
            CarController_Player.instance.car = plyrCar;
            CarController_Player.instance.track = raceTrack;
            CarController_Player.instance.checkpointToReach = raceTrack.checkpoints[0];
            participants.Add(CarController_Player.instance);
        }

        //Spawn NPC Participants

        for (int i = 0; i < raceTrack.startPositions.Length; i++)
        {
            if (!raceTrack.startPositions[i].occupied)
            {
                //Spawn a opponent at every free start position
                Transform spawnPos = raceTrack.startPositions[i].startTransform;
                Controller_Base Npc = Instantiate(NpcObject, transform.GetChild(0)).GetComponent<Controller_Base>();
                Car NPCcar = CarSpawner.instance.InstantiateCar(NPCcars[UnityEngine.Random.Range(0, NPCcars.Length)], spawnPos, Npc);

                participants.Add(Npc);

                Npc.car = NPCcar;
                Npc.track = raceTrack;
                Npc.checkpointToReach = raceTrack.checkpoints[0];
            }
        }
    }

    public void UpdatePlacement(Transform checkPoint, Controller_Base participant)
    {
        int newPosition = participants.IndexOf(participant);

        for (int i = 0; i < participants.Count; i++)
        {
            if (raceTrack.checkpoints.IndexOf(participants[i].checkpointToReach) <
                raceTrack.checkpoints.IndexOf(participant.checkpointToReach))
            {
                if (participants.IndexOf(participant) > participants.IndexOf(participants[i]))
                {
                    newPosition = participants.IndexOf(participants[i]);
                }
            }
        }

        participants.RemoveAt(participants.IndexOf(participant));
        participants.Insert(newPosition, participant);

        //Update game UI
        GameUI.instance.UpdateLeaderBoard(participant, newPosition);
    }

    public override void StartGame()
    {
        Debug.Log("vader");
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
