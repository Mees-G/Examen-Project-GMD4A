using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishUI : MonoBehaviour
{

    [SerializeField]
    private TMP_Text placeText;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text timeText;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowFinishUI(string place, int score, string time)
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        SetPlace(place);
        SetScore(score);
        SetTime(time);
    }

    public void SetPlace(string place)
    {
        placeText.text = "#" + place;
    }

    public void SetScore(int score)
    {
        scoreText.SetValue(this, score, 1.0F);
    }

    public void SetTime(string time)
    {
        timeText.text = time;
    }

    public void ButtonClickContinue()
    {
        GameManager.INSTANCE.currentLevel.completed = true;
        SceneManager.LoadScene("Store");

    }

}