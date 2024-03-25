using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class CoinCollectionManager : MonoBehaviour
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
    private float spawnDuration = 0.5F;
    private float collectionDuration = 1.0F;
    private float coinAnimationDuration = 0.5f;

    private bool hasCollectedCoins;

    public List<Coin> coinList = new List<Coin>();



    private void Awake()
    {
    }

    private void Start()
    {
        intitialCoins = CurrencyManager.INSTANCE.amount;
        coinText.text = intitialCoins.ToString();
        liters = GameManager.INSTANCE.currentCar.alcoholCapacity;
    }

    public void OnDrop(InputValue value)
    {
        Debug.Log("droppa");
        if (GameManager.INSTANCE.currentLevel.levelType == LevelType.CHASER) {
            AttemptToLoseAlcohol();
        }

    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.R))
        {
            AttemptToLoseAlcohol();
        }*/

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


    public void CollectCoins(UnityEngine.UI.Button button)
    {
        if (!hasCollectedCoins) {
            StartCoroutine(CollectCoins(button.transform.position));
            hasCollectedCoins = true;
        }
    }

    private IEnumerator CollectCoins(Vector2 position)
    {
        int totalCoinAmount = GameManager.INSTANCE.currentLevel.baseEarning;
        if (GameManager.INSTANCE.currentLevel.levelType == LevelType.CHASER)
        {
            totalCoinAmount += CurrencyManager.INSTANCE.ConvertAlcoholToMoney(liters);
        }
        CurrencyManager.INSTANCE.amount += totalCoinAmount;
        float endTime = Time.time + spawnDuration;
        int givenAmount = 0;
        float previousFactor = 0;

        while (Time.time < endTime)
        {
            float difference = endTime - Time.time;
            float factor = 1 - (difference / spawnDuration);
            int currentAmount = (int)(totalCoinAmount * (factor - previousFactor));
            //
            for (int i = 0; i < currentAmount; i++)
            {
                SpawnCoin(position);
                givenAmount++;
            }
            previousFactor = factor;
            yield return new WaitForSeconds(spawnDuration / totalCoinAmount);
        }

        while (givenAmount < totalCoinAmount)
        {
            Debug.Log(givenAmount + " - AAAAAAA");
            SpawnCoin(position);
            givenAmount++;
        }

    }

    public void SpawnCoin(Vector2 position)
    {
        Vector2 spawnAround = position + (Random.insideUnitCircle * 150);
        GameObject coinInstansiated = Instantiate(coinPrefab, spawnAround, Quaternion.Euler(0, 0, Random.Range(-180, 180)), coinParent);
        coinInstansiated.transform.SetSiblingIndex(0);
        coinInstansiated.transform.localScale = Vector3.zero;
        coinList.Add(new Coin(coinInstansiated));
    }


    public void AttemptToLoseAlcohol()
    {
        if (liters > 0)
        {
            GameObject alcoholInstaniate = Instantiate(alcoholPrefab, CarController_Player.instance.car.transform.position, Random.rotation);
            GameObject currentCar = CarController_Player.instance.car.gameObject;

            Rigidbody rigidbody = alcoholInstaniate.GetComponent<Rigidbody>();
            Physics.IgnoreCollision(alcoholInstaniate.GetComponent<Collider>(), currentCar.GetComponentInChildren<Collider>());

            rigidbody.velocity = -currentCar.transform.forward * 5;
            rigidbody.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            liters--;

            //3.0f alcohol weight
            CarController_Player.instance.car.rb.mass -= 3.0F;
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
