using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RacerManager : GameModeManager
{
    public List<Controller_Base> participants;
    public Track raceTrack;

    public GameObject[] NPCcars;
    public GameObject playerCarPrefab;

    public override void SetupGame()
    {
        SpawnParticipants();
    }

    public void SpawnParticipants()
    {
        //Spawn Player

        //Select one of the track's start positions at random
        int spawnPosIndex = Random.Range(0, raceTrack.startPositions.Length);
        Transform spawnPoint = raceTrack.startPositions[spawnPosIndex].startTransform;

        //Mark it as occupied
        raceTrack.startPositions[spawnPosIndex].occupied = true;

        //Instantiate the player's car at the selected start position
        Car plyrCar = CarSpawner.instance.InstantiateCar(playerCarPrefab, spawnPoint);

        CarController_Player.instance.car = plyrCar;
        participants.Add(CarController_Player.instance);

        //Spawn NPC Participants

        for (int i = 0; i < raceTrack.startPositions.Length; i++)
        {
            if (!raceTrack.startPositions[i].occupied)
            {
                //Spawn a opponent at every free start position
                Transform spawnPos = raceTrack.startPositions[i].startTransform;
                Car NPCcar = CarSpawner.instance.InstantiateCar(NPCcars[Random.Range(0, NPCcars.Length)], spawnPos);
                Controller_Base Npc = Instantiate(NpcObject, transform.GetChild(0)).GetComponent<Controller_Base>();

                participants.Add(Npc);

                Npc.car = NPCcar;
                Npc.track = raceTrack;
            }
        }
    }
    public override void StartGame()
    {
        foreach (Controller_Base controller in participants)
        {
            controller.car.handBrake = false;
            controller.ActivateControl();
        }
    }

    public void PlayerFinished()
    {

    }

    public void NPCFinished(CarController_NPC npc)
    {

    }
}
