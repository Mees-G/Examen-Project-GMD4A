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

        //Apply upgrades and modifiers

        carScript.motorTorque += (GameManager.INSTANCE.currentCar.GetUpgradeByName("Transmission").GetValue<int>() * 5);
        carScript.topSpeed += (GameManager.INSTANCE.currentCar.GetUpgradeByName("Engine").GetValue<int>());
        carScript.brakeTorque += (GameManager.INSTANCE.currentCar.GetUpgradeByName("Brakes").GetValue<int>() * 10);

        return carScript;
    }
}
