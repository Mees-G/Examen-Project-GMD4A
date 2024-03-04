using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlcoholManager : MonoBehaviour
{

    public int liters;
    public GameObject alcoholPrefab;
    public Transform alcoholSpawnPoint;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AttemptToLoseAlcohol();
        }
    }

    public void AttemptToLoseAlcohol()
    {
        if (liters > 0) {
            GameObject alcoholInstaniate = Instantiate(alcoholPrefab, alcoholSpawnPoint.transform.position, Random.rotation);
            GameObject currentCar = GameManager.INSTANCE.currentCar;

            Rigidbody rigidbody = alcoholInstaniate.GetComponent<Rigidbody>();
            //Physics.IgnoreCollision(alcoholInstaniate.GetComponent<Collider>(), GameManager.INSTANCE.currentCar.GetComponent<Collider>());

            rigidbody.velocity = -transform.forward * 2;
            rigidbody.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            liters = Mathf.Max(liters - 1, 0);
        }
    }
}
