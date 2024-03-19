using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaserManager : GameModeManager
{
    public static ChaserManager instance;

    [Header("Chaser variables")]
    public GameObject[] pursuitCars;

    //[Header("runtime variables")]

    private void Awake()
    {
        instance = this;
        gameMode = GameMode.chaser;
    }

    public override void SetupGame()
    {
        SpawnCars();
    }

    private void SpawnCars()
    {
        if (!dontSpawnPlayer)
        {
            //spawn player
            //Select one of the track's start positions at random
            Transform spawnPoint = currentTrack.startPositions[0].startTransform;

            //Mark it as occupied
            currentTrack.startPositions[0].occupied = true;

            //Instantiate the player's car at the selected start positionv
            Car plyrCar = CarSpawner.instance.InstantiateCar(GameManager.INSTANCE.currentCar.car, spawnPoint, CarController_Player.instance);

            //Assign the track and car to the player component
            CarController_Player.instance.car = plyrCar;
            CarController_Player.instance.track = currentTrack;
            CarController_Player.instance.checkpointToReach = currentTrack.checkpoints[0];

            participants.Add(CarController_Player.instance);
        }

        //spawn police cars
        for (int i = 1; i < currentTrack.startPositions.Length; i++)
        {
            Controller_Base npc = Instantiate(NpcObject, transform.parent.GetChild(0)).GetComponent<Controller_Base>();
            Car npcCar = CarSpawner.instance.InstantiateCar(pursuitCars[Random.Range(0, pursuitCars.Length)], currentTrack.startPositions[i].startTransform, npc);

            participants.Add(npc);

            npc.modeManager = this;
            npc.car = npcCar;
            npc.track = currentTrack;
            npc.checkpointToReach = currentTrack.checkpoints[0];
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
}
