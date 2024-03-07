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
    public Car InstantiateCar(GameObject car, Transform spawnPosition)
    {
        GameObject spawnedCar = Instantiate(car, spawnPosition.position, spawnPosition.rotation);
        Car carScript = spawnedCar.GetComponent<Car>();

        //Apply upgrades and modifiers

        //carScript.motorTorque = 
        //carScript.brakeTorque = 

        return carScript;
    }
}
