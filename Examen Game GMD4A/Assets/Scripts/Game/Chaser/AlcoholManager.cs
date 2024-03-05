using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

public class AlcoholManager : MonoBehaviour
{

    public double liters;
    public GameObject alcoholPrefab;
    public Transform alcoholSpawnPoint;

    //UI
    public TMP_Text coinText;
    private int intitialCoins;

    public GameObject coinPrefab;
    public Transform coinParent;
    public Transform coinEndPoint;
    public AnimationCurve animationSpawn;
    public AnimationCurve animationCollect;
    private float spawnDuration = 0.02f;
    private float collectionDuration = 1.0F;
    private float coinAnimationDuration = 0.5f;

    public List<Coin> coinList = new List<Coin>();

    private void Awake()
    {
        intitialCoins = CurrencyManager.INSTANCE.amount;
        coinText.text = intitialCoins.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AttemptToLoseAlcohol();
        }


        //Coin Spawning

        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(CollectCoins());
        }

        foreach (Coin coin in coinList)
        {
            if (Time.time >= coin.startTime + coinAnimationDuration)
            {
                float factor = (Time.time - (coin.startTime + coinAnimationDuration)) / collectionDuration;
                float evaluated = animationCollect.Evaluate(factor);
                if (factor <= 1)
                {
                    Vector3 position = Vector3.LerpUnclamped(coin.startPosition, coinEndPoint.position, evaluated);
                    Quaternion rotation = Quaternion.LerpUnclamped(coin.startRotation, coinEndPoint.rotation, evaluated);
                    coin.gameObject.transform.position = position;
                    coin.gameObject.transform.rotation = rotation;
                }
                else if (!coin.done)
                {
                    coin.gameObject.transform.position = coinEndPoint.position;
                    coin.gameObject.transform.rotation = coinEndPoint.rotation;
                    coin.done = true;
                    //coinList.Remove(coin);
                    Destroy(coin.gameObject);
                    intitialCoins++;
                    coinText.text = intitialCoins.ToString();
                }
            }
            else
            {
                float factor = (Time.time - coin.startTime) / coinAnimationDuration;
                float evaluated = animationSpawn.Evaluate(factor);
                coin.gameObject.transform.localScale = new Vector3(evaluated, evaluated, evaluated);
            }
        }
    }

    private IEnumerator CollectCoins()
    {
        int convertedToMoney = CurrencyManager.INSTANCE.ConvertAlcoholToMoney(liters);
        Debug.Log(convertedToMoney);
        CurrencyManager.INSTANCE.amount += convertedToMoney;

        float xMiddle = Screen.width / 2;
        float yMiddle = Screen.height / 2;

        for (int i = 0; i < convertedToMoney; i++)
        {
            float randomX = Random.Range(-Screen.height / 8, Screen.height / 8);
            float randomY = Random.Range(-Screen.height / 8, Screen.height / 8);
            GameObject coinInstansiated = Instantiate(coinPrefab, new Vector2(xMiddle + randomX, yMiddle + randomY), Quaternion.Euler(0, 0, Random.Range(-180, 180)), coinParent);
            coinInstansiated.transform.localScale = Vector3.zero;
            coinList.Add(new Coin(coinInstansiated));
            yield return new WaitForSeconds(spawnDuration / convertedToMoney);
        }
    }


    public void AttemptToLoseAlcohol()
    {
        if (liters > 0)
        {
            GameObject alcoholInstaniate = Instantiate(alcoholPrefab, alcoholSpawnPoint.transform.position, Random.rotation);
            GameObject currentCar = GameManager.INSTANCE.currentCar;

            Rigidbody rigidbody = alcoholInstaniate.GetComponent<Rigidbody>();
            //Physics.IgnoreCollision(alcoholInstaniate.GetComponent<Collider>(), GameManager.INSTANCE.currentCar.GetComponent<Collider>());

            rigidbody.velocity = -transform.forward * 2;
            rigidbody.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            liters--;

        }
    }

    public class Coin
    {
        public GameObject gameObject;
        public Vector3 startPosition;
        public Quaternion startRotation;
        public float startTime;
        public bool done;

        public Coin(GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.startPosition = gameObject.transform.position;
            this.startRotation = gameObject.transform.rotation;
            this.startTime = Time.time;
            this.done = false;
        }

    }

}
