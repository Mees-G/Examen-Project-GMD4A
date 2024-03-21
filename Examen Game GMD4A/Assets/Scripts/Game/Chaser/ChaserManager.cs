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
        gameMode = GameMode.chaser;
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

        if (!allPoliceActive)
        {
            int playerCheckpointIndex = currentTrack.checkpoints.IndexOf(CarController_Player.instance.checkpointToReach);

            for (int i = 0; i < participants.Count; i++)
            {
                allPoliceActive = false;

                if (participants[i] is CarController_NPC)
                {
                    int checkpointIndex = currentTrack.checkpoints.IndexOf(participants[i].checkpointToReach);

                    if (playerCheckpointIndex > checkpointIndex)
                    {
                        participants[i].SwitchControl(true);
                        allPoliceActive = true;
                    }
                }
            }
        }
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
            Car npcCar = CarSpawner.instance.InstantiateCar(currentLevel.NPC_Cars[Random.Range(0, currentLevel.NPC_Cars.Count)], currentTrack.startPositions[i].startTransform, npc);

            participants.Add(npc);
            
            npc.modeManager = this;
            npc.car = npcCar;
            npc.track = currentTrack;

            float closestDistance = Mathf.Infinity;
            Transform newCheckpoint = currentTrack.checkpoints[0];
            for (int index = 0; index < currentTrack.checkpoints.Count; index++)
            {
                if(Vector3.Distance(currentTrack.startPositions[index].startTransform.position, currentTrack.checkpoints[index].position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(currentTrack.startPositions[index].startTransform.position, currentTrack.checkpoints[index].position);
                }
            }

            npc.checkpointToReach = newCheckpoint;
        }
    }
    public override void StartGame()
    {
        CarController_Player.instance.car.handBrake = false;
        CarController_Player.instance.SwitchControl(true);
    }
}
