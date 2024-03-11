using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class RacerManager : GameModeManager
{
    public static RacerManager instance;
    
    public List<Controller_Base> participants;
    public RaceTrack raceTrack;

    public GameObject[] NPCcars;
    public GameObject playerCarPrefab;

    public Transform banner;

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

    public void SpawnParticipants()
    {
        //Spawn Player

        //Select one of the track's start positions at random
        int spawnPosIndex = UnityEngine.Random.Range(0, raceTrack.startPositions.Length);
        Transform spawnPoint = raceTrack.startPositions[spawnPosIndex].startTransform;

        //Mark it as occupied
        raceTrack.startPositions[spawnPosIndex].occupied = true;

        //Instantiate the player's car at the selected start position
        Car plyrCar = CarSpawner.instance.InstantiateCar(playerCarPrefab, spawnPoint, CarController_Player.instance);

        //Assign the track and car to the player component
        CarController_Player.instance.car = plyrCar;
        CarController_Player.instance.track = raceTrack;

        participants.Add(CarController_Player.instance);

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
        foreach (Controller_Base controller in participants)
        {
            controller.checkpointToReach = raceTrack.checkpoints[0];
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
            //player finished UI ding doen
        }
    }
}
