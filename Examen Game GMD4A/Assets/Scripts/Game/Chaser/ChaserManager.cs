using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ChaserManager : GameModeBase
{
    public static ChaserManager instance;
    bool allPoliceActive;

    private void Awake()
    {
        instance = this;
        gameMode = LevelType.CHASER;
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

        for (int i = 0; i < participants.Count; i++)
        {
            if (participants[i] is CarController_NPC)
            {
                CarController_NPC npc = participants[i] as CarController_NPC;

                if(currentTrack.checkpoints.IndexOf(npc.checkpointToReach) <= currentTrack.checkpoints.IndexOf(CarController_Player.instance.checkpointToReach))
                {
                    npc.SwitchControl(true);
                    npc.car.lights.SetActive(true);
                }
                else
                {
                    npc.SwitchControl(false);
                    npc.car.lights.SetActive(false);
                }
            }
        }
    }

    public override void SetupGame()
    {
        SpawnCars();
    }

    //TODO: this spawncars and racermanager spawncars are nearly identical, it should be in gamemodebase
    private void SpawnCars()
    {
        //spawn player
        if (!editorDebugMode)
        {
            
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
            CarController_Player.instance.gameMode = LevelType.CHASER;

            participants.Add(CarController_Player.instance);

        }

        //spawn special NPC's
        for (int i = 0; i < currentLevel.Special_Npcs.Count; i++)
        {
            if (!currentTrack.startPositions[i].occupied)
            {
                CarController_NPC npc = Instantiate(NpcObject, transform.parent.GetChild(0)).GetComponent<CarController_NPC>();
                npc.specialNPC = currentLevel.Special_Npcs[i];
                Car npcCar = CarSpawner.instance.InstantiateCar(currentLevel.NPC_Cars[Random.Range(0, currentLevel.NPC_Cars.Count)], currentTrack.startPositions[i].startTransform, npc);

                participants.Add(npc);

                npc.modeManager = this;
                npc.car = npcCar;
                npc.track = currentTrack;
                npc.gameMode = LevelType.CHASER;
                currentTrack.startPositions[i].occupied = true;

                float closestDistance = Mathf.Infinity;
                Transform newCheckpoint = currentTrack.checkpoints[0];
                for (int index = 0; index < currentTrack.checkpoints.Count; index++)
                {
                    if (Vector3.Distance(currentTrack.startPositions[i].startTransform.position, currentTrack.checkpoints[index].position) < closestDistance)
                    {
                        closestDistance = Vector3.Distance(currentTrack.startPositions[i].startTransform.position, currentTrack.checkpoints[index].position);
                        newCheckpoint = currentTrack.checkpoints[index];
                    }
                }

                npc.checkpointToReach = newCheckpoint;
            }
        }

        //spawn police cars
        for (int i = 1; i < currentTrack.startPositions.Length; i++)
        {
            if (!currentTrack.startPositions[i].occupied)
            {
                CarController_NPC npc = Instantiate(NpcObject, transform.parent.GetChild(0)).GetComponent<CarController_NPC>();
                Car npcCar = CarSpawner.instance.InstantiateCar(currentLevel.NPC_Cars[Random.Range(0, currentLevel.NPC_Cars.Count)], currentTrack.startPositions[i].startTransform, npc);

                participants.Add(npc);

                npc.modeManager = this;
                npc.car = npcCar;
                npc.track = currentTrack;
                npc.gameMode = LevelType.CHASER;
                currentTrack.startPositions[i].occupied = true;

                float closestDistance = Mathf.Infinity;
                Transform newCheckpoint = currentTrack.checkpoints[0];
                for (int index = 0; index < currentTrack.checkpoints.Count; index++)
                {
                    if (Vector3.Distance(currentTrack.startPositions[i].startTransform.position, currentTrack.checkpoints[index].position) < closestDistance)
                    {
                        closestDistance = Vector3.Distance(currentTrack.startPositions[i].startTransform.position, currentTrack.checkpoints[index].position);
                        newCheckpoint = currentTrack.checkpoints[index];
                    }
                }

                npc.checkpointToReach = newCheckpoint;
            }

        }
    }
    public override void StartGame()
    {
        CarController_Player.instance.car.handBrake = false;
        CarController_Player.instance.SwitchControl(true);
    }
}
