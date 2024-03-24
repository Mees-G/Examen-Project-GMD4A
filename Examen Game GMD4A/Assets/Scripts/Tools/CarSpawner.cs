using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public static CarSpawner instance;

    private void Awake()
    {
        instance = this;
    }
    public Car InstantiateCar(GameObject car, Transform spawnPosition, Controller_Base controller)
    {
        GameObject spawnedCar = Instantiate(car, spawnPosition.position, spawnPosition.rotation);

        Car carScript = spawnedCar.GetComponent<Car>();
        carScript.currentCarController = controller;

        if (GameManager.INSTANCE)
        {
            carScript.lights.SetActive(GameManager.INSTANCE.currentLevel.timeOfDay == TimeOfDay.NIGHT);
        }

        //Apply upgrades and modifiers

        if (controller.NPC)
        {
            if((controller as CarController_NPC).specialNPC)
            {
                carScript.motorTorque = carScript.motorTorque += (carScript.motorTorque * (controller as CarController_NPC).specialNPC.torqueMultiplier);
                carScript.topSpeed = carScript.topSpeed += (carScript.motorTorque * (controller as CarController_NPC).specialNPC.topSpeedMultiplier);
                carScript.brakeTorque = carScript.brakeTorque += (carScript.motorTorque * (controller as CarController_NPC).specialNPC.brakesMultiplier);
            }
            else
            {
                if (GameManager.INSTANCE)
                {
                    carScript.motorTorque = carScript.motorTorque += (carScript.motorTorque * GameManager.INSTANCE.currentLevel.difficultyMultiplier);
                    carScript.topSpeed = carScript.topSpeed += (carScript.motorTorque * GameManager.INSTANCE.currentLevel.difficultyMultiplier);
                    carScript.brakeTorque = carScript.brakeTorque += (carScript.motorTorque * GameManager.INSTANCE.currentLevel.difficultyMultiplier);
                }
            }
            //TODO: Apply NPC modifiers
        }
        else
        {
            carScript.motorTorque += (GameManager.INSTANCE.currentCar.GetUpgradeByName("Transmission").GetValue<int>() * 5);
            carScript.topSpeed += (GameManager.INSTANCE.currentCar.GetUpgradeByName("Engine").GetValue<int>());
            carScript.brakeTorque += (GameManager.INSTANCE.currentCar.GetUpgradeByName("Brakes").GetValue<int>() * 5);
        }

        return carScript;
    }
}
